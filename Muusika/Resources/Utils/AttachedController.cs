using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using Android.Widget;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;

namespace Muusika.Resources.Utils
{
    public class AttachedController
    {
        private DataBase db;

        public AttachedController()
        {
            //Create Database
            db = new DataBase();
            db.CreateDatabase();
        }

        public bool AlreadyExistObjetc(Attached mAttached)
        {
            try
            {
                Attached attached;

                attached = db.SelectQueryTableAttachedByObjetc(mAttached);

                if (attached != null)
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

        /// <summary>
        /// Add new Attached object
        /// </summary>
        /// <param name="pIdLyric"></param>
        /// <param name="pType"></param>
        /// <param name="pPath"></param>
        /// <param name="pName"></param>
        /// <returns>IdAttached</returns>
        public int Add(int pIdLyric, string pType, string pPath, string pName)
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

                if (AlreadyExistObjetc(mAttached))
                {
                    return 0;
                   
                }
                else
                {
                    db.InsertIntoTableAttached(mAttached);
                    return db.SelectQueryTableAttachedByObjetc(mAttached).IdAttached;
                    //LoadData();
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error AddLyric", ex.Message);
                return -1;
            }
        }//AddLyric

        public List<Attached> GetList(int pIdLyric)
        {
            return db.SelectTableAttachedByIdLyric(pIdLyric).ToList();
        }

    }
}
