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
using SQLite;

namespace Muusika.Resources.model
{
    public class Lyric
    {
        [PrimaryKey, AutoIncrement]
        public int IdLyric { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Album { get; set; }

        public string lyric { get; set; }

        public bool IsFavorite { get; set; }

        public override string ToString()
        {
            return string.Format("[Muusika - Storage and Share]\n\n*{0}*\nby *{1}*\nfrom *{2}*\n\nLyric: \n\n*\n{3}\n*", Title, Author, Album, lyric);
        }
    }
}