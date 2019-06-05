using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Muusika.Resources;
using Muusika.Resources.Activities;
using Muusika.Resources.Adapters;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;
using NotiXamarin.Adapters;
using Plugin.Clipboard;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika
{
    public class letras_Fragment : Android.Support.V4.App.Fragment, ISelectedChecker
    {
        ListView _LetrasListView;
        List<Letra> _Letras;
        List<Letra> _LetrasSeleccionadas;

        letras_listViewAdapter _LetrasAdapter;

        DataBase db;
        bool SelectingMultipleItems = false;

        SupportToolbar _Toolbar;

        public bool IsSearching;

        public bool IsSelectingMultipleItms
        {
            get
            {
                return SelectingMultipleItems;
            }
        }

        public letras_Fragment()
        {
            //Create Database
            db = new DataBase();
            db.CreateDatabase();

            //Listas
            _Letras = new List<Letra>();
            _LetrasSeleccionadas = new List<Letra>();

        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                _LetrasSeleccionadas.Clear();
                //SetHasOptionsMenu(true);

                _Toolbar = this.Activity.FindViewById<SupportToolbar>(Resource.Id.letras_main_toolbar);

            }
            catch (Exception ex)
            {
                Log.Error("OnCreate", ex.Message);
            }
        }

        private async void OnFab_Import_Click(object sender, EventArgs e)
        {
            try
            {
                string clipboardText = await CrossClipboard.Current.GetTextAsync();


                if (clipboardText.Contains("--- Muusika ---"))
                {
                    string[] tokens = clipboardText.Split("--- Muusika ---");

                    foreach (var token in tokens)
                    {
                        AddLirycFromClipboard(token);
                    }
                }
                else
                {
                    AddLirycFromClipboard(clipboardText);
                }
                
                
            }
            catch (Exception ex)
            {
                Log.Error("OnFab_Import_Click", ex.Message);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {

                View view = inflater.Inflate(Resource.Layout.letras_layout, container, false);


                //ListView
                _LetrasListView = view.FindViewById<ListView>(Resource.Id.LetrasListView);
                _LetrasListView.ItemClick += LetrasListView_ItemClick;
                _LetrasListView.ItemLongClick += LetrasListView_ItemLongClick;

                //Methods
                LoadData();


                //FloatingActionButton
                FloatingActionButton fab_Import = view.FindViewById<FloatingActionButton>(Resource.Id.fab_Import);
                fab_Import.Click += OnFab_Import_Click;

                return view;
            }
            catch (Exception ex)
            {
                Log.Error("OnCreateView", ex.Message);
                return null;
            }

        }

        private void LetrasListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            try
            {
                ////set backgroud for selected item
                //for (int i = 0; i < _LetrasListView.Count; i++)
                //{
                //    if (e.Position == i)
                //        _LetrasListView.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.BlueViolet);
                //    else
                //        _LetrasListView.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
                //}

                if (SelectingMultipleItems)
                {
                    int id = (int)e.Id;
                    View view = e.View;

                    SelectUnselectElements(id, view, e.Position);
                }
                else
                {
                    Intent intent;

                    intent = new Intent(this.Activity, typeof(letras_viewer_activity));
                    intent.PutExtra("IdLetra", _LetrasListView.Adapter.GetItemId(e.Position).ToString());
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error_LetrasListView_ItemClick", ex.Message);
            }
        }

        private void LetrasListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            try
            {
                int id = (int)e.Id;
                View view = e.View;

                //consult if is searching
                if (!IsSearching)
                {
                    SelectUnselectElements(id, view, e.Position);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error_LetrasListView_ItemLongClick", ex.Message);
            }
        }

        private void SelectUnselectElements(int id, View view, int Position)
        {
            try
            {
                LinearLayout linearLayout = view.FindViewById<LinearLayout>(Resource.Id.LetrasListItem_LinearLayout);

                if (_LetrasSeleccionadas.Select(x => x.IdLetra).Contains(id))
                {
                    //deselect element
                    var colorForUnselected = Android.Graphics.Color.Transparent;
                    linearLayout.SetBackgroundColor(colorForUnselected);
                    _LetrasSeleccionadas.Remove(_LetrasAdapter[Position]);
                }
                else
                {
                    //select element
                    var colorForSelected = Android.Graphics.Color.ParseColor(Resources.GetString(Resource.Color.colorListItemSelected));
                    linearLayout.SetBackgroundColor(colorForSelected);
                    _LetrasSeleccionadas.Add(_LetrasAdapter[Position]);
                }

                if (_LetrasSeleccionadas.Count > 0)
                {
                    SelectingMultipleItems = true;
                    _Toolbar.Title = _LetrasSeleccionadas.Count.ToString();
                    //this.Activity.SupportActionBar.Title = "Muusika";

                }
                else
                {
                    SelectingMultipleItems = false;
                }


                //Forces Android to execute OnCreateOptionsMenu
                this.Activity.InvalidateOptionsMenu();
            }
            catch (Exception ex)
            {
                Log.Error("Error_SelectUnselectElements", ex.Message);
            }
        }

        public void UnselectElements()
        {
            try
            {
                int count = _LetrasListView.ChildCount;

                for (int i = 0; i < count; i++)
                {
                    View row = _LetrasListView.GetChildAt(i);
                    var rl = row.FindViewById<LinearLayout>(Resource.Id.LetrasListItem_LinearLayout);
                    var color = Android.Graphics.Color.Transparent;
                    rl.SetBackgroundColor(color);
                }

                SelectingMultipleItems = false;
                _LetrasSeleccionadas.Clear();

                //Forces Android to execute OnCreateOptionsMenu
                this.Activity.InvalidateOptionsMenu();
            }
            catch (Exception ex)
            {
                Log.Error("UnselectElements", ex.Message);
            }
        }

        public bool IsItemSelected(int id)
        {
            return _LetrasSeleccionadas.Select(x => x.IdLetra).Contains(id);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_delete:
                    Toast.MakeText(this.Activity, "Eliminar!", ToastLength.Short).Show();
                    return true;
                case Resource.Id.home:
                    Toast.MakeText(this.Activity, "HOME!", ToastLength.Short).Show();
                    return true;
            }

            return false;
        }

        public void LoadData()
        {
            try
            {
                if (!SelectingMultipleItems)
                {
                    _Letras = db.SelectTableLetras().OrderBy(n => n.Titulo).ToList();
                    _LetrasAdapter = new letras_listViewAdapter(this, _Letras);
                    _LetrasListView.Adapter = _LetrasAdapter;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error_LoadData", ex.Message);
            }
        }//LoadData

        public void AddLiryc(string title, string author, string album, string liryc)
        {
            try
            {
                Letra mLetra = new Letra()
                {
                    Autor = author,
                    Album = album,
                    Titulo = title,
                    letra = liryc
                };
                db.InsertIntoTableLetras(mLetra);
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error("Error AddLiryc", ex.Message);
            }
        }//AddLiryc

        public void AddLirycFromClipboard(string LirycTextFromClipboard)
        {
            try
            {
                string[] tokens = LirycTextFromClipboard.Split('*');

                if (tokens.Count() >= 9 && tokens[0] == "[Muusika - Storage and Share]\n\n")
                {
                    string title = tokens[1];
                    string author = tokens[3];
                    string album = tokens[5];
                    string liryc = tokens[7];

                    Letra mLetra = new Letra()
                    {
                        Autor = author,
                        Album = album,
                        Titulo = title,
                        letra = liryc
                    };
                    db.InsertIntoTableLetras(mLetra);
                    LoadData();

                    Toast.MakeText(this.Activity, "Letra añadida", ToastLength.Short);
                }
                else
                {
                    string clipboardShorted;

                    if (LirycTextFromClipboard.Length > 10)
                    {
                        clipboardShorted = LirycTextFromClipboard.Substring(0, 10);
                    }
                    else
                    {
                        clipboardShorted = LirycTextFromClipboard;
                    }

                    Toast toast = Toast.MakeText(this.Activity, "El texto '" + clipboardShorted + "...' no proviene de Muusika ;)", ToastLength.Long);
                    toast.SetGravity(GravityFlags.Center, 0, 0);
                    toast.Show();
                }
            }
            catch (Exception ex)
            {
                Log.Error("AddLirycFromClipboard", ex.Message);
            }
        }

        public bool EditLiryc(string title, string author, string album, string liryc, int IdLiryc)
        {
            try
            {
                Letra mLiryc = new Letra()
                {
                    Titulo = title,
                    Autor = author,
                    Album = album,
                    letra = liryc,
                    IdLetra = IdLiryc
                };
                db.UpdateTableLetras(mLiryc);
                LoadData();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error EditLiryc", ex.Message);
                return false;
            }

        }

        public bool DeleteLirycs()
        {
            try
            {
                foreach (Letra letra in _LetrasSeleccionadas)
                {
                    db.DeleteTableLetras(letra);
                }

                _LetrasSeleccionadas.Clear();
                SelectingMultipleItems = false;
                //Forces Android to execute OnCreateOptionsMenu
                this.Activity.InvalidateOptionsMenu();

                LoadData();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error letras_Fragment", ex.Message);
                return false;
            }
        }//DeleteLirycs

        public void CopyLirycs()
        {
            try
            {
                string clipBoard = string.Empty;

                foreach (Letra letra in _LetrasSeleccionadas)
                {
                    clipBoard += letra.ToString();
                    clipBoard += "--- Muusika ---";
                }

                CrossClipboard.Current.SetText(clipBoard);
                Toast toast = Toast.MakeText(this.Activity, "Letra(s) copiada(s)", ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();

                _LetrasSeleccionadas.Clear();
                SelectingMultipleItems = false;
                //Forces Android to execute OnCreateOptionsMenu
                this.Activity.InvalidateOptionsMenu();

                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error("CopyLirycs", ex.Message);
            }
        }

        public void ShareLirycs()
        {
            try
            {
                string LirycText = string.Empty;

                foreach (Letra letra in _LetrasSeleccionadas)
                {
                    LirycText += letra.ToString();
                    LirycText += "--- Muusika ---";
                }

                Intent intentsend = new Intent();
                intentsend.SetAction(Intent.ActionSend);
                intentsend.PutExtra(Intent.ExtraText, LirycText);
                intentsend.SetType("text/plain");
                StartActivity(intentsend);

                _LetrasSeleccionadas.Clear();
                SelectingMultipleItems = false;
                //Forces Android to execute OnCreateOptionsMenu
                this.Activity.InvalidateOptionsMenu();

                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error("ShareLirycs", ex.Message);
            }
        }
        public void FilterLirycs(string filterQuery)
        {
            try
            {
                if (filterQuery != "" )
                {
                    _Letras = db.FilterTableLetras(filterQuery);
                    _LetrasAdapter = new letras_listViewAdapter(this, _Letras);
                    _LetrasListView.Adapter = _LetrasAdapter;

                    if (_Letras.Count == 0)
                    {
                        Toast toast = Toast.MakeText(this.Activity, "No result found for '" + filterQuery + "'", ToastLength.Short);
                        toast.SetGravity(GravityFlags.Center, 0, 0);
                        toast.Show();
                    }
                }
                else
                {
                    LoadData();
                }
                
            }
            catch (Exception ex)
            {
                Log.Error("Error FilterLirycs", ex.Message);
            }
        }//FilterLirycs

        [Obsolete]
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            if (_LetrasSeleccionadas.Count > 0)
            {
                //Include delete, share, favorite, etc. option
                inflater.Inflate(Resource.Menu.letras_listview_toolbar, menu);
                ((MainActivity)this.Activity).ResetActionBar(true);
            }
            else
            {
                //change main_compat_menu
                //Include search option
                inflater.Inflate(Resource.Menu.letras_main_toolbar, menu);

                _Toolbar.Title = GetString(Resource.String.app_name);
                ((MainActivity)this.Activity).ResetActionBar(false);


                //Search action
                var mSearchItem = menu.FindItem(Resource.Id.action_search);

                var searchView = MenuItemCompat.GetActionView(mSearchItem);
                var _searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();

                _searchView.QueryTextChange += (s, e) => FilterLirycs(e.NewText);

                _searchView.QueryTextSubmit += (s, e) =>
                {
                    // Handle enter/search button on keyboard here
                    Toast.MakeText(this.Activity, "Searched for: " + e.Query, ToastLength.Short).Show();
                    e.Handled = true;
                };

                MenuItemCompat.SetOnActionExpandListener(mSearchItem, new SearchViewExpandListener(this.Activity, _Toolbar));

            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        private class SearchViewExpandListener : Java.Lang.Object, MenuItemCompat.IOnActionExpandListener
        {
            Context context;
            SupportToolbar _Toolbar;

            public SearchViewExpandListener(Context pContext, SupportToolbar pToolbar)
            {
                context = pContext;
                _Toolbar = pToolbar;
            }
            public bool OnMenuItemActionCollapse(IMenuItem item)
            {

                //IsSearching = false;
                //MyStuff with context

                // Called when SearchView is collapsing

                //https://stackoverflow.com/questions/30369246/implementing-searchview-as-per-the-material-design-guidelines
                //https://stackoverflow.com/questions/45930524/searchview-to-your-actionbar-for-recyclerview-in-xamarin-android
                //https://stackoverflow.com/questions/38761866/xamarin-searchview-onactionexpandlistener

                if (item.IsActionViewExpanded)
                {
                    AnimateSearchToolbar(1, false, false);
                }
                return true;
            }

            public bool OnMenuItemActionExpand(IMenuItem item)
            {
                try
                {
                    //IsSearching = true;

                    // Called when SearchView is expanding
                    AnimateSearchToolbar(1, true, true);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error("OnMenuItemActionExpand", ex.Message);
                    return false;
                }
            }

            public void AnimateSearchToolbar(int numberOfMenuIcon, bool containsOverflow, bool show)
            {
                try
                {
                    _Toolbar.SetBackgroundColor(Android.Graphics.Color.White);
                    //mDrawerLayout.setStatusBarBackgroundColor(ContextCompat.getColor(this, R.color.quantum_grey_600));

                    if (show)
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                        {
                            int width = _Toolbar.Width -
                                    (containsOverflow ? (int)context.Resources.GetDimensionPixelSize(Resource.Dimension.abc_action_button_min_width_overflow_material) : 0) -
                                    (((int)context.Resources.GetDimensionPixelSize(Resource.Dimension.abc_action_button_min_width_material) * numberOfMenuIcon) / 2);

                            Animator createCircularReveal = ViewAnimationUtils.CreateCircularReveal(_Toolbar,
                                    IsRtl(context.Resources) ? _Toolbar.Width - width : width, _Toolbar.Height / 2, 0.0f, (float)width);
                            createCircularReveal.SetDuration(250);
                            createCircularReveal.Start();
                        }
                        else
                        {
                            TranslateAnimation translateAnimation = new TranslateAnimation(0.0f, 0.0f, (float)(-_Toolbar.Height), 0.0f);
                            translateAnimation.Duration = 220;
                            _Toolbar.ClearAnimation();
                            _Toolbar.StartAnimation(translateAnimation);
                        }
                    }// if (show)
                    else
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                        {
                            int width = _Toolbar.Width -
                                (containsOverflow ? (int)context.Resources.GetDimensionPixelSize(Resource.Dimension.abc_action_button_min_width_overflow_material) : 0) -
                                (((int)context.Resources.GetDimensionPixelSize(Resource.Dimension.abc_action_button_min_width_material) * numberOfMenuIcon) / 2);

                            Animator createCircularReveal = ViewAnimationUtils.CreateCircularReveal(_Toolbar,
                                    IsRtl(context.Resources) ? _Toolbar.Width - width : width, _Toolbar.Height / 2, (float)width, 0.0f);
                            createCircularReveal.SetDuration(250);
                            createCircularReveal.AddListener(new AnimatorListener(_Toolbar));
                            createCircularReveal.Start();
                        }
                        else
                        {
                            AlphaAnimation alphaAnimation = new AlphaAnimation(1.0f, 0.0f);
                            Animation translateAnimation = new TranslateAnimation(0.0f, 0.0f, 0.0f, (float)(-_Toolbar.Height));
                            AnimationSet animationSet = new AnimationSet(true);
                            animationSet.AddAnimation(alphaAnimation);
                            animationSet.AddAnimation(translateAnimation);
                            animationSet.Duration = 220;
                            animationSet.SetAnimationListener(new AnimationListener(_Toolbar));
                            _Toolbar.StartAnimation(animationSet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("AnimateSearchToolbar", ex.Message);
                }
            }//AnimateSearchToolbar
            
            private bool IsRtl(Android.Content.Res.Resources resources)
            {
                return resources.Configuration.LayoutDirection == Android.Views.LayoutDirection.Rtl;
            }//isRtl


            /// <summary>
            /// https://forums.xamarin.com/discussion/98853/how-to-write-the-c-equivalent-code-of-this-java-code
            /// </summary>
            class AnimationListener : Java.Lang.Object, Animation.IAnimationListener
            {
                View View;

                public AnimationListener(View view)
                {
                    this.View = view;
                }
                public void OnAnimationEnd(Animation animation)
                {
                    //base.OnAnimationEnd(animation);
                    View.SetBackgroundResource(Resource.Color.colorPrimaryToolbar);
                    //mDrawerLayout.setStatusBarBackgroundColor(getThemeColor(MainActivity.this, R.attr.colorPrimaryDark));

                }
                // Other interface functions
                public void OnAnimationCancel(Animation animation) { }
                public void OnAnimationRepeat(Animation animation) { }
                public void OnAnimationStart(Animation animation) { }
            }

            class AnimatorListener : Java.Lang.Object, Animator.IAnimatorListener
            {
                View View;
                public AnimatorListener(View view)
                {
                    this.View = view;
                }
                public void OnAnimationEnd(Animator animation)
                {
                    //base.OnAnimationEnd(animation);
                    View.SetBackgroundResource(Resource.Color.colorPrimaryToolbar);
                    //mDrawerLayout.setStatusBarBackgroundColor(getThemeColor(MainActivity.this, R.attr.colorPrimaryDark));

                }
                // Other interface functions
                public void OnAnimationCancel(Animator animation) { }
                public void OnAnimationRepeat(Animator animation) { }
                public void OnAnimationStart(Animator animation) { }
            }

        }//private class SearchViewExpandListener

    }//public class letras

}//Namespace Muusika