using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cheesebaron.SlidingUpPanel;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;
using Plugin.Clipboard;
using static Android.App.ActionBar;
using static Android.Views.View;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "letras_viewer_activity", Theme = "@style/Theme.AppCompat.Light.NoActionBar", NoHistory = false)]
    public class letras_viewer_activity : AppCompatActivity
    {
        TextView LyricTextView;
        int IdLyric;
        Letra mLetra = new Letra();
        DataBase db;

        PopupWindow popupWindow;

        ImageButton play_ImageButton;
        ImageButton stop_ImageButton;
        ImageButton repeat_ImageButton;

        SlidingUpPanelLayout sliding_layout;
        LinearLayout dragView;
        protected MediaPlayer player;

        bool IsPlaying;
        bool IsLooping;

        public letras_viewer_activity()
        {
            //Create Database
            db = new DataBase();
            db.CreateDatabase();
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

                //Retrieve Lyric
                RetrieveLyric(IdLyric);


                //https://github.com/Cheesebaron/SlidingUpPanel/blob/master/component/GettingStarted.md


                //Buttons
                play_ImageButton = FindViewById<ImageButton>(Resource.Id.play_ImageButton);
                play_ImageButton.Click += OnPlay_button_Click;
                stop_ImageButton = FindViewById<ImageButton>(Resource.Id.stop_ImageButton);
                stop_ImageButton.Click += OnStop_button_Click;
                repeat_ImageButton = FindViewById<ImageButton>(Resource.Id.repeat_ImageButton);
                repeat_ImageButton.Click += OnRepeat_button_Click;
                repeat_ImageButton.SetColorFilter(Resources.GetColor(Resource.Color.material_grey_50), Android.Graphics.PorterDuff.Mode.SrcAtop);

                //SlidingUpPanelLayout 
                sliding_layout = FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_layout);
                //sliding_layout.PanelHeight = 600;//heigh collapsed
                
                sliding_layout.Click += OnSliding_layout_Click;
                sliding_layout.PanelSlide += OnSliding_layout_PanelSlide;

                //dragView
                dragView = FindViewById<LinearLayout>(Resource.Id.dragView);
                dragView.Clickable = true;

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
                    }
                    else
                    {
                        repeat_ImageButton.SetColorFilter(Resources.GetColor(Resource.Color.colorPrimary), Android.Graphics.PorterDuff.Mode.SrcAtop);
                        player.Looping = true;
                        IsLooping = true;
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
                player = null;
                IsPlaying = false;
                play_ImageButton.SetImageResource(Resource.Drawable.btn_Play);
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
                //
                string filePath = db.SelectTableAttachedByIdLyric(1)[0].Path;

                //test play media 
                if (player == null)
                {
                    player = new MediaPlayer();
                    player.Completion += OnPlayer_Completion;
                    player.Reset();
                    player.SetDataSource(filePath);
                    player.Prepare();
                    player.Start();
                    IsPlaying = true;

                    play_ImageButton.SetImageResource(Resource.Drawable.btn_Pause);
                    repeat_ImageButton.SetColorFilter(Color.Black, Android.Graphics.PorterDuff.Mode.SrcAtop);
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

        private void OnPlayer_Completion(object sender, EventArgs e)
        {
            try
            {
                if (IsLooping)
                {
                    player.Start();
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnPlayer_Completion", ex.Message);
            }
        }
        #endregion


        private void RetrieveLyric(int idLetra)
        {
            try
            {
                mLetra = db.SelectQueryTableLetrasById(idLetra);
                SupportActionBar.Title = mLetra.Titulo;
                SupportActionBar.Subtitle = mLetra.Autor;
                LyricTextView.Text = mLetra.letra;

                //Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }
            catch (Exception ex)
            {
                Log.Error("ErrorRetrieveLyric", ex.Message);
            }
        }


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
                    player.Dispose();
                    player = null;
                }
                base.OnBackPressed();
            }
            
        }

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


    }

    

}