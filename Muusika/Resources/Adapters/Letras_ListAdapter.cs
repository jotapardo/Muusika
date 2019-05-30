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
using Muusika.Resources.model;
using NotiXamarin.Adapters;

namespace Muusika.Resources.Adapters
{
    class Letras_ListAdapter : BaseAdapter<Letra>
    {
        private Activity _context;
        private List<Letra> _Letras;
        private ISelectedChecker _selectedChecker;


        public Letras_ListAdapter(Activity context, List<Letra> letras, ISelectedChecker selectedChecker)
        {
            _context = context;
            _Letras = letras;
            _selectedChecker = selectedChecker;
        }

        public override Letra this[int position] => _Letras[position];


        public override int Count => _Letras.Count;


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return this[position].IdLetra;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this[position];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.letras_layout_listView_dataTemplate, null);
            }
            else
            {
                var id = (int)GetItemId(position);
                RelativeLayout rl = convertView.FindViewById<RelativeLayout>(Resource.Id.LetrasListItem_LinearLayout);

                if (_selectedChecker.IsItemSelected(id))
                {
                    var colorForSelected = Android.Graphics.Color.BlueViolet;
                    rl.SetBackgroundColor(colorForSelected);
                }
                else
                {
                    var colorForUnselected = Android.Graphics.Color.Transparent;
                    rl.SetBackgroundColor(colorForUnselected);
                }
            }

            convertView.FindViewById<TextView>(Resource.Id.TituloTextView).Text = item.Titulo;
            //var newsImage = convertView.FindViewById<ImageView>(Resource.Id.newsImage);

            //var imageURL = string.Concat(ValuesService.ImagesBaseURL, item.ImageName);

            //Picasso.With(_context)
            //    .Load(imageURL)
            //    .Placeholder(_context.GetDrawable(Resource.Drawable.Icon))
            //    .Into(newsImage);

            return convertView;


            //var view = convertView;
            //Letras_ListAdapterViewHolder holder = null;

            //if (view != null)
            //    holder = view.Tag as Letras_ListAdapterViewHolder;

            //if (holder == null)
            //{
            //    holder = new Letras_ListAdapterViewHolder();
            //    var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            //    //replace with your item and your holder items
            //    //comment back in
            //    //view = inflater.Inflate(Resource.Layout.item, parent, false);
            //    //holder.Title = view.FindViewById<TextView>(Resource.Id.text);
            //    view.Tag = holder;
            //}


            ////fill in your items
            ////holder.Title.Text = "new text here";

            //return view;
        }

        
    }

    class Letras_ListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}