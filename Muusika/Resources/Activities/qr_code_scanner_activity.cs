
using System;
using System.Collections.Generic;
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
                .SetAutoFocusEnabled(true)
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
            if (cameraSource != null)
            {
                cameraSource.Stop();
                cameraSource.Release();
                cameraSource = null;
            }
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

        public override bool OnTouchEvent(MotionEvent e)
        {
            try
            {
                if (e.Action == MotionEventActions.Up)
                {
                    float x = e.XPrecision;
                    float y = e.YPrecision;
                    float touchMajor = e.TouchMajor;
                    float touchMinor = e.TouchMinor;

                    Rect touchRect = new Rect((int)(x - touchMajor / 2), (int)(y - touchMinor / 2), (int)(x + touchMajor / 2), (int)(y + touchMinor / 2));

                    SubmitFocusAreaRect(touchRect);
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnTouchEvent", ex.Message);
            }
            return base.OnTouchEvent(e);
        }

        private void SubmitFocusAreaRect(Rect touchRect)
        {
            try
            {
                Java.Lang.Reflect.Field[] declaredFields = cameraSource.Class.GetFields();

                foreach (var field in declaredFields)
                {
                    if (field.GetType() == typeof(Android.Hardware.Camera))
                    {
                        field.Accessible = true;

                        try
                        {
                            Android.Hardware.Camera camera = (Android.Hardware.Camera) field.Get(cameraSource);

                            if (camera != null)
                            {
                                Android.Hardware.Camera.Parameters cameraparameters = camera.GetParameters();

                                if (cameraparameters.MaxNumFocusAreas == 0)
                                {
                                    return;
                                }

                                Rect focusArea = new Rect();

                                focusArea.Set(touchRect.Left * 2000 / camera_surfaceView.Width - 1000,
                                                touchRect.Top * 2000 / camera_surfaceView.Height - 1000,
                                                touchRect.Right * 2000 / camera_surfaceView.Width - 1000,
                                                touchRect.Bottom * 2000 / camera_surfaceView.Height - 1000);

                                List<Android.Hardware.Camera.Area> focusAreas = new List<Android.Hardware.Camera.Area>();
                                focusAreas.Add(new Android.Hardware.Camera.Area(focusArea, 1000));

                                cameraparameters.FocusMode = Android.Hardware.Camera.Parameters.FocusModeAuto;
                                cameraparameters.FocusAreas = focusAreas;

                                camera.SetParameters(cameraparameters);

                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("SubmitFocusAreaRect_Internal", ex.Message);
                        }
                    }

                }//foreach


            }
            catch (Exception ex)
            {
                Log.Error("SubmitFocusAreaRect", ex.Message);
            }
            
        }//SubmitFocusAreaRect
    }
}
