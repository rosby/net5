using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Titan112Data
{
    //[Index(nameof(User))]
    public class PaymentDoc : NsgDataItem
    {
        public Guid Id { get; set; }
        public String PaymentRequestId { get; set; }
        public DateTime Date { get; set; }
        public virtual UserItemDb User { get; set; }
        public int ActionType { get; set; }
        public double Sum { get; set; }
        public int Visits { get; set; }
        public int Months { get; set; }
        public bool Processed { get; set; }
        public String Comment { get; set; }

        public void ProcessPayment(TitanDBContext db, PaymentRequest paymentRequest)
        {
            using var transaction = db.Database.BeginTransaction();
            try
            {
                paymentRequest.Successful = true;
                paymentRequest.Processed = true;

                Id = Guid.NewGuid();
                PaymentRequestId = paymentRequest.RequestId;
                Date = DateTime.Now;
                User = paymentRequest.User;
                ActionType = ActionTypes.Payment;
                Sum = paymentRequest.Sum;
                Processed = true;

                db.PaymentDocs.Add(this);

                //Регистрируем запись в журнале регистрации
                var regRecord = new RegistrationRecord()
                {
                    Date = paymentRequest.Date,
                    User = paymentRequest.User,
                    Sum = paymentRequest.Sum,
                    ActionType = ActionTypes.Payment,
                    Comment = $"Пополнение счета на сумму {paymentRequest.Sum}, id заявки {paymentRequest.Id}"
                };
                db.RegistrationJournal.Add(regRecord);
                db.SaveChanges();

                var newBalance = ActualBalanceReg.CalcUserBalance(db, paymentRequest.User);
                ActualBalanceReg.CheckUserSubscription(db, User, paymentRequest, newBalance);
                
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR ProcessPayment. {e}");
                transaction.Rollback();
                throw;
            }
        }

        internal void CancelWithdrow(TitanDBContext db, string comment)
        {
            Processed = false;
            Comment = comment;

            //Регистрируем запись в журнале регистрации
            var regRecord = new RegistrationRecord()
            {
                Date = DateTime.Now,
                User = User,
                Sum = -Sum,
                ActionType = ActionTypes.CancelationPayment,
                Comment = $"comment, id платежа {Id}"
            };
            db.RegistrationJournal.Add(regRecord);
            db.SaveChanges();
           
        }

        public void WithdrawalPayment(TitanDBContext db, UserItemDb user, double sum, int visit, int month, int actionType)
        {
            if (sum <= 0)
                throw new Exception("Ошибка в сумме списания");
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            User = user.FindUserInDb(db);
            ActionType = actionType; //ActionTypes.MountlyPayment
            Sum = -sum;
            Visits = visit;
            Months = month;
           
            Processed = true;
            if (ActionType == ActionTypes.MountlyPayment)
                Comment = $"Списание абон. платы за {month} мес.";
            else
                Comment = $"Оплата за вызов {Sum}";

            db.PaymentDocs.Add(this);
            //Регистрируем запись в журнале регистрации
            var regRecord = new RegistrationRecord()
            {
                Date = this.Date,
                User = this.User,
                Sum = this.Sum,
                Visits = this.Visits,
                Months = this.Months,
                ActionType = ActionTypes.MountlyPayment,
                Comment = this.Comment
            };
            db.RegistrationJournal.Add(regRecord);
            db.SaveChanges();

            User.Balance = ActualBalanceReg.CalcUserBalance(db, User);
            db.SaveChanges();
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.PaymentDocs.ToList<NsgDataItem>();
            return list;
        }
    }
}

