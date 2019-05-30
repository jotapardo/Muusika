using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace Muusika
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        //TextView textMessage;
        private SupportFragment mCurrentFragment;
        private letras_Fragment mLetras_Fragment;
        private Fragment2_fragment mFragment2_fragment;
        private Fragment3_fragment mFragment3_fragment;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //textMessage = FindViewById<TextView>(Resource.Id.message);

            //Navigation
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);


            //Fragmentos
            mLetras_Fragment = new letras_Fragment();
            mFragment2_fragment = new Fragment2_fragment();
            mFragment3_fragment = new Fragment3_fragment();

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Add(Resource.Id.content_frame, mLetras_Fragment, "Letras_Fragment");
            trans.Hide(mLetras_Fragment);

            trans.Add(Resource.Id.content_frame, mFragment2_fragment, "Fragment2_fragment");
            trans.Hide(mFragment2_fragment);

            trans.Add(Resource.Id.content_frame, mFragment3_fragment, "Fragment3_fragment");
            trans.Hide(mFragment3_fragment);


            /// ... add more fragments
            /// 
            trans.Commit();

            mCurrentFragment = mLetras_Fragment;


            //FloatActionButton
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            CreateDataBase();
        }

        private void CreateDataBase()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.Error("Error CreateDataBase", ex.Message);
            }
        }

        private void FabOnClick(object sender, System.EventArgs e)
        {

            //mLetras_Fragment.AddLiryc();
            //View view = (View)sender;
            //Snackbar.Make(view, "Test", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();

            //StartActivity(typeof(letras_nueva_activity(mLetras_Fragment)));
            //https://www.youtube.com/watch?v=sj-eXmpRjtI
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            string title = "";//Resource.String.app_name;

            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    ShowFragment(mLetras_Fragment);

                    //textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    ShowFragment(mFragment2_fragment);
                    //textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    ShowFragment(mFragment3_fragment);
                    //textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }

            return false;
        }

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

            //// set the toolbar title
            //if (getSupportActionBar() != null)
            //{
            //    getSupportActionBar().setTitle(title);
            //}

            //DrawerLayout drawer = (DrawerLayout)findViewById(R.id.drawer_layout);
            //drawer.closeDrawer(GravityCompat.START);

        }
    }
}

