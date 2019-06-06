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
    public class Letra
    {
        [PrimaryKey, AutoIncrement]
        public int IdLetra { get; set; }

        public string Titulo { get; set; }

        public string Autor { get; set; }

        public string Album { get; set; }

        public string letra { get; set; }

        public bool EsFavorita { get; set; }

        public override string ToString()
        {
            return string.Format("[Muusika - Storage and Share]\n\n*{0}*\nby *{1}*\nfrom *{2}*\n\nLyric: \n\n*\n{3}\n*", Titulo, Autor, Album, letra);
        }
    }
}