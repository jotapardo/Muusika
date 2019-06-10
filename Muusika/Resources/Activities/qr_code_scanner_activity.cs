
using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Barcodes;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using static Android.Gms.Vision.Detector;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "qr_code_scanner_activity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class qr_code_scanner_activity : AppCompatActivity, ISurfaceHolderCallback, IProcessor
    {
        TextView result_textView;
        SurfaceView camera_surfaceView;
        BarcodeDetector barCodeDetector;
        CameraSource cameraSource;

        const int CAMERA_REQUEST_PERMISSION = 1101;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.qr_code_scanner_layout);

            result_textView = FindViewById<TextView>(Resource.Id.result_textView);
            camera_surfaceView = FindViewById<SurfaceView>(Resource.Id.camera_surfaceView);

            barCodeDetector = new BarcodeDetector.Builder(this)
                .SetBarcodeFormats(BarcodeFormat.QrCode)
                .Build();
            cameraSource = new CameraSource
                .Builder(this, barCodeDetector)
                .SetRequestedPreviewSize(640, 480)
                .Build();
            camera_surfaceView.Holder.AddCallback(this);

            barCodeDetector.SetProcessor(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case CAMERA_REQUEST_PERMISSION:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
                            {
                                ActivityCompat.RequestPermissions(this, new string[]
                                {
                                Manifest.Permission.Camera
                                }, CAMERA_REQUEST_PERMISSION);
                                return;
                            }

                            try
                            {
                                cameraSource.Start(camera_surfaceView.Holder);
                            }
                            catch (InvalidOperationException ex)
                            {

                            }
                        }

                    }
                    break;
                default:
                    break;
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                if(ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new string[]
                    {
                        Manifest.Permission.Camera
                    }, CAMERA_REQUEST_PERMISSION);
                    return;
                }

                try
                {
                    cameraSource.Start(camera_surfaceView.Holder);
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error("SurfaceCreated", ex.Message);
                }
            }
            catch (Exception ex)
            {
                Log.Error("SurfaceCreated", ex.Message);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            cameraSource.Stop();
        }

        public void ReceiveDetections(Detections detections)
        {
            SparseArray qrcodes = detections.DetectedItems;
            if (qrcodes.Size() != 0)
            {
                result_textView.Post(() => {
                    Vibrator vibrator = (Vibrator)GetSystemService(Context.VibratorService);
                    vibrator.Vibrate(1000);
                    result_textView.Text = ((Barcode)qrcodes.ValueAt(0)).RawValue;

                    //Toast.MakeText(this, result_textView.Text, ToastLength.Short).Show();

                    Intent intent = new Intent();
                    intent.PutExtra("QRcodeResult", result_textView.Text);
                    SetResult(Result.Ok, intent);

                    Finish();
                });
            }
        }

        public void Release()
        {

        }
    }
}
