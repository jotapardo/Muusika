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

namespace Muusika.Resources.Adapters
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView TituloTextView { get; set; }
        public TextView ArtistaTextView { get; set; }
        public TextView AlbumTextView { get; set; }
        public TextView AdjuntosTextView { get; set; }
    }

    public class letras_listViewAdapter : BaseAdapter<Lyric>
    {
        private Android.Support.V4.App.Fragment letras_fragment;
        private List<Lyric> lstLetras;

        public letras_listViewAdapter(Android.Support.V4.App.Fragment letras_fragment, List<Lyric> lstLetras)
        {
            this.letras_fragment = letras_fragment;
            this.lstLetras = lstLetras;
        }

        public override Lyric this[int position] => lstLetras[position];

        public override int Count => lstLetras.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return lstLetras[position].IdLyric;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //Api 27
            //var view = convertView ?? letras_fragment.LayoutInflater.Inflate(Resource.Layout.letras_layout_listView_dataTemplate, parent, false);

            //Api 23
            LayoutInflater layoutInflater = (LayoutInflater)letras_fragment.Activity.GetSystemService(Context.LayoutInflaterService);
            var view = convertView ?? layoutInflater.Inflate(Resource.Layout.letras_layout_listView_dataTemplate, parent, false);

            var TituloTextView = view.FindViewById<TextView>(Resource.Id.TituloTextView);
            var ArtistaTextView = view.FindViewById<TextView>(Resource.Id.ArtistaTextView);
            var AlbumTextView = view.FindViewById<TextView>(Resource.Id.AlbumTextView);
            var AdjuntosTextView = view.FindViewById<TextView>(Resource.Id.AdjuntosTextView);

            TituloTextView.Text = lstLetras[position].Title;
            ArtistaTextView.Text = lstLetras[position].Author;
            AlbumTextView.Text = lstLetras[position].Album;
            AdjuntosTextView.Text = "";

            return view;
        }
    }
}