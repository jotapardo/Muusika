using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Muusika.Resources;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;

namespace Muusika
{
    public class letras_Fragment: Android.Support.V4.App.Fragment
    {
        ListView lstData;
        List<Letra> lstSorce = new List<Letra>();
        DataBase db;

        public letras_Fragment()
        {
            //Create Database
            db = new DataBase();
            db.CreateDatabase();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.letras_layout, container, false);

            lstData = view.FindViewById<ListView>(Resource.Id.LetrasListView);

            LoadData();

            return view;
        }

        public void LoadData()
        {
            lstSorce = db.SelectTableLetras();
            var adapter = new letras_listViewAdapter(this, lstSorce);

            lstData.Adapter = adapter;
        }

        public void AddLiryc(string title, string author, string album, string liryc)
        {
            try
            {
                Letra mLetra = new Letra()
                {
                    Autor = author,
                    Album = album,
                    Titulo = title,
                    letra = liryc
                };
                db.InsertIntoTableLetras(mLetra);
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error("Error letras_Fragment", ex.Message);
            }
            

        }
    }
}