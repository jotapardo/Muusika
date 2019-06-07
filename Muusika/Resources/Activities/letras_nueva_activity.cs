using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.IO;
using Muusika.Resources.Utils;
using Newtonsoft.Json;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika
{
    [Activity(Label = "@string/title_add_Lyric", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class letras_nueva_activity : AppCompatActivity, ISerializable
    {
        EditText TitleEditText;
        EditText AuthorEditText;
        EditText AlbumEditText;
        EditText LyricEditText;

        TextInputLayout title_textInputLayout;
        TextInputLayout author_textInputLayout;
        TextInputLayout album_textInputLayout;
        TextInputLayout lyric_textInputLayout;

        HideAndShowKeyboard hideAndShowKeyboard = new HideAndShowKeyboard();

        private letras_Fragment mLetras_Fragment;

        private Vibrator vibrator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                SetContentView(Resource.Layout.letras_nueva_layout);

                TitleEditText = FindViewById<EditText>(Resource.Id.TitleEditText);
                AuthorEditText = FindViewById<EditText>(Resource.Id.AuthorEditText);
                AlbumEditText = FindViewById<EditText>(Resource.Id.AlbumEditText);
                LyricEditText = FindViewById<EditText>(Resource.Id.LyricEditText);


                //Toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_nueva_toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);//backbutton
                SupportActionBar.SetHomeButtonEnabled(true);


                //Fragmento
                mLetras_Fragment = JsonConvert.DeserializeObject<letras_Fragment>(Intent.GetStringExtra("Letras_Fragment"));


                //keyboard
                hideAndShowKeyboard.showSoftKeyboard(this, TitleEditText);


                //vibrator
                vibrator = (Vibrator)GetSystemService(Context.VibratorService);

                //TextImputLayout
                title_textInputLayout = FindViewById<TextInputLayout>(Resource.Id.title_textInputLayout);
                author_textInputLayout = FindViewById<TextInputLayout>(Resource.Id.author_textInputLayout);
                album_textInputLayout = FindViewById<TextInputLayout>(Resource.Id.album_textInputLayout);
                lyric_textInputLayout = FindViewById<TextInputLayout>(Resource.Id.lyric_textInputLayout);

            }
            catch (Exception ex)
            {
                Log.Error("Error_OnCreate", ex.Message);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change main_compat_menu
            MenuInflater.Inflate(Resource.Menu.letras_nueva_toolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //case Resource.Id.action_edit:
                //    Toast.MakeText(this, "You pressed edit action!", ToastLength.Short).Show();
                //    break;
                case Resource.Id.action_save:

                    AddLyric();
                    break;
                case Android.Resource.Id.Home:
                    hideAndShowKeyboard.hideSoftKeyboard(this);
                    this.OnBackPressed();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void AddLyric()
        {
            try
            {
                bool Validated = true;

                //Validate fields
                if (TextUtils.IsEmpty(TitleEditText.Text))
                {
                    TitleEditText.SetError(GetString(Resource.String.message_text_empty), null);
                    Validated = false;
                    vibrator.Vibrate(120);
                    title_textInputLayout.ErrorEnabled = true;
                    title_textInputLayout.Error = "Ingresa un título";
                }

                if (TextUtils.IsEmpty(LyricEditText.Text))
                {
                    LyricEditText.SetError(GetString(Resource.String.message_text_empty), null);
                    Validated = false;
                    vibrator.Vibrate(120);
                    lyric_textInputLayout.ErrorEnabled = true;
                    lyric_textInputLayout.Error = "Ingresa la letra";
                }

                if (Validated)
                {
                    title_textInputLayout.ErrorEnabled = false;
                    lyric_textInputLayout.ErrorEnabled = false;

                    mLetras_Fragment.AddLyric(TitleEditText.Text, AuthorEditText.Text, AlbumEditText.Text, LyricEditText.Text);
                    Toast.MakeText(this, "Letra añadida correctamente!", ToastLength.Short).Show();
                    hideAndShowKeyboard.hideSoftKeyboard(this);
                    Finish();
                }
            }
            catch (Exception ex)
            {
                Log.Error("AddLyric", ex.Message);
            }
        }
    }
}