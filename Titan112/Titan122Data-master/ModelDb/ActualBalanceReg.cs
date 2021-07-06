using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Titan112Data
{
    //[Index(nameof(User))]
    public class ActualBalanceReg : NsgDataItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual UserItemDb User { get; set; }
        public double Balance { get; set; }
        public int Visits { get; set; }
        public int Months { get; set; }
        public DateTime LastChanged { get; set; }

        public static ActualBalanceReg Empty => new ActualBalanceReg() { Id = 0, User = null, Balance = 0, Visits = 0, Months = 0 };
        public static ActualBalanceReg GetUserActualBalance(UserItemDb user)
        {
            var db = new TitanDBContext();
            var balanceItem = db.ActualBalanceRegs.Where(e => e.User == user).FirstOrDefault();
            if (balanceItem == null) return ActualBalanceReg.Empty;
            return balanceItem;
        }

        public static double CalcUserBalance(TitanDBContext db, UserItemDb user)
        {
            var sum = db.PaymentDocs.Where(r => r.User == user && r.Processed).Sum(r => r.Sum);
            //var months = db.PaymentDocs.Where(r => r.User == user && r.Processed).Sum(r => r.Months);
            //var visits = db.PaymentDocs.Where(r => r.User == user && r.Processed).Sum(r => r.Visits);
            var balanceItem = db.ActualBalanceRegs.Where(e => e.User == user).FirstOrDefault();
            if (balanceItem == null)
            {
                balanceItem = new ActualBalanceReg()
                {
                    User = user.FindUserInDb(db),
                    Balance = sum,
                    //Visits = visits,
                    //Months = months,
                    LastChanged = DateTime.Now
                };
                db.Add(balanceItem);
            }
            balanceItem.Balance = sum;
            

            db.SaveChanges();
            return sum;
        }
        /// <summary>
        /// Проверить наличие средств и активировать аккаунт после пополнения
        /// </summary>
        /// <param name="db">TitanDBContext</param>
        /// <param name="user">Пользователь</param>
        /// <param name="newBalance">Новый баланс</param>
        public static void CheckUserSubscription(TitanDBContext db, UserItemDb user, PaymentRequest paymentRequest, double newBalance)
        {
            if (paymentRequest != null && paymentRequest.tarifRequest != null)
            {
                if (newBalance >= paymentRequest.tarifRequest.Sum)
                {
                    var doc = new PaymentDoc();
                    doc.WithdrawalPayment(db, user, paymentRequest.tarifRequest.TarifPack.Price, 0, 
                        paymentRequest.tarifRequest.TarifPack.MonthToBuy, ActionTypes.MountlyPayment);
                    UserSubsctription.AddSubscription(db, user, doc, paymentRequest.tarifRequest.Tarif, paymentRequest.tarifRequest.TarifPack.MonthToBuy);
                    db.SaveChanges();
                }
            }
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.ActualBalanceRegs.ToList<NsgDataItem>();
            return list;
        }

    }
}

