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
using Java.IO;
using Newtonsoft.Json;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika
{
    [Activity(Label = "@string/title_add_liryc", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class letras_nueva_activity : AppCompatActivity, ISerializable
    {
        EditText TitleEditText;
        EditText AuthorEditText;
        EditText AlbumEditText;
        EditText LirycEditText;

        private letras_Fragment mLetras_Fragment;

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
                LirycEditText = FindViewById<EditText>(Resource.Id.LirycEditText);

                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_nueva_toolbar);
                SetSupportActionBar(toolbar);

                mLetras_Fragment = JsonConvert.DeserializeObject<letras_Fragment>(Intent.GetStringExtra("Letras_Fragment"));

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
                case Resource.Id.action_edit:
                    Toast.MakeText(this, "You pressed edit action!", ToastLength.Short).Show();
                    break;
                case Resource.Id.action_save:
                    Toast.MakeText(this, "Letra añadida correctamente!", ToastLength.Short).Show();
                    mLetras_Fragment.AddLiryc(TitleEditText.Text,AuthorEditText.Text,AlbumEditText.Text,LirycEditText.Text);
                    Finish();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}