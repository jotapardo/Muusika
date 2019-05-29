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

        public string Artista { get; set; }

        public string Album { get; set; }

        public string letra { get; set; }

        public override string ToString()
        {
            return string.Format("[{0} by {1} \n {2}]", Titulo, Artista, letra);
        }
    }
}