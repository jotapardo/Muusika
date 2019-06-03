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
using Plugin.Clipboard;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "letras_viewer_activity", Theme = "@style/Theme.AppCompat.Light.NoActionBar", NoHistory = false)]
    public class letras_viewer_activity : AppCompatActivity
    {
        TextView LirycTextView;
        int IdLetra;
        Letra mLetra = new Letra();
        DataBase db;

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

                LirycTextView = FindViewById<TextView>(Resource.Id.LirycTextView);

                //Toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_viewer_toolbar);
                SetSupportActionBar(toolbar);
                //backbutton
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);

                SupportActionBar.Title = "NombreCancion";
                LirycTextView.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

                //Get IdLetra from putextra
                IdLetra = Convert.ToInt32(Intent.GetStringExtra("IdLetra"));

                //Retrieve liryc
                RetrieveLiryc(IdLetra);
            }
            catch (Exception ex)
            {
                Log.Error("Error_OnCreate", ex.Message);
            }

        }

        private void RetrieveLiryc(int idLetra)
        {
            try
            {
                mLetra = db.SelectQueryTableLetrasById(idLetra);
                SupportActionBar.Title = mLetra.Titulo;
                SupportActionBar.Subtitle = mLetra.Autor;
                LirycTextView.Text = mLetra.letra;

                //Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }
            catch (Exception ex)
            {
                Log.Error("ErrorRetrieveLiryc", ex.Message);
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
                    Intent intent = new Intent(this, typeof(letras_attach_activity));
                    StartActivity(intent);
                    break;
                case Resource.Id.action_edit:
                    Intent intentEdit = new Intent(this, typeof(letras_edit_activity));
                    intentEdit.PutExtra("IdLiryc", IdLetra.ToString());
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
                    Intent intentsend = new Intent();
                    intentsend.SetAction(Intent.ActionSend);
                    intentsend.PutExtra(Intent.ExtraText, mLetra.ToString());
                    intentsend.SetType("text/plain");
                    StartActivity(intentsend);
                    break;
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }//OnOptionsItemSelected

        protected override void OnResume()
        {
            RetrieveLiryc(IdLetra);
            base.OnResume();
        }

        protected override void OnRestart()
        {
            RetrieveLiryc(IdLetra);
            base.OnRestart();
        }


    }

    

}