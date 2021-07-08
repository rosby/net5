using NsgServerClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class UserTokens : NsgDataItem
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public virtual UserItemDb User { get; set; }
        public DateTime LastAccessTime { get; set; }
        [MaxLength(255)]
        public String DeviceDescription { get; set; }
        [MaxLength(255)]
        public String FirebaseId { get; set; }

        public void FillFromToken(INsgTokenExtension token)
        {
            Token = token.Token;
            LastAccessTime = DateTime.Now;
            var db = new TitanDBContext();
            var savedToken = db.UserTokens.AsQueryable().Where(e=> e.Token == token.Token).FirstOrDefault();
            if (savedToken != null)
            {
                token.UserId = savedToken.User.Id;
                (token as TitanTokenItem).UserDb = savedToken.User;
                return;
            }
            try
            {
                User = new UserItemDb().FindCreateUser(token.Phone, db);
                db.UserTokens.Add(this);
                db.SaveChanges();
                token.UserId = User.Id;
                (token as TitanTokenItem).UserDb = User;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception {e}");
                throw;
            }
            Console.WriteLine($"created token for phone {token.Phone}");
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.UserTokens.ToList<NsgDataItem>();
            return list;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Token))
                return Id.ToString();
            else
                return Token;
        }
    }
}
