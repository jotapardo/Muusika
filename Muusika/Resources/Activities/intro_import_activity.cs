using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppIntro;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "into_activity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class intro_import_activity : AppIntro.AppIntro2, ISlideBackgroundColorHolder
    {

        /// <summary>
        /// https://github.com/JonDouglas/AppIntro Xamarin documentation
        /// https://github.com/PaoloRotolo/AppIntro#appintro Java documentation
        /// </summary>
        /// <param name="savedInstanceState"></param>

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AddSlide(AppIntro.AppIntroFragment.NewInstance("Importa letras"
                , "Utiliza esta opción para importar las letras que comparten contigo desde Muusika"
                , Resource.Drawable.mobile_receive
                ,Color.LightGray));//Color.ParseColor("#f64c73")

            AddSlide(AppIntro.AppIntroFragment.NewInstance("Elige las opciones"
                , "Puedes elegir entre importar desde un texto copiado o desde un código QR que escanees con tu cámara"
                , Resource.Drawable.img_qrcode
                , Color.ParseColor("#20d2bb")));


            //hide status bar with hour, signal, conection internet, etc
            ShowStatusBar(false);

            //SetBarColor(Color.ParseColor(GetString(Resource.Color.colorPrimary)));
            //SetSeparatorColor(Color.ParseColor("#2193F3"));

            ShowSkipButton(false);

            SetFadeAnimation();
            //SetZoomAnimation();//brusco
            //SetFlowAnimation();//Cubo interno
            //SetSlideOverAnimation();//Sobre y desaparece
            //SetDepthAnimation();//Quitar el slide brusco

            //askForPermissions(new String[]{Manifest.permission.CAMERA, Manifest.permission.READ_CONTACTS}, 2);

        }//OnCreate

        public override void OnDonePressed()
        {
            Finish();
            base.OnDonePressed();
        }

        public override void OnSkipPressed()
        {
            base.OnSkipPressed();
        }

        public override void OnSlideChanged()
        {
            base.OnSlideChanged();
        }

        public int GetDefaultBackgroundColor()
        {
            // Return the default background color of the slide.
            return Color.ParseColor("#000000");
        }

        public void SetBackgroundColor(int backgroundColor)
        {
            // Set the background color of the view within your slide to which the transition should be applied.
            //if (layoutContainer != null)
            //{
            //    layoutContainer.setBackgroundColor(backgroundColor);
            //}
        }
    }
}