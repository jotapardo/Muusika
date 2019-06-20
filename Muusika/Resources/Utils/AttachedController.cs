using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using Android.Widget;
using Muusika.Resources.DataHelper;
using Muusika.Resources.model;

namespace Muusika.Resources.Utils
{
    public class AttachmentController
    {
        private DataBase db;

        public AttachmentController()
        {
            //Create Database
            db = new DataBase();
            db.CreateDatabase();
        }

        public bool AlreadyExistObjetc(Attachment mAttachment)
        {
            try
            {
                Attachment attachment;

                attachment = db.SelectQueryTableAttachmentByObjetc(mAttachment);

                if (attachment != null)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                Log.Error("AlreadyExistAttachment", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Add new Attachment object
        /// </summary>
        /// <param name="pIdLyric"></param>
        /// <param name="pType"></param>
        /// <param name="pPath"></param>
        /// <param name="pName"></param>
        /// <returns>IdAttachment</returns>
        public int Add(int pIdLyric, string pType, string pPath, string pName)
        {
            try
            {
                Attachment mAttachment = new Attachment()
                {
                    IdLyric = pIdLyric,
                    Type = pType,
                    Path = pPath,
                    Name = pName
                };

                if (AlreadyExistObjetc(mAttachment))
                {
                    return 0;
                   
                }
                else
                {
                    db.InsertIntoTableAttachment(mAttachment);
                    return db.SelectQueryTableAttachmentByObjetc(mAttachment).IdAttachment;
                    //LoadData();
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error AddLyric", ex.Message);
                return -1;
            }
        }//AddLyric

        public List<Attachment> GetList(int pIdLyric)
        {
            return db.SelectTableAttachmentByIdLyric(pIdLyric).ToList();
        }

        public bool Update(string Name, int IdAttachment)
        {
            try
            {
                Attachment attachment = new Attachment()
                {
                    IdAttachment = IdAttachment,
                    Name = Name
                };

                return db.UpdateTableAttachment(attachment);
                
            }
            catch (Exception ex)
            {
                Log.Error("Update", ex.Message);
                return false;
            }
        }

    }
}
