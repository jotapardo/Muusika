
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
    [Activity(Label = "letras_attach_activity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class letras_attach_activity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                SetContentView(Resource.Layout.letras_attach_layout);


                //toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_attach_toolbar);
                SetSupportActionBar(toolbar);
                //backbutton
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.Title = GetString(Resource.String.Attach);//context.Resources.GetString(Resource.String.KeyfortheString)

                //buttons
                var audio_imageButton = FindViewById<ImageButton>(Resource.Id.audio_imageButton);
                audio_imageButton.Click += Audio_ImageButton_Click;
            }
            catch (Exception ex)
            {
                Log.Error("OnCreate", ex.Message);
            }
        }//OnCreate

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change main_compat_menu
            MenuInflater.Inflate(Resource.Menu.general_toolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }//OnCreateOptionsMenu

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

        private void Audio_ImageButton_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent();
                intent.SetType("*/*");
                intent.PutExtra(Intent.ExtraAllowMultiple, true);
                intent.SetAction(Intent.ActionGetContent);
                intent.AddCategory(Intent.CategoryOpenable);
                //intent.SetAction(Intent.ActionOpenDocument);
                //StartActivityForResult(intent, 1);
                StartActivityForResult(Intent.CreateChooser(intent, "Select a file"), 123);


            }
            catch (Exception ex)
            {
                Log.Error("Audio_ImageButton_Click", ex.Message);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            try
            {
                if (resultCode == Result.Ok)
                {
                    if (data != null)
                    {
                        switch (requestCode)
                        {
                            case 123:

                                break;
                            default:
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnActivityResult", ex.Message);
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

    }
}
