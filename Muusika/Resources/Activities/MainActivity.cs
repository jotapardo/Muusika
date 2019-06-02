﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;
using Newtonsoft.Json;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity: AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        //TextView textMessage;
        private SupportFragment mCurrentFragment;
        private letras_Fragment mLetras_Fragment;
        private Fragment2_fragment mFragment2_fragment;
        private Fragment3_fragment mFragment3_fragment;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
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

                //Toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_main_toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = GetString(Resource.String.app_name);
                

            }
            catch (Exception ex)
            {
                Log.Error("OnCreate", ex.Message);
            }
        }//oncreate

        private void FabOnClick(object sender, System.EventArgs e)
        {

            //mLetras_Fragment.AddLiryc();
            //View view = (View)sender;
            //Snackbar.Make(view, "Test", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();




            //StartActivity(typeof(letras_nueva_activity(mLetras_Fragment)));
            //https://www.youtube.com/watch?v=sj-eXmpRjtI

            Intent intent = new Intent(this, typeof(letras_nueva_activity));
            intent.PutExtra("Letras_Fragment", JsonConvert.SerializeObject(mLetras_Fragment));
            StartActivity(intent);


        }//FabOnClick

        protected override void OnResume()
        {
            mLetras_Fragment.LoadData();
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

            //// set the toolbar title
            //if (getSupportActionBar() != null)
            //{
            //    getSupportActionBar().setTitle(title);
            //}

            //DrawerLayout drawer = (DrawerLayout)findViewById(R.id.drawer_layout);
            //drawer.closeDrawer(GravityCompat.START);

        }//ShowFragment

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            try
            {
                if (mCurrentFragment == mLetras_Fragment)
                {
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
                //case Resource.Id.action_edit:
                //    Toast.MakeText(this, "You pressed edit action!", ToastLength.Short).Show();
                //    break;
                case Resource.Id.action_delete:
                    if (mLetras_Fragment.DeleteLirycs())
                    {
                        Toast.MakeText(this, "Eliminadas!", ToastLength.Short).Show();
                    }
                    //hideAndShowKeyboard.hideSoftKeyboard(this);
                    break;
                case Android.Resource.Id.Home:
                    //hideAndShowKeyboard.hideSoftKeyboard(this);
                    this.OnBackPressed();
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
        }
    }
}
