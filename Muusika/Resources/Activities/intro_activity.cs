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
    public class intro_activity : AppIntro.AppIntro2, ISlideBackgroundColorHolder
    {

        /// <summary>
        /// https://github.com/JonDouglas/AppIntro Xamarin documentation
        /// https://github.com/PaoloRotolo/AppIntro#appintro Java documentation
        /// </summary>
        /// <param name="savedInstanceState"></param>

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AddSlide(AppIntro.AppIntroFragment.NewInstance("TERE TULEMAST"
                , "Es hora de ponerte en modo Muusika. \n (Por cierto 'Tere tulemast', quiere decir BIENVENIDO en el idioma Estonio)"
                , Resource.Drawable.Muusika_icon
                ,Color.LightGray));//Color.ParseColor("#f64c73")

            AddSlide(AppIntro.AppIntroFragment.NewInstance("Almacena tus letras"
                , "Crea ru propia colección de letras, aunque estas no existan ni en Internet"
                , Resource.Drawable.img_create
                , Color.ParseColor("#20d2bb")));

            AddSlide(AppIntro.AppIntroFragment.NewInstance("Comparte con amigos"
                , "Envía tus letras a tus amigos de forma sencilla"
                , Resource.Drawable.img_share
                , Color.ParseColor("#3395ff")));

            AddSlide(AppIntro.AppIntroFragment.NewInstance("Importa fácilmente"
                , "Utiliza la opción de importar para que crees una letra que te comparta un amigo desde su Muusika"
                , Resource.Drawable.img_discover
                , Color.ParseColor("#f64c73")));

            AddSlide(AppIntro.AppIntroFragment.NewInstance(GetString(Resource.String.app_name)
                , "COMENZAR"
                , Resource.Drawable.img_nodata
                , Color.ParseColor("#c873f4")));

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
            StartActivity(new Intent(this, typeof(MainActivity)));
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