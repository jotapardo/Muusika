using System;
using SQLite;

namespace Muusika.Resources.model
{
    public class Adjunto
    {
        [PrimaryKey, AutoIncrement]
        public int IdAdjunto { get; set; }

        public int IdLetra { get; set; }

        public string Tipo { get; set; } //Audio, Imagen

        public byte[] Imagen { get; set; }
        
    }
}
