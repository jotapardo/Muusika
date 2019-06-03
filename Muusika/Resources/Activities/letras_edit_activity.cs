
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
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;
using Newtonsoft.Json;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "@string/title_edit_liryc", Theme = "@style/Theme.AppCompat.Light.NoActionBar", NoHistory = false)]
    public class letras_edit_activity : AppCompatActivity
    {
        EditText TitleEditText;
        EditText AuthorEditText;
        EditText AlbumEditText;
        EditText LirycEditText;
        int intIdLiryc;
        DataBase db;

        private letras_Fragment mLetras_Fragment;
        private Letra mLiryc;

        public letras_edit_activity()
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
                SetContentView(Resource.Layout.letras_edit_layout);

                TitleEditText = FindViewById<EditText>(Resource.Id.TitleEditText);
                AuthorEditText = FindViewById<EditText>(Resource.Id.AuthorEditText);
                AlbumEditText = FindViewById<EditText>(Resource.Id.AlbumEditText);
                LirycEditText = FindViewById<EditText>(Resource.Id.LirycEditText);


                //Toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_edit_toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);//backbutton
                SupportActionBar.SetHomeButtonEnabled(true);


                //Fragmento
                mLetras_Fragment = new letras_Fragment();


                //Params
                intIdLiryc = Convert.ToInt32(Intent.GetStringExtra("IdLiryc"));


                //Call methods
                RetrieveLiryc(intIdLiryc);
            }
            catch (Exception ex)
            {
                Log.Error("Error_OnCreate", ex.Message);
            }

        }//OnCreate

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change main_compat_menu
            MenuInflater.Inflate(Resource.Menu.letras_edit_toolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_save:
                    if (mLetras_Fragment.EditLiryc(TitleEditText.Text, AuthorEditText.Text, AlbumEditText.Text, LirycEditText.Text, intIdLiryc))
                    {
                        Toast.MakeText(this, "Letra editada correctamente!", ToastLength.Short).Show();
                        Finish();
                    }
                    break;
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }//OnOptionItemSelected

        private void RetrieveLiryc(int IdLiryc)
        {
            try
            {
                mLiryc = db.SelectQueryTableLetrasById(IdLiryc);
                TitleEditText.Text = mLiryc.Titulo;
                AuthorEditText.Text = mLiryc.Autor;
                AlbumEditText.Text = mLiryc.Album;
                LirycEditText.Text = mLiryc.letra;

                //Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }
            catch (Exception ex)
            {
                Log.Error("ErrorRetrieveLiryc", ex.Message);
            }
        }//RetrieveLiryc
    }
}
