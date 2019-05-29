using System;
using System.Collections.Generic;
using Android.Util;
using Muusika.Resources.model;
using SQLite;

namespace Muusika.Resources.DataHelper
{
    public class DataBase
    {
        string folder = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public bool CreateDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.CreateTable<Letra>();
                    return true;
                }
                
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTableLetras(Letra letra)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Insert(letra);
                    return true;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<Letra> SelectTableLetras()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    return connection.Table<Letra>().ToList();
                }

            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public DataBase()
        {
        }
    }
}
