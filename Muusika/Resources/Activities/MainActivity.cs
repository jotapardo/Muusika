using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
//using GR.Net.Maroulis.Library;
using Muusika.Resources.Activities;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;
using Newtonsoft.Json;

using SupportFragment = Android.Support.V4.App.Fragment;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]//, MainLauncher = true
    public class MainActivity: AppCompatActivity //, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        //TextView textMessage;
        private SupportFragment mCurrentFragment;
        private letras_Fragment mLetras_Fragment;
        private Fragment2_fragment mFragment2_fragment;
        private Fragment3_fragment mFragment3_fragment;

        FloatingActionButton fab;

        //Google
        //GoogleSignInClient mGoogleSignInClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                SetContentView(Resource.Layout.activity_main);

                ////Test websearch
                //StartActivity(new Intent(this, typeof(letras_search_activity)));
                //Finish();


                //SplashScreen
                //Nutget with error EasySplashScreen
                //var config = new EasySplashScreen(this)
                //    .WithFullScreen()
                //    .WithTargetActivity(Java.Lang.Class.FromType(typeof(splash_screen_activity)))
                //    .WithSplashTimeOut(1000) //1 sec
                //    .WithBackgroundColor(Color.White)
                //    .WithLogo(Resource.Drawable.Muusika_icon);
                ////create view
                //View view = config.Create();
                ////Set Content View
                //SetContentView(view);


                //SatusBar
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    Window.SetStatusBarColor(Color.ParseColor(GetString(Resource.Color.colorPrimaryDark)));
                }

                //Intro
                RunOnUiThread(() => {
                    ISharedPreferences getPresfs = PreferenceManager.GetDefaultSharedPreferences(BaseContext);
                    bool IsFirstStart = getPresfs.GetBoolean("firstStart", true);
                    if (IsFirstStart)
                    {
                        StartActivity(new Intent(this, typeof(intro_activity)));
                        Finish();
                    ISharedPreferencesEditor sharedPreferencesEditor = getPresfs.Edit();
                    sharedPreferencesEditor.PutBoolean("firstStart", false);
                    sharedPreferencesEditor.Apply();
                }
                });


                //Navigation
                //BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
                //navigation.SetOnNavigationItemSelectedListener(this);


                //Fragmentos
                mLetras_Fragment = new letras_Fragment();
                mFragment2_fragment = new Fragment2_fragment();
                mFragment3_fragment = new Fragment3_fragment();

                var trans = SupportFragmentManager.BeginTransaction();

                trans.Add(Resource.Id.content_frame, mLetras_Fragment, "Letras_Fragment");

                trans.Add(Resource.Id.content_frame, mFragment2_fragment, "Fragment2_fragment");
                trans.Hide(mFragment2_fragment);

                trans.Add(Resource.Id.content_frame, mFragment3_fragment, "Fragment3_fragment");
                trans.Hide(mFragment3_fragment);

                /// ... add more fragments
                /// 
                trans.Commit();

                mCurrentFragment = mLetras_Fragment;


                //FloatActionButton
                fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
                fab.Click += FabOnClick;

                //Toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_main_toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = GetString(Resource.String.app_name);

                ConectGoogleAccount();
            }
            catch (Exception ex)
            {
                Log.Error("OnCreate", ex.Message);
            }
        }//oncreate

        private  void FabOnClick(object sender, System.EventArgs e)
        {

            //mLetras_Fragment.AddLyric();
            //View view = (View)sender;
            //Snackbar.Make(view, "Test", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();


            //StartActivity(typeof(letras_nueva_activity(mLetras_Fragment)));
            //https://www.youtube.com/watch?v=sj-eXmpRjtI

            Intent intent = new Intent(this, typeof(letras_nueva_activity));
            intent.PutExtra("Letras_Fragment", JsonConvert.SerializeObject(mLetras_Fragment));
            StartActivity(intent);

            fab.Enabled = false;


        }//FabOnClick

        protected override void OnResume()
        {
            try
            {
                //Refresh fragment
                ShowFragment(mCurrentFragment);

                if (mCurrentFragment == mLetras_Fragment)
                {
                    mLetras_Fragment.LoadData();
                    fab.Enabled = true;
                }



            }
            catch (Exception ex)
            {
                Log.Error("OnResume", ex.Message);
            }
            base.OnResume();
        }//OnResume

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }//OnRequestPermissionsResult

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    ShowFragment(mLetras_Fragment);
                    return true;
                case Resource.Id.navigation_dashboard:
                    ShowFragment(mFragment2_fragment);
                    return true;
                case Resource.Id.navigation_notifications:
                    ShowFragment(mFragment3_fragment);
                    return true;
            }

            return false;
        }//OnNavigationItemSelected

        private void ShowFragment(SupportFragment fragment)
        {
            if (fragment != null)
            {

                var fragment_transaction = SupportFragmentManager.BeginTransaction();
                fragment_transaction.Hide(mCurrentFragment);
                fragment_transaction.Show(fragment);
                fragment_transaction.AddToBackStack(null);
                fragment_transaction.Commit();

                mCurrentFragment = fragment;
            }

        }//ShowFragment

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            try
            {
                if (mCurrentFragment == mLetras_Fragment)
                {
                    //Call internal method of mLetras_Fragment
                    mLetras_Fragment.OnCreateOptionsMenu(menu, MenuInflater);
                }
                return base.OnCreateOptionsMenu(menu);
            }
            catch (Exception ex)
            {
                Log.Error("OnCreateOptionsMenu", ex.Message);
                return false;
            }

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_delete:
                    mLetras_Fragment.DeleteLyrics();
                    break;
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    break;
                //case Resource.Id.action_backup:
                //    Intent intent = new Intent(this, typeof(backup_activity));
                //    StartActivity(intent);
                //    break;
                case Resource.Id.action_addFavorite:
                    break;
                case Resource.Id.action_removeFavorite:
                    break;
                case Resource.Id.action_copy:
                    mLetras_Fragment.CopyLyrics();
                    break;
                case Resource.Id.action_share:
                    mLetras_Fragment.ShareLyrics();
                    break;
                case Resource.Id.action_intro:
                    //Intro
                    RunOnUiThread(() => {
                        StartActivity(new Intent(this, typeof(intro_activity)));
                        Finish();
                    });
                    break;
                case Resource.Id.action_settings:
                    Toast.MakeText(this, "Muy pronto ;)", ToastLength.Short).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            //Do not call the base method
            //base.Onbackpressed
            //Do something else
            // *something else code*
            //finish this activity unless you want to keep it for some reason
            //funcionamiento como whatsapp
            try
            {
                if (mCurrentFragment != mLetras_Fragment)
                {
                    ShowFragment(mLetras_Fragment);
                }
                else
                {
                    //mLetras_Fragment is the current
                    if (mLetras_Fragment.IsSelectingMultipleItms)
                    {
                        mLetras_Fragment.UnselectElements();
                    }
                    else
                    {
                        Finish();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnBackPressed", ex.Message);
            }
        }//OnBackPressed

        //Toolbar show up arrow
        public void ResetActionBar(bool showHomeButton)
        {
            if (showHomeButton)
            {
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);//backbutton
                SupportActionBar.SetHomeButtonEnabled(true);
            }
            else
            {
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);//backbutton
                SupportActionBar.SetHomeButtonEnabled(false);
            }

        }


        #region Conect with google
        
        //https://causerexception.com/2017/12/03/google-native-login-with-xamarin-forms/
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {

                //GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                //GoogleManager.Instance.OnAuthCompleted(result);
            }
        }//OnActivityResult

        private void ConectGoogleAccount()
        {
            try
            {
                //https://developers.google.com/identity/sign-in/android/sign-in

                // Configure sign-in to request the user's ID, email address, and basic
                // profile. ID and basic profile are included in DEFAULT_SIGN_IN.
                GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                    .RequestEmail()
                    .Build();
                //var mGoogleApiClient = new GoogleApiClient.Builder(this)
                //    .EnableAutoManage(mLoginFragment, failedHandler)
                //    .AddApi(Auth.GOOGLE_SIGN_IN_API)
                //    .Build();

                //If you need to request additional scopes to access Google APIs, specify them with requestScopes. 
                //For the best user experience, on sign-in, only request the scopes that are required for your app 
                //to minimally function. Request any additional scopes only when you need them, 
                //so that your users see the consent screen in the context of an action they performed. 
                //See Requesting Additional Scopes.

                // Build a GoogleSignInClient with the options specified by gso.
                //mGoogleSignInClient = GoogleSignIn.getClient(this, gso);
            }
            catch (Exception ex)
            {
                Log.Error("ConectGoogleAccount", ex.Message);
            }
        }//ConectGoogleAccount


        protected override void OnStart()
        {
            try
            {
                // Check for existing Google Sign In account, if the user is already signed in
                // the GoogleSignInAccount will be non-null.
                //GoogleSignInAccount account = GoogleSignIn.getLastSignedInAccount(this);
                //updateUI(account);

            }
            catch (Exception)
            {

                throw;
            }
            base.OnStart();
        }
        #endregion



    }
}

