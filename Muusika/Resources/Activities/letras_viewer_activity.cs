using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Icu.Text;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cheesebaron.SlidingUpPanel;
using Java.Util.Concurrent;
using Muusika.Resources.Adapters;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;
using Muusika.Resources.Utils;
using Plugin.Clipboard;
using static Android.App.ActionBar;
using static Android.Views.View;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "letras_viewer_activity"
        , Theme = "@style/Theme.AppCompat.Light.NoActionBar"
        , NoHistory = false
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class letras_viewer_activity : AppCompatActivity, SeekBar.IOnSeekBarChangeListener
    {
        TextView LyricTextView;
        int IdLyric;
        Lyric mLetra = new Lyric();
        DataBase db;

        PopupWindow popupWindow;

        ImageButton play_ImageButton;
        ImageButton stop_ImageButton;
        ImageButton repeat_ImageButton;

        TextView titleCurrentAudio_textView;
        TextView currentPosion_textView;
        TextView maxTime_textView;

        SlidingUpPanelLayout sliding_layout;
        LinearLayout dragView;
        SeekBar seekBar;

        protected MediaPlayer player;

        bool IsPlaying;
        bool IsLooping;
        private int CurrentPosition;
        private int Duration;

        private Handler handler = new Handler();

        ListView AttachmentListView;
        List<Attachment> _attachments;

        int _selected_IdAttachment;
        string _selected_filePath;

        HideAndShowKeyboard hideAndShowKeyboard = new HideAndShowKeyboard();

        public letras_viewer_activity()
        {
            //Create Database
            db = new DataBase();
            db.CreateDatabase();

            _attachments = new List<Attachment>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                SetContentView(Resource.Layout.letras_viewer_layout);

                LyricTextView = FindViewById<TextView>(Resource.Id.LyricTextView);

                //Toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_viewer_toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true); //backbutton
                SupportActionBar.SetHomeButtonEnabled(true);

                SupportActionBar.Title = "NombreCancion";
                LyricTextView.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

                //Get IdLyric from putextra
                IdLyric = Convert.ToInt32(Intent.GetStringExtra("IdLetra"));

                
                //https://github.com/Cheesebaron/SlidingUpPanel/blob/master/component/GettingStarted.md


                //Buttons
                play_ImageButton = FindViewById<ImageButton>(Resource.Id.play_ImageButton);
                play_ImageButton.Click += OnPlay_button_Click;
                stop_ImageButton = FindViewById<ImageButton>(Resource.Id.stop_ImageButton);
                stop_ImageButton.Click += OnStop_button_Click;
                repeat_ImageButton = FindViewById<ImageButton>(Resource.Id.repeat_ImageButton);
                repeat_ImageButton.Click += OnRepeat_button_Click;
                repeat_ImageButton.SetColorFilter(Resources.GetColor(Resource.Color.material_grey_50), Android.Graphics.PorterDuff.Mode.SrcAtop);


                //TextViews
                currentPosion_textView = FindViewById<TextView>(Resource.Id.currentPosion_textView);
                maxTime_textView = FindViewById<TextView>(Resource.Id.maxTime_textView);
                titleCurrentAudio_textView = FindViewById<TextView>(Resource.Id.titleCurrentAudio_textView);


                //SlidingUpPanelLayout 
                sliding_layout = FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_layout);
                //sliding_layout.PanelHeight = 600;//heigh collapsed
                
                sliding_layout.Click += OnSliding_layout_Click;
                sliding_layout.PanelSlide += OnSliding_layout_PanelSlide;


                //dragView
                dragView = FindViewById<LinearLayout>(Resource.Id.dragView);
                dragView.Clickable = true;


                //Seekbar
                seekBar = FindViewById<SeekBar>(Resource.Id.seekBar);
                seekBar.Clickable = false;
                //seekBar.SetOnSeekBarChangeListener(this);


                //Listview
                AttachmentListView = FindViewById<ListView>(Resource.Id.AttachmentListView);
                RegisterForContextMenu(AttachmentListView);


                //Retrieve Lyric
                RetrieveLyric(IdLyric);


                //SaveInstance
                if (savedInstanceState != null)
                {
                    IsPlaying = savedInstanceState.GetBoolean(key: "IsPlaying");
                    IsLooping = savedInstanceState.GetBoolean(key: "IsLooping");
                    Duration = savedInstanceState.GetInt("Duration");
                    CurrentPosition = savedInstanceState.GetInt("CurrentPosition");

                    if (IsPlaying)
                    {
                        play_ImageButton.SetImageResource(Resource.Drawable.btn_Pause);
                    }

                    if (IsLooping)
                    {
                        repeat_ImageButton.SetColorFilter(Resources.GetColor(Resource.Color.colorPrimary), Android.Graphics.PorterDuff.Mode.SrcAtop);
                        player.Looping = true;
                    }
                    else
                    {
                        repeat_ImageButton.SetColorFilter(Color.Black, Android.Graphics.PorterDuff.Mode.SrcAtop);
                    }

                    if (CurrentPosition == 0)
                    {
                        seekBar.Max = Duration;
                        String maxTimeString = this.MillisecondsToString(Duration);
                        maxTime_textView.Text = maxTimeString;
                    }
                    else if (CurrentPosition == Duration)
                    {
                        //Resets the MediaPlayer to its uninitialized state
                        player.Reset();
                    }
                    else
                    {
                        String currentTimeString = this.MillisecondsToString(CurrentPosition);
                        currentPosion_textView.Text = currentTimeString;

                        String maxTimeString = this.MillisecondsToString(Duration);
                        maxTime_textView.Text = maxTimeString;

                    }
                    //CurrentPosition == 0
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error_OnCreate", ex.Message);
            }
        }

        private void OnSliding_layout_PanelSlide(object sender, SlidingUpPanelSlideEventArgs args)
        {
            try
            {
                if (sliding_layout.IsExpanded || sliding_layout.IsAnchored)
                {
                    //sliding_layout.SlidingEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnSliding_layout_PanelSlide", ex.Message);
            }
        }

        private void OnSliding_layout_Click(object sender, EventArgs e)
        {
            try
            {
                sliding_layout.CollapsePane();
            }
            catch (Exception ex)
            {
                Log.Error("OnSliding_layout_Click", ex.Message);
            }
        }

        #region MepiaPlayer

        private void OnRepeat_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (player != null)
                {
                    if (IsLooping)
                    {
                        repeat_ImageButton.SetColorFilter(Color.Black, Android.Graphics.PorterDuff.Mode.SrcAtop);
                        player.Looping = false;
                        IsLooping = false;

                        Toast toast = Toast.MakeText(this, GetString(Resource.String.MediaPlayer_noLoopingAudio), ToastLength.Short);
                        toast.SetGravity(GravityFlags.Center, 0, 0);
                        toast.Show();
                    }
                    else
                    {
                        repeat_ImageButton.SetColorFilter(Resources.GetColor(Resource.Color.colorPrimary), Android.Graphics.PorterDuff.Mode.SrcAtop);
                        player.Looping = true;
                        IsLooping = true;

                        Toast toast = Toast.MakeText(this, GetString(Resource.String.MediaPlayer_loopingAudio), ToastLength.Short);
                        toast.SetGravity(GravityFlags.Center, 0, 0);
                        toast.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnRepeat_button_Click", ex.Message);
            }
        }

        private void OnStop_button_Click(object sender, EventArgs e)
        {
            try
            {
                player.Stop();
                player.Reset();
                player.SetDataSource(_selected_filePath);
                player.Prepare();

                IsPlaying = false;

                play_ImageButton.SetImageResource(Resource.Drawable.btn_Play);
                repeat_ImageButton.SetColorFilter(Resources.GetColor(Resource.Color.material_grey_50), Android.Graphics.PorterDuff.Mode.SrcAtop);


                CurrentPosition = 0;
                seekBar.SetProgress(0, true);

                String currentPositionString = this.MillisecondsToString(0);
                currentPosion_textView.Text = currentPositionString;

            }
            catch (Exception ex)
            {
                Log.Error("OnStop_button_Click", ex.Message);
            }
        }

        private void OnPlay_button_Click(object sender, EventArgs e)
        {
            try
            {
                //test play media 
                if (player == null)
                {
                    Toast toast = Toast.MakeText(this, GetString(Resource.String.MediaPlayer_selectAudio), ToastLength.Short);
                    toast.SetGravity(GravityFlags.Center, 0, 0);
                    toast.Show();

                    sliding_layout.ExpandPane();
                }
                else
                {
                    if (IsPlaying)
                    {
                        player.Pause();
                        IsPlaying = false;
                        play_ImageButton.SetImageResource(Resource.Drawable.btn_Play);
                    }
                    else
                    {
                        

                        player.Start();
                        IsPlaying = true;
                        play_ImageButton.SetImageResource(Resource.Drawable.btn_Pause);
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error("OnPlay_button_Click", ex.Message);
            }
        }

        private void OnPlayer_Prepared(object sender, EventArgs e)
        {
            try
            {
                if (player != null)
                {
                    player.SeekTo((int)CurrentPosition);
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnPlayer_Prepared", ex.Message);
            }
        }

        private void OnPlayer_Completion(object sender, EventArgs e)
        {
            try
            {
                if (IsLooping)
                {
                    player.Start();
                }
                else
                {
                    repeat_ImageButton.SetColorFilter(Resources.GetColor(Resource.Color.material_grey_50), Android.Graphics.PorterDuff.Mode.SrcAtop);
                    IsPlaying = false;
                    play_ImageButton.SetImageResource(Resource.Drawable.btn_Play);
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnPlayer_Completion", ex.Message);
            }
        }

        // Convert millisecond to string.
        private String MillisecondsToString(int milliseconds)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(milliseconds);
            DateTime dt = new DateTime(ts.Ticks);
            String hms = dt.ToString(("mm:ss"));//HH:
            return hms;
        }


        #endregion


        private void RetrieveLyric(int idLetra)
        {
            try
            {
                mLetra = db.SelectQueryTableLyricsById(idLetra);
                SupportActionBar.Title = mLetra.Title;
                SupportActionBar.Subtitle = mLetra.Author;
                LyricTextView.Text = mLetra.lyric;

                RetrieveAttachments(idLetra);

                //Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }
            catch (Exception ex)
            {
                Log.Error("ErrorRetrieveLyric", ex.Message);
            }
        }

        private void RetrieveAttachments(int idLetra)
        {
            try
            {
                _attachments.Clear();

                _attachments = db.SelectTableAttachmentByIdLyric(idLetra).OrderBy(n => n.Name).ToList();

                // populate the listview with data
                AttachmentListView.Adapter = new attachment_listViewAdapter(this, _attachments);
                AttachmentListView.ItemClick += OnAttachmentListView_ItemClick;
                //AttachmentListView.ItemLongClick += OnAttachmentListView_ItemLongClick;

            }
            catch (Exception ex)
            {
                Log.Error("RetrieveAttachments", ex.Message);
            }
        }

        //private void OnAttachmentListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        //{
        //    try
        //    {
        //        //open contextual menu
        //        OpenContextMenu(AttachmentListView);

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("OnAttachmentListView_ItemLongClick", ex.Message);
        //    }
        //}

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.AttachmentListView)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
                menu.SetHeaderTitle(_attachments[info.Position].Name);
                var menuItems = Resources.GetStringArray(Resource.Array.menuAttachmentListView);
                for (var i = 0; i < menuItems.Length; i++)
                    menu.Add(Menu.None, i, i, menuItems[i]);
            }
            base.OnCreateContextMenu(menu, v, menuInfo);
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            try
            {
                var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
                //var menuItemIndex = item.ItemId;
                //var menuItems = Resources.GetStringArray(Resource.Array.menuAttachmentListView);
                //var menuItemName = menuItems[menuItemIndex];
                var listItemName = _attachments[info.Position];

                //Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName, listItemName), ToastLength.Short).Show();

                switch (item.ItemId)
                {
                    case 0:
                        //Rename
                        UpdateAttachmentName(listItemName);
                        break;
                    case 1:
                        //Delete
                        DeleteAttachment(listItemName);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnContextItemSelected", ex.Message);
            }
            
            return true;
            //return base.OnContextItemSelected(item);
        }

        [Obsolete]
        private void UpdateAttachmentName(Attachment attachment)
        {
            try
            {
                ////Show popup
                //LayoutInflater layoutInflater = (LayoutInflater)this.BaseContext.GetSystemService(Context.LayoutInflaterService);
                //View popupView = layoutInflater.Inflate(Resource.Layout.letras_attachment_popup_layout, null);
                //popupWindow = new PopupWindow(popupView, LayoutParams.MatchParent, LayoutParams.WrapContent);

                //popupWindow.SetBackgroundDrawable(new BitmapDrawable());

                //Button Ok_button = (Button)popupView.FindViewById(Resource.Id.Ok_button);
                //Ok_button.Click += delegate {
                //    popupWindow.Dismiss();
                //};

                //EditText Name_EditText = popupView.FindViewById<EditText>(Resource.Id.Name_EditText);
                //hideAndShowKeyboard.showSoftKeyboard(this, Name_EditText);


                //popupWindow.ShowAtLocation(AttachmentListView, GravityFlags.Center, 0, 0);

                //popupWindow.Focusable = true;

                Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);

                alert.SetTitle("Title");
                alert.SetMessage("Message");

                // Set an EditText view to get user input 
                EditText input = new EditText(this);
                input.Text = attachment.Name;
                alert.SetView(input);

                alert.SetPositiveButton(GetString(Resource.String.Ok), (sender, args) =>
                {

                })
                .SetNegativeButton(GetString(Resource.String.Cancel), (sender, args) =>
                {

                });


            }
            catch (Exception ex)
            {
                Log.Error("UpdateAttachmentName", ex.Message);
            }
        }

        private void DeleteAttachment(Attachment attachment)
        {
            try
            {
                //Confirm delete

                string Message = string.Empty;
                Message = GetString(Resource.String.message_Delete_attachmnet_intial) + " " + attachment.Name + "?" + GetString(Resource.String.message_Delete_attachmnet_final);

                Android.Support.V7.App.AlertDialog.Builder dialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                dialog.SetPositiveButton(GetString(Resource.String.message_Delete_attachmnet), (sender, args) =>
                {
                    //yes
                    if (db.DeleteTableAttachment(attachment))
                    {
                        RetrieveAttachments(IdLyric);
                    }

                    Toast.MakeText(this, GetString(Resource.String.message_Delete_attachmnet_success), ToastLength.Short).Show();
                })
                .SetNegativeButton(GetString(Resource.String.Cancel), (sender, args) =>
                {

                })
                .SetMessage(Message);
                //.SetTitle(GetString(Resource.String.message_Delete));

                dialog.Show();

                
            }
            catch (Exception ex)
            {
                Log.Error("DeleteAttachment", ex.Message);
            }
        }

        private void OnAttachmentListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                _selected_IdAttachment = (int)e.Id;
                Attachment attachment = db.SelectTableAttachmentByIdAttachment(_selected_IdAttachment);
                _selected_filePath = attachment.Path;
                titleCurrentAudio_textView.Text = attachment.Name;

                sliding_layout.CollapsePane();

                //play audio
                if (player == null)
                {
                    player = new MediaPlayer();
                    player.Completion += OnPlayer_Completion;
                    player.Prepared += OnPlayer_Prepared;
                    
                }//if (player != null)

                player.Reset();
                player.SetDataSource(_selected_filePath);
                player.Prepare();

                Duration = player.Duration;
                CurrentPosition = player.CurrentPosition;

                if (CurrentPosition == 0)
                {
                    seekBar.Max = Duration;
                    String maxTimeString = this.MillisecondsToString(Duration);
                    maxTime_textView.Text = maxTimeString;
                }
                else if (CurrentPosition == Duration)
                {
                    //Resets the MediaPlayer to its uninitialized state
                    player.Reset();
                }//CurrentPosition == 0

                player.Start();

                // Create a thread to update position of SeekBar.
                //UpdateSeekBarThread updateSeekBarThread = new UpdateSeekBarThread();
                //handler.PostDelayed(updateSeekBarThread, 50);

                IsPlaying = true;

                play_ImageButton.SetImageResource(Resource.Drawable.btn_Pause);
                repeat_ImageButton.SetColorFilter(Color.Black, Android.Graphics.PorterDuff.Mode.SrcAtop);

            }
            catch (Exception ex)
            {
                Log.Error("OnAttachmentListView_ItemClick", ex.Message);
            }
        }

        #region Overrides

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change main_compat_menu
            MenuInflater.Inflate(Resource.Menu.letras_viewer_toolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_attach:
                    //Toast.MakeText(this,"Muy pronto ;)", ToastLength.Short).Show();
                    Intent intentAttached = new Intent(this, typeof(letras_attach_activity));
                    intentAttached.PutExtra("IdLyric", IdLyric.ToString());
                    StartActivity(intentAttached);
                    break;
                case Resource.Id.action_edit:
                    Intent intentEdit = new Intent(this, typeof(letras_edit_activity));
                    intentEdit.PutExtra("IdLyric", IdLyric.ToString());
                    StartActivity(intentEdit);
                    break;
                case Resource.Id.menu_copy:
                    /*

                        To set text
                        CrossClipboard.Current.SetText("my clipboard text");
                        CrossClipboard.Current.SetText( WebUtility.UrlDecode(message));

                        To get text
                        string clipboardText = await CrossClipboard.Current.GetTextAsync();                   

                    */
                    CrossClipboard.Current.SetText(mLetra.ToString());
                    Toast toast = Toast.MakeText(this, "Letra copiada", ToastLength.Short);
                    toast.SetGravity(GravityFlags.Center, 0, 0);
                    toast.Show();
                    break;
                case Resource.Id.menu_share:

                    ShowShareOptions();

                    break;
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }//OnOptionsItemSelected

        public override void OnBackPressed()
        {
            if (sliding_layout != null &&
                (sliding_layout.IsExpanded|| sliding_layout.IsAnchored))
            {
                sliding_layout.CollapsePane();
            }
            else
            {
                if (player != null)
                {
                    player.Stop();
                    player.Release();
                    player.Dispose();
                    player = null;
                }
                base.OnBackPressed();
            }
            
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            try
            {
                outState.PutBoolean("IsPlaying", IsPlaying);
                outState.PutBoolean("IsLooping", IsLooping);
                outState.PutInt("Duration", Duration);
                outState.PutInt("CurrentPosition", CurrentPosition);

            }
            catch (Exception ex)
            {
                Log.Error("OnSaveInstanceState", ex.Message);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (player != null)
            {
                Duration = player.Duration;
                CurrentPosition = player.CurrentPosition;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //if (player != null)
            //{
            //    player.Stop();
            //    player.Release();
            //    player = null;
            //}
        }

        protected override void OnResume()
        {
            RetrieveLyric(IdLyric);
            base.OnResume();
        }

        protected override void OnRestart()
        {
            RetrieveLyric(IdLyric);
            base.OnRestart();
        }

        //public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        //{
        //    try
        //    {
        //        //View view = Inflate(Resource.Layout.letras_viewer_layout_slidepanel, Resource.Id.AttachmentListView, false);


        //        ////ListView
        //        //AttachmentListView = view.FindViewById<ListView>(Resource.Id.AttachmentListView);
        //        //AttachmentListView.ItemClick += AttachmentListView_ItemClick;
        //        ////_LetrasListView.ItemLongClick += LetrasListView_ItemLongClick;

        //        ////Methods
        //        //LoadAttachments();

        //        //return view;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("OnCreateView", ex.Message);
        //        return null;
        //    }
        //    //return base.OnCreateView(parent, name, context, attrs);
        //}


        #endregion


        private void ShowShareOptions()
        {
            try
            {
                Intent intent = new Intent(this, typeof(qr_code_generator_activity));
                intent.PutExtra("lyricText", mLetra.ToString());
                StartActivity(intent);


                //Intent intentsend = new Intent();
                //intentsend.SetAction(Intent.ActionSend);
                //intentsend.PutExtra(Intent.ExtraText, mLetra.ToString());
                //intentsend.SetType("text/plain");
                //StartActivity(intentsend);

            }
            catch (Exception ex)
            {
                Log.Error("ShowShareOptions", ex.Message);
            }
           
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (fromUser)
            {
                Toast.MakeText(this,"SeekBar value to " + seekBar.Progress.ToString(),ToastLength.Short).Show();
                player.SeekTo(seekBar.Progress);
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            Log.Info("OnStartTrackingTouch", "1");
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            Log.Info("OnStopTrackingTouch", "1");
        }
    }

    

}