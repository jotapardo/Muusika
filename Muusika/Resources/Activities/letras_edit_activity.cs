
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;
using Newtonsoft.Json;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "@string/title_edit_Lyric", Theme = "@style/Theme.AppCompat.Light.NoActionBar", NoHistory = false)]
    public class letras_edit_activity : AppCompatActivity
    {
        EditText TitleEditText;
        EditText AuthorEditText;
        EditText AlbumEditText;
        EditText LyricEditText;
        int intIdLyric;
        DataBase db;

        private letras_Fragment mLetras_Fragment;
        private Lyric mLyric;

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
                LyricEditText = FindViewById<EditText>(Resource.Id.LyricEditText);


                //Toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_edit_toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);//backbutton
                SupportActionBar.SetHomeButtonEnabled(true);


                //Fragmento
                mLetras_Fragment = new letras_Fragment();


                //Params
                intIdLyric = Convert.ToInt32(Intent.GetStringExtra("IdLyric"));


                //Call methods
                RetrieveLyric(intIdLyric);
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
                    UpdateLyric();
                    break;
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }//OnOptionItemSelected

        private void RetrieveLyric(int IdLyric)
        {
            try
            {
                mLyric = db.SelectQueryTableLyricsById(IdLyric);
                TitleEditText.Text = mLyric.Title;
                AuthorEditText.Text = mLyric.Author;
                AlbumEditText.Text = mLyric.Album;
                LyricEditText.Text = mLyric.lyric;

                //Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }
            catch (Exception ex)
            {
                Log.Error("ErrorRetrieveLyric", ex.Message);
            }
        }//RetrieveLyric

        private void UpdateLyric()
        {
            try
            {
                bool Validated = true;

                //Validate fields
                if (TextUtils.IsEmpty(TitleEditText.Text))
                {
                    TitleEditText.SetError(GetString(Resource.String.message_text_empty), null);
                    Validated = false;
                }

                if (TextUtils.IsEmpty(LyricEditText.Text))
                {
                    LyricEditText.SetError(GetString(Resource.String.message_text_empty), null);
                    Validated = false;
                }

                if (Validated)
                {
                    if (mLetras_Fragment.EditLyric(TitleEditText.Text, AuthorEditText.Text, AlbumEditText.Text, LyricEditText.Text, intIdLyric))
                    {
                        Toast.MakeText(this, "Letra editada correctamente!", ToastLength.Short).Show();
                        Finish();
                    }
                }

                
            }
            catch (Exception ex)
            {
                Log.Error("UpdateLyric", ex.Message);
            }
        }
    }
}
