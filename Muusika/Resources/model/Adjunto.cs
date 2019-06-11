﻿using System;
using System.ComponentModel.DataAnnotations;
using SQLite;

namespace Muusika.Resources.model
{
    public class Adjunto
    {
        [PrimaryKey, AutoIncrement]
        public int IdAdjunto { get; set; }

        public int IdLetra { get; set; }

        public string Tipo { get; set; } //Audio, Imagen, Link, Video

        public string Ruta { get; set; }


        //public byte[] Imagen { get; set; }

        //[Required]
        //[Display(Name = "First Name")]
        //[StringLength(50, ErrorMessage = "First Name cannot exceed more than 50 characters")]
        //[RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "Name cannot have special character,numbers or space")]
        //[Column("FName")]
        //public string CFName { get; set; }

        //[Display(Name = "Middle Name")]
        //[RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "Middle Name cannot have special character,numbers or space")]
        //[StringLength(35, ErrorMessage = "Middle Name cannot have more than 35 characters")]
        //[Column("MName")]
        //public string CMName { get; set; }

    }
}
