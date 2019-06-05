
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "@string/Backup", Theme = "@style/Theme.AppCompat.Light.NoActionBar", NoHistory = true)]
    public class backup_activity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                SetContentView(Resource.Layout.backup_layout);


                //Toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.backup_toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = GetString(Resource.String.Backup);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);//backbutton
                SupportActionBar.SetHomeButtonEnabled(true);

                //Button
                Button backup_btn = FindViewById<Button>(Resource.Id.backup_btn);
                backup_btn.Click += Backup_Btn_Click;
            }
            catch (Exception ex)
            {
                Log.Error("OnCreate", ex.Message);
            }
        }//OnCreate

        void Backup_Btn_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Backup", ToastLength.Long).Show();
        }//Backup_Btn_Click

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }//OnOptionsItemSelected

    }
}
