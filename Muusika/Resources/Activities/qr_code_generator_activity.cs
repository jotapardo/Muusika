
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
using ZXing;
using ZXing.Common;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "qr_code_generator_activity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class qr_code_generator_activity : AppCompatActivity
    {
        ImageView QRcode_imageView;
        Button moreOptions_button;
        string lyricText = string.Empty;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.qr_code_generator_layout);

            try
            {
                QRcode_imageView = FindViewById<ImageView>(Resource.Id.QRcode_imageView);
                moreOptions_button = FindViewById<Button>(Resource.Id.moreOptions_button);

                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    //Options = new EncodingOptions
                    //{
                    //    Height = 200,
                    //    Width = 600
                    //}
                };

                //Get lyric text from putextra
                lyricText = Intent.GetStringExtra("lyricText");

                var bitmap = writer.Write(lyricText);

                QRcode_imageView.SetImageBitmap(bitmap);

                moreOptions_button.Click += MoreOptions_Button_Click;

                //Toolbar
                SupportToolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.qr_code_generator_toolbar);
                SetSupportActionBar(toolbar);//backbutton
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.Title = "Escanéa el código";


            }
            catch (Exception ex)
            {
                Log.Error("OnCreate", ex.Message);
            }
        }

        void MoreOptions_Button_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intentsend = new Intent();
                intentsend.SetAction(Intent.ActionSend);
                intentsend.PutExtra(Intent.ExtraText, lyricText);
                intentsend.SetType("text/plain");
                StartActivity(intentsend);
            }
            catch (Exception ex)
            {
                Log.Error("MoreOptions_Button_Click", ex.Message);
            }
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
        }

    }
}
