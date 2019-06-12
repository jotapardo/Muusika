using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using Android.Widget;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;

namespace Muusika.Resources.Utils
{
    public class AdjuntoController
    {
        DataBase db;

        public AdjuntoController()
        {
            //Create Database
            db = new DataBase();
            db.CreateDatabase();
        }

        public bool AlreadyExistAttached(Attached mAttached)
        {
            try
            {
                Letra mLetra;

                mLetra = db.SelectQueryTableAttachedByObjetc(mAttached);

                if (mLetra != null)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                Log.Error("AlreadyExistAttached", ex.Message);
                return false;
            }
        }

        public void AddAttached(int pIdLyric, string pType, string pPath, string pName)
        {
            try
            {
                Attached mAttached = new Attached()
                {
                    IdLyric = pIdLyric,
                    Type = pType,
                    Path = pPath,
                    Name = pName
                };

                if (AlreadyExistAttached(mAttached))
                {
                    //Toast toast = Toast.MakeText(this.Activity, Resource.String.message_Lyric_alredy_added, ToastLength.Long);
                    //toast.SetGravity(GravityFlags.Center, 0, 0);
                    //toast.Show();
                }
                else
                {
                    db.InsertIntoTableAttached(mAttached);
                    //LoadData();
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error AddLyric", ex.Message);
            }
        }//AddLyric

        public List<Attached> GetList(int IdLyric)
        {
            return db.SelectTableAttachedByIdLyric(IdLyric).ToList();
        }

    }
}
