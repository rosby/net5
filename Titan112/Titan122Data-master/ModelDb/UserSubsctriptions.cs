using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class UserSubsctription : NsgDataItem
    {
        public int Id { get; set; }
        [Required] 
        public virtual UserItemDb User { get; set; }
        [Required]
        public virtual TarifItemDb Tarif { get; set; }
        public virtual PaymentDoc PaymentDoc { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }

        static public void AddSubscription(TitanDBContext db, UserItemDb user, PaymentDoc doc, TarifItemDb tarif, int month)
        {
            //Проверяем не добавлялась ли подписка по данному документу ранее
            if (db.UserSubsctriptions.Where(e => e.User == user && e.PaymentDoc == doc).FirstOrDefault() != null)
                return;
            //Ищем активную подписку для продления
            var curDate = DateTime.Now.Date;
            var activeSubscription = db.UserSubsctriptions.Where(e => e.User == user && e.Tarif == tarif &&
                e.ValidFrom <= curDate && e.ValidTo>= curDate && e.IsActive).OrderBy(e => e.ValidTo).FirstOrDefault();
            var validFrom = curDate;
            if (activeSubscription != null)
            {
                validFrom = activeSubscription.ValidTo;

            }
            var validTo = validFrom.AddMonths(month);

            var sub = new UserSubsctription()
            {
                User = user,
                Tarif = tarif,
                PaymentDoc = doc,
                ValidFrom = validFrom,
                ValidTo = validTo,
                IsActive = true
            };
            db.UserSubsctriptions.Add(sub);
            if (!user.SubscriptionActive) user.SubscriptionActive = true;
            if (user.SubscriptionValidTo < validTo) user.SubscriptionValidTo = validTo;
            db.SaveChanges();
        }

        static public List<UserSubsctription> GetActiveSubscriptions(TitanDBContext db, UserItemDb user)
        {
            var curDate = DateTime.Now.Date;
            var activeSubscriptions = db.UserSubsctriptions.Where(e => e.User == user &&
                e.ValidFrom <= curDate && e.ValidTo >= curDate && e.IsActive).ToList();
            return activeSubscriptions;
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.UserSubsctriptions.ToList<NsgDataItem>();
            return list;
        }
    }

}
