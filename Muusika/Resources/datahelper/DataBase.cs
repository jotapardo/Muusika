using System;
using System.Collections.Generic;
using Android.Util;
using Muusika.Resources.model;
using SQLite;

namespace Muusika.Resources.DataHelper
{
    public class DataBase
    {
        public DataBase()
        {
        }

        string folder = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public bool CreateDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.CreateTable<Lyric>();
                    connection.CreateTable<Attachment>();
                    return true;
                }
                
            }
            catch (SQLiteException ex)
            {
                Log.Error("CreateDatabase", ex.Message);
                return false;
            }
        }

        #region Lyrics

        public bool InsertIntoTableLyrics(Lyric lyric)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Insert(lyric);
                    return true;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("InsertIntoTableLyrics", ex.Message);
                return false;
            }
        }

        public List<Lyric> SelectTableLyrics()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    return connection.Table<Lyric>().ToList();
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("SelectTableLyrics", ex.Message);
                return null;
            }
        }

        public List<Lyric> FilterTableLyrics(string filterQuery)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    string strQuery = "SELECT * FROM Lyric WHERE Title like '%" + filterQuery + "%' OR Author like '%" + filterQuery + "%' OR Album like '%" + filterQuery + "%' OR lyric like '%" + filterQuery + "%'";
                    return connection.Query<Lyric>(strQuery);
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("FilterTableLyrics", ex.Message);
                return null;
            }
        }

        public bool UpdateTableLyrics(Lyric lyric)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Query<Lyric>("UPDATE Lyric SET Title=?, Author=?, Album=?, lyric=? WHERE IdLyric=?", lyric.Title, lyric.Author, lyric.Album, lyric.lyric, lyric.IdLyric);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateTableLyrics", ex.Message);
                return false;
            }
        }

        public bool SelectQueryTableLyrics(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Query<Lyric>("SELECT * FROM Lyric WHERE IdLyric=?", Id);
                    return true;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("SelectQueryTableLyrics", ex.Message);
                return false;
            }
        }

        public Lyric SelectQueryTableLyricsById(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    var Lista = connection.Query<Lyric>("SELECT * FROM Lyric WHERE IdLyric=?", Id);
                    return Lista[0];
                }
            }
            catch (SQLiteException ex)
            {
                Log.Error("SelectQueryTableLyricsById", ex.Message);
                return null;
            }
        }

        public Lyric SelectQueryTableLyricsByObjetc(Lyric lyric)
        {
            try
            {
                string Title = lyric.Title;
                string Author = lyric.Author;
                string Lyric = lyric.lyric.Replace("\n", "");
                string Album = lyric.Album;

                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    var Lista = connection.Query<Lyric>("SELECT * FROM Lyric WHERE Title=? AND Author=? AND lyric=? and Album=?", Title, Author, Lyric, Album);

                    if (Lista.Count > 0)
                        return Lista[0];
                    else
                        return null;

                }
            }
            catch (SQLiteException ex)
            {
                Log.Error("SelectQueryTableLyricsByObjetc", ex.Message);
                return null;
            }
        }

        public bool DeleteTableLyrics(Lyric lyric)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Delete(lyric);
                    return true;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("DeleteTableLyrics", ex.Message);
                return false;
            }
        }


        #endregion

        #region Attachment

        public bool InsertIntoTableAttachment(Attachment attachment)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Insert(attachment);
                    return true;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("InsertIntoTableAttachment", ex.Message);
                return false;
            }
        }

        public List<Attachment> SelectTableAttachmentByIdLyric(int IdLyric)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    return connection.Query<Attachment>("SELECT * FROM Attachment WHERE IdLyric=?", IdLyric);
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("SelectTableAttachmentByIdLyric", ex.Message);
                return null;
            }
        }

        public Attachment SelectTableAttachmentByIdAttachment(int idAttachment)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    var List = connection.Query<Attachment>("SELECT * FROM Attachment WHERE IdAttachment=?", idAttachment);

                    if (List.Count > 0)
                        return List[0];
                    else
                        return null;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Error("SelectTableAttachmentByIdAttachment", ex.Message);
                return null;
            }
        }

        public Attachment SelectQueryTableAttachmentByObjetc(Attachment attachment)
        {
            try
            {
                int IdLyric = attachment.IdLyric;
                string Type = attachment.Type;
                string Path = attachment.Path;

                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    var Lista = connection.Query<Attachment>("SELECT * FROM Attachment WHERE IdLyric=? AND Type=? AND Path=?", IdLyric, Type, Path);

                    if (Lista.Count > 0)
                        return Lista[0];
                    else
                        return null;

                }
            }
            catch (SQLiteException ex)
            {
                Log.Error("SelectQueryTableAttachmentByObjetc", ex.Message);
                return null;
            }
        }

        public bool DeleteTableAttachment(Attachment attachment)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Delete(attachment);
                    return true;
                }

            }
            catch (SQLiteException ex)
            {
                Log.Error("DeleteTableAttachment", ex.Message);
                return false;
            }
        }

        public bool UpdateTableAttachment(Attachment attachment)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Muusika.db")))
                {
                    connection.Query<Lyric>("UPDATE Attachment SET Name=? WHERE IdAttachment=?", attachment.Name, attachment.IdAttachment);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UpdateTableLyrics", ex.Message);
                return false;
            }
        }
        #endregion

    }
}
