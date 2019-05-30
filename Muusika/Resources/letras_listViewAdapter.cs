using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Muusika.Resources.model;

namespace Muusika.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView TituloTextView { get; set; }
        public TextView ArtistaTextView { get; set; }
        public TextView AlbumTextView { get; set; }
        public TextView AdjuntosTextView { get; set; }
    }

    public class letras_listViewAdapter : BaseAdapter
    {
        private Android.Support.V4.App.Fragment letras_fragment;
        private List<Letra> lstLetras;

        public letras_listViewAdapter(Android.Support.V4.App.Fragment letras_fragment, List<Letra> lstLetras)
        {
            this.letras_fragment = letras_fragment;
            this.lstLetras = lstLetras;
        }

        public override int Count
        {
            get
            {
                return lstLetras.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return lstLetras[position].IdLetra;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? letras_fragment.LayoutInflater.Inflate(Resource.Layout.letras_layout_listView_dataTemplate, parent, false);
              
            var TituloTextView = view.FindViewById<TextView>(Resource.Id.TituloTextView);
            var ArtistaTextView = view.FindViewById<TextView>(Resource.Id.ArtistaTextView);
            var AlbumTextView = view.FindViewById<TextView>(Resource.Id.AlbumTextView);
            var AdjuntosTextView = view.FindViewById<TextView>(Resource.Id.AdjuntosTextView);

            TituloTextView.Text = lstLetras[position].Titulo;
            ArtistaTextView.Text = lstLetras[position].Autor;
            AlbumTextView.Text = lstLetras[position].Album;
            AdjuntosTextView.Text = "";

            return view;
        }
    }
}