using NsgServerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class TitanTokenItem : NsgTokenItem
    {
        private UserItemDb userDb;
        private DateTime loadTime;
        public UserItemDb UserDb
        {
            get { 
                if ((DateTime.Now - loadTime).TotalSeconds > 10)
                {
                    //reload user data
                    var db = new TitanDBContext();
                    UserDb = db.Users.Find(userDb.Id);
                }
                return userDb; 
            }
            set { userDb = value;
                loadTime = DateTime.Now;
            }
        }
    }
}
