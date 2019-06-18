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
    public class ViewHolder_Attachment : Java.Lang.Object
    {
        public TextView NameTextView { get; set; }
        public TextView PathTextView { get; set; }
        public ImageView TypeImageView { get; set; }

    }

    public class attachment_listViewAdapter : BaseAdapter<Attachment>
    {
        private Activity context;
        private List<Attachment> ListAttachment;

        public attachment_listViewAdapter(Activity context, List<Attachment> mListAttachment)
        {
            this.context = context;
            this.ListAttachment = mListAttachment;
        }

        public override Attachment this[int position] => ListAttachment[position];

        public override int Count => ListAttachment.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return ListAttachment[position].IdAttachment;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //Api 23
            LayoutInflater layoutInflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

            View view = convertView;

            view = convertView ?? layoutInflater.Inflate(Resource.Layout.letras_attachment_listView_dataTemplate, parent, false);

            var NameTextView = view.FindViewById<TextView>(Resource.Id.NameTextView);
            var PathTextView = view.FindViewById<TextView>(Resource.Id.PathTextView);
            var TypeImageView = view.FindViewById<ImageView>(Resource.Id.TypeImageView);


            NameTextView.Text = ListAttachment[position].Name;
            PathTextView.Text = ListAttachment[position].Path;
            switch (ListAttachment[position].Type)
            {
                case "AUDIO":
                    TypeImageView.SetImageResource(Resource.Drawable.img_music);
                    break;
                default:
                    break;
            }

            return view;
        }
    }
}