
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.IO;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Provider;
using Android.Database;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Webkit;
using Muusika.Resources.Utils;
using Android.Media;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "letras_attach_activity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class letras_attach_activity : AppCompatActivity
    {
        ImageView image1;
        WebView webView1;

        private int IdLyric;

        private const int PICK_AUDIO_REQUEST = 70;
        private const int PICK_IMAGE_REQUEST = 71;
        private const int PICK_VIDEO_REQUEST = 72;

        private const int REQUEST_READEXTERNALSTORAGE = 1000;
        private const int REQUEST_INTERNET = 1001;


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

                var image_imageButton = FindViewById<ImageButton>(Resource.Id.image_imageButton);
                image_imageButton.Click += Image_ImageButton_Click;

                var videoWeb_imageButton = FindViewById<ImageButton>(Resource.Id.videoWeb_imageButton);
                videoWeb_imageButton.Click += VideoWeb_ImageButton_Click;

                //Aditionals
                image1 = FindViewById<ImageView>(Resource.Id.imageView1);

                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Internet }, REQUEST_INTERNET);


                //webView
                webView1 = FindViewById<WebView>(Resource.Id.webView1);
                WebSettings webSettings = webView1.Settings;
                webSettings.JavaScriptEnabled = true;
                webView1.SetWebChromeClient(new WebChromeClient());
                webView1.LoadData("<iframe frameborder=\"0\" scrolling=\"no\" marginheight=\"0\" marginwidth=\"0\"width=\"788.54\" height=\"443\" type=\"text/html\" src=\"https://www.youtube.com/embed/DBXH9jJRaDk?autoplay=0&fs=0&iv_load_policy=3&showinfo=0&rel=0&cc_load_policy=0&start=0&end=0&origin=https://youtubeembedcode.com\"><div><small><a href=\"https://youtubeembedcode.com/nl/\">youtubeembedcode nl</a></small></div><div><small><a href=\"https://misshowtostartablog.com/best-webhosting-for-wordpress-blogs/\">https://misshowtostartablog.com/best-webhosting-for-wordpress-blogs/</a></small></div></iframe>", "text/html", "UTF-8");


                //Get IdLyric from putextra
                IdLyric = Convert.ToInt32(Intent.GetStringExtra("IdLyric"));

            }
            catch (Exception ex)
            {
                Log.Error("OnCreate", ex.Message);
            }
        }//OnCreate

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change main_compat_menu
            MenuInflater.Inflate(Resource.Menu.letras_main_toolbar, menu);
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
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadExternalStorage }, REQUEST_READEXTERNALSTORAGE);

                Intent intent = new Intent();
                intent.SetType("audio/*");
                intent.PutExtra(Intent.ExtraAllowMultiple, true);
                intent.SetAction(Intent.ActionGetContent);
                intent.AddCategory(Intent.CategoryOpenable);
                //intent.SetAction(Intent.ActionOpenDocument);
                //StartActivityForResult(intent, 1);
                StartActivityForResult(Intent.CreateChooser(intent, "Select audio file"), PICK_AUDIO_REQUEST);

            }
            catch (Exception ex)
            {
                Log.Error("Audio_ImageButton_Click", ex.Message);
            }
        }

        void Image_ImageButton_Click(object sender, EventArgs e)
        {
            try
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadExternalStorage }, REQUEST_READEXTERNALSTORAGE);

                Intent intent = new Intent();
                intent.SetAction(Intent.ActionGetContent);
                intent.SetType("image/*");
                StartActivityForResult(Intent.CreateChooser(intent, "select picture"), PICK_IMAGE_REQUEST);
            }
            catch (Exception ex)
            {
                Log.Error("Image_ImageButton_Click", ex.Message);
            }
        }

        void VideoWeb_ImageButton_Click(object sender, EventArgs e)
        {
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            try
            {
                if (resultCode == Result.Ok)
                {
                    if (data != null && data.Data != null)
                    {
                        string filePath;



                        switch (requestCode)
                        {
                            case PICK_AUDIO_REQUEST://Audio


                                Android.Net.Uri audioUri = data.Data;
                                filePath = Uris.GetPath(this, audioUri);

                                int IdAttached;
                                AttachedController attachedController = new AttachedController();


                                IdAttached = attachedController.Add((int)IdLyric, "AUDIO", filePath, System.IO.Path.GetFileName(filePath));

                                switch (IdAttached)
                                {
                                    case 0:
                                        //Duplicate attached
                                        Toast toast = Toast.MakeText(this, Resource.String.message_Attached_alredy_added, ToastLength.Long);
                                        toast.SetGravity(GravityFlags.Center, 0, 0);
                                        toast.Show();
                                        break;
                                    case -1:
                                        //Error adding
                                        break;
                                    default:
                                        //Add succefull

                                        

                                        break;
                                }

                                break;
                            case PICK_IMAGE_REQUEST: //Image

                                Android.Net.Uri imageUri = data.Data;
                                Bitmap bitmap;

                                //https://www.youtube.com/watch?v=3OINnrebrkQ
                                //https://stackoverflow.com/questions/34809847/how-to-store-the-audio-video-file-to-sqlite-database-in-android

                                try
                                {
                                    bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, imageUri);
                                    image1.SetImageBitmap(bitmap);
                                }
                                catch (Java.IO.IOException ex)
                                {
                                    ex.PrintStackTrace();
                                    image1.SetImageURI(imageUri);
                                }

                                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted)
                                {
                                    // We have permission, go ahead and use the camera.
                                    Toast.MakeText(this, "Tiene permiso", ToastLength.Short).Show();
                                    Toast.MakeText(this, "2" + GetPathToImage(imageUri), ToastLength.Short).Show();

                                }
                                else
                                {
                                    // Camera permission is not granted. If necessary display rationale & request.
                                    Toast.MakeText(this, "No tiene permiso", ToastLength.Short).Show();

                                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadExternalStorage }, REQUEST_READEXTERNALSTORAGE);
                                }



                                image1.Visibility = ViewStates.Visible;


                                String imagePath = GetPathToImage(imageUri);

                                bitmap = BitmapFactory.DecodeFile(imagePath);
                                Toast.MakeText(this, "" + bitmap, ToastLength.Short).Show();

                                //ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                                bitmap.Compress(Bitmap.CompressFormat.Png, 100, memoryStream);
                                byte[] img = memoryStream.ToArray();

                                Toast.MakeText(this, "" + img, ToastLength.Short).Show();

                                break;
                            case PICK_VIDEO_REQUEST:


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
        }//OnActivityResult

        //https://stackoverflow.com/questions/26597811/xamarin-choose-image-from-gallery-path-is-null
        //https://github.com/xamarin/docs-archive/tree/master/Recipes/android/other_ux/pick_image
        private string GetPathToImage(Android.Net.Uri uri)
        {
            ICursor cursor = this.ContentResolver.Query(uri, null, null, null, null);
            cursor.MoveToFirst();
            string document_id = cursor.GetString(0);
            document_id = document_id.Split(':')[1];
            cursor.Close();

            cursor = ContentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new String[] { document_id }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == REQUEST_READEXTERNALSTORAGE)
            {
                string TAG = "OnRequestPermissionsResult";

                // Received permission result for camera permission.
                Log.Info(TAG, "Received response for READEXTERNAL permission request.");

                // Check if the only required permission has been granted
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    // READEXTERNAL permission has been granted, okay to retrieve the location of the device.
                    Log.Info(TAG, "READEXTERNAL permission has now been granted.");
                    //Snackbar.Make(layout, Resource.String.permission_available_camera, Snackbar.LengthShort).Show();
                    Toast.MakeText(this, "READEXTERNAL permission has now been granted.", ToastLength.Long).Show();
                }
                else
                {
                    Log.Info(TAG, "READEXTERNAL permission was NOT granted.");
                    //Snackbar.Make(layout, Resource.String.permissions_not_granted, Snackbar.LengthShort).Show();
                    Toast.MakeText(this, "READEXTERNAL permission was NOT granted.", ToastLength.Long).Show();
                }
            }
            else
            {
                //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }

            if (requestCode == REQUEST_INTERNET)
            {
                string TAG = "OnRequestPermissionsResult";

                // Received permission result for camera permission.
                Log.Info(TAG, "Received response for READEXTERNAL permission request.");

                // Check if the only required permission has been granted
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    // REQUEST_INTERNET permission has been granted, okay to retrieve the location of the device.
                    Log.Info(TAG, "REQUEST_INTERNET permission has now been granted.");
                    //Snackbar.Make(layout, Resource.String.permission_available_camera, Snackbar.LengthShort).Show();
                    Toast.MakeText(this, "REQUEST_INTERNET permission has now been granted.", ToastLength.Long).Show();
                }
                else
                {
                    Log.Info(TAG, "REQUEST_INTERNET permission was NOT granted.");
                    //Snackbar.Make(layout, Resource.String.permissions_not_granted, Snackbar.LengthShort).Show();
                    Toast.MakeText(this, "REQUEST_INTERNET permission was NOT granted.", ToastLength.Long).Show();
                }
            }

            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }//OnRequestPermissionsResult

    }
}
