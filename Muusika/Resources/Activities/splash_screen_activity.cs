using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "@string/app_name"
        , Theme = "@style/SplashTheme" //@style/Theme.AppCompat.Light.NoActionBar
        , MainLauncher = true
        , NoHistory = true
        , ConfigurationChanges = ConfigChanges.ScreenSize)] 
    public class splash_screen_activity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MainActivity));


            //SetContentView(Resource.Layout.activity_main);

            //Task taskSplashScreen = new Task(() => { ShowSplashScreen(); });
            //taskSplashScreen.Start();
        }

        //private async void ShowSplashScreen()
        //{
        //    await Task.Delay(1000);
        //    StartActivity(new Android.Content.Intent(this,typeof(MainActivity)));
        //}
    }
}