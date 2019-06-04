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
                Log.Error("CreateDatabase", ex.Message);
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
                Log.Error("InsertIntoTableLetras", ex.Message);
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
                Log.Error("SelectTableLetras", ex.Message);
                return null;
            }
        }

        public List<Letra> FilterTableLetras(string filterQuery)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    string strQuery = "SELECT * FROM Letra WHERE Titulo like '%" + filterQuery + "%' OR Autor like '%" + filterQuery + "%' OR Album like '%" + filterQuery + "%' OR letra like '%" + filterQuery + "%'";
                    return connection.Query<Letra>(strQuery);
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("FilterTableLetras", ex.Message);
                return null;
            }
        }

        public bool UpdateTableLetras(Letra letra)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Query<Letra>("UPDATE Letra SET Titulo=?, Autor=?, Album=?, letra=? WHERE IdLetra=?", letra.Titulo, letra.Autor, letra.Album, letra.letra, letra.IdLetra);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateTableLetras", ex.Message);
                return false;
            }
        }

        public bool SelectQueryTableLetras(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Query<Letra>("SELECT * FROM Letra WHERE IdLetra=?",Id);
                    return true;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("SelectQueryTableLetras", ex.Message);
                return false;
            }
        }

        public Letra SelectQueryTableLetrasById(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    var Lista = connection.Query<Letra>("SELECT * FROM Letra WHERE IdLetra=?", Id);
                    return Lista[0];
                }
            }
            catch (SQLiteException ex)
            {
                Log.Error("SelectQueryTableLetrasById", ex.Message);
                return null;
            }
        }

        public bool DeleteTableLetras(Letra letra)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Delete(letra);
                    return true;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("DeleteTableLetras", ex.Message);
                return false;
            }
        }

        public DataBase()
        {
        }
    }
}
