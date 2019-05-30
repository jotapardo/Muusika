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
using Muusika.Resources.Activities;
using Muusika.Resources.Adapters;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;
using NotiXamarin.Adapters;

namespace Muusika
{
    public class letras_Fragment: Android.Support.V4.App.Fragment, ISelectedChecker
    {
        ListView _LetrasListView;
        List<Letra> _Letras;
        List<Letra> _LetrasSeleccionadas;
        Letras_ListAdapter _LetrasAdapter;
        DataBase db;

        public letras_Fragment()
        {
            //Create Database
            db = new DataBase();
            db.CreateDatabase();

            //Listas
            _Letras = new List<Letra>();
            _LetrasSeleccionadas = new List<Letra>();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _LetrasSeleccionadas.Clear();
            //SetHasOptionsMenu(true);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.letras_layout, container, false);

            _LetrasListView = view.FindViewById<ListView>(Resource.Id.LetrasListView);

            _LetrasListView.ItemClick += (s, e) =>
            {
                ////set backgroud for selected item
                //for (int i = 0; i < _LetrasListView.Count; i++)
                //{
                //    if (e.Position == i)
                //        _LetrasListView.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.BlueViolet);
                //    else
                //        _LetrasListView.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
                //}

                Intent intent;

                intent = new Intent(this.Activity, typeof(letras_viewer_activity));
                intent.PutExtra("IdLetra", _LetrasListView.Adapter.GetItemId(e.Position).ToString());
                StartActivity(intent);
            };

            _LetrasListView.ItemLongClick += LetrasListView_ItemLongClick;

            LoadData();

            return view;
        }

        private void LetrasListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            try
            {
                int id = (int)e.Id;
                View view = e.View;

                LinearLayout linearLayout = view.FindViewById<LinearLayout>(Resource.Id.LetrasListItem_LinearLayout);

                if (_LetrasSeleccionadas.Select(x => x.IdLetra).Contains(id))
                {
                    //deselect element
                    var colorForUnselected = Android.Graphics.Color.Transparent;
                    linearLayout.SetBackgroundColor(colorForUnselected);
                    _LetrasSeleccionadas.Remove(_LetrasAdapter[e.Position]);
                }
                else
                {
                    //select element
                    var colorForSelected = Android.Graphics.Color.BlueViolet;
                    linearLayout.SetBackgroundColor(colorForSelected);
                    _LetrasSeleccionadas.Remove(_LetrasAdapter[e.Position]);
                }

                //Forces Android to execute OnCreateOptionsMenu
                Activity.InvalidateOptionsMenu();
            }
            catch (Exception ex)
            {
                Log.Error("Error_LetrasListView_ItemLongClick",ex.Message);
            }
        }

        protected void UnselectElements()
        {
            int count = _LetrasListView.ChildCount;

            for (int i = 0; i < count; i++)
            {
                View row = _LetrasListView.GetChildAt(i);
                var rl = row.FindViewById<RelativeLayout>(Resource.Id.LetrasListItem_LinearLayout);
                var color = Android.Graphics.Color.Transparent;
                rl.SetBackgroundColor(color);
            }
        }

        public bool IsItemSelected(int id)
        {
            return _LetrasSeleccionadas.Select(x => x.IdLetra).Contains(id);
        }
        public void LoadData()
        {
            _Letras = db.SelectTableLetras();
            var adapter = new letras_listViewAdapter(this, _Letras);
            _LetrasListView.Adapter = adapter;
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