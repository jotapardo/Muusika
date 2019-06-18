using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using HtmlAgilityPack;

namespace Muusika.Resources.Activities
{
    [Activity(Label = "letras_search_activity")]
    public class letras_search_activity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.letras_search_layout);

            //string searchText = "https://www.google.com/search?q=letra+colgando+en+tus+manos";

            //string textise = "https://www.textise.net/showText.aspx?strURL=https%253A//www.google.com/search%253Fq%253Dletra+colgando+en+tus+manos";

            //string ResultSearh = "";

            //EditText searh = FindViewById<EditText>(Resource.Id.search_textView)

            Button search_button = FindViewById<Button>(Resource.Id.search_button);
            search_button.Click += Search_button_Click;



        }//OnCreate

        //private void Search_button_Click(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        public async void Search_button_Click(object sender, EventArgs e)
        {
            try
            {
                string str = string.Empty;
                WebRequest request = HttpWebRequest.Create("https://www.textise.net/showText.aspx?strURL=https%253A//www.google.com/search%253Fq%253Dletra+colgando+en+tus+manos");
                WebResponse response = await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);

                var html = new HtmlAgilityPack.HtmlDocument();
                html.Load(response.GetResponseStream());

                ////If you don't need to parse HTML structure then you can just simply read with StreamReader:
                //using (var s = new StreamReader(response.GetResponseStream()))
                //{
                //    var html = await s.ReadToEndAsync();
                //}

                HtmlAgilityPack.HtmlNodeCollection col = html.DocumentNode.SelectNodes("//body");

                foreach (HtmlNode node in col)
                {
                    str += node.InnerText;
                }

                str = html.DocumentNode.SelectSingleNode("//body").InnerText;

            }
            catch (Exception ex)
            {
                Log.Error("OnSearch", ex.Message);
            }
        }
    }

   
}
