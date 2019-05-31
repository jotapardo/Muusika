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
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace Muusika
{
    public class letras_Fragment: Android.Support.V4.App.Fragment, ISelectedChecker
    {
        ListView _LetrasListView;
        List<Letra> _Letras;
        List<Letra> _LetrasSeleccionadas;
  
        letras_listViewAdapter _LetrasAdapter;

        DataBase db;
        bool SelectingMultipleItems = false;

        SupportToolbar toolbar;

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

            toolbar = this.Activity.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.letras_main_toolbar);


        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.letras_layout, container, false);

            _LetrasListView = view.FindViewById<ListView>(Resource.Id.LetrasListView);

            _LetrasListView.ItemClick += LetrasListView_ItemClick;

            _LetrasListView.ItemLongClick += LetrasListView_ItemLongClick;

            LoadData();

            return view;
        }

        private void LetrasListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            try
            {
                ////set backgroud for selected item
                //for (int i = 0; i < _LetrasListView.Count; i++)
                //{
                //    if (e.Position == i)
                //        _LetrasListView.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.BlueViolet);
                //    else
                //        _LetrasListView.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
                //}

                if (SelectingMultipleItems)
                {
                    int id = (int)e.Id;
                    View view = e.View;

                    SelectUnselectElements(id, view, e.Position);
                }
                else
                {
                    Intent intent;

                    intent = new Intent(this.Activity, typeof(letras_viewer_activity));
                    intent.PutExtra("IdLetra", _LetrasListView.Adapter.GetItemId(e.Position).ToString());
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error_LetrasListView_ItemClick", ex.Message);
            }
        }

        private void LetrasListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            try
            {
                int id = (int)e.Id;
                View view = e.View;

                SelectUnselectElements(id, view, e.Position);

            }
            catch (Exception ex)
            {
                Log.Error("Error_LetrasListView_ItemLongClick",ex.Message);
            }
        }

        private void SelectUnselectElements(int id, View view, int Position)
        {
            try
            {
                LinearLayout linearLayout = view.FindViewById<LinearLayout>(Resource.Id.LetrasListItem_LinearLayout);

                if (_LetrasSeleccionadas.Select(x => x.IdLetra).Contains(id))
                {
                    //deselect element
                    var colorForUnselected = Android.Graphics.Color.Transparent;
                    linearLayout.SetBackgroundColor(colorForUnselected);
                    _LetrasSeleccionadas.Remove(_LetrasAdapter[Position]);
                }
                else
                {
                    //select element
                    var colorForSelected = Android.Graphics.Color.ParseColor(Resources.GetString(Resource.Color.colorListItemSelected));
                    linearLayout.SetBackgroundColor(colorForSelected);
                    _LetrasSeleccionadas.Add(_LetrasAdapter[Position]);
                }

                if (_LetrasSeleccionadas.Count > 0)
                {
                    SelectingMultipleItems = true;
                    //this.Activity.SupportActionBar.Title = "Muusika";

                }
                else
                {
                    SelectingMultipleItems = false;
                }
                    

                //Forces Android to execute OnCreateOptionsMenu
                this.Activity.InvalidateOptionsMenu();
            }
            catch (Exception ex)
            {
                Log.Error("Error_SelectUnselectElements", ex.Message);
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



        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_delete:
                    Toast.MakeText(this.Activity, "Eliminar!", ToastLength.Short).Show();
                    return true;
                case Resource.Id.home:
                    Toast.MakeText(this.Activity, "HOME!", ToastLength.Short).Show();
                    return true;
            }

            return false;
        }

        public void LoadData()
        {
            _Letras = db.SelectTableLetras();
            _LetrasAdapter = new letras_listViewAdapter(this, _Letras);
            _LetrasListView.Adapter = _LetrasAdapter;
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

        public void DeleteLirycs()
        {
            try
            {
                foreach (Letra letra in _LetrasSeleccionadas)
                {
                    db.deleteTableLetras(letra);
                }
                _LetrasSeleccionadas.Clear();
                LoadData();
            }
            catch (Exception ex)
            {
                Log.Error("Error letras_Fragment", ex.Message);
            }
        }
    }
}