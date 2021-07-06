using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class AlarmItemDb : NsgDataItem
    {
        public Guid Id { get; set; }
        public virtual UserItemDb User { get; set; }
        public int Status { get; set; }
        public int InitialTimeToArrive { get; set; }
        public double InitialDistance { get; set; }
        public DateTime BeginAlarmTime { get; set; }
        public DateTime BeginMovingTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime CloseTime { get; set; }
        public bool IsClosed { get; set; }
        public double UserLat { get; set; }
        public double UserLng { get; set; }
        public virtual CarItemDb Car { get; set; }
        public virtual GuardianItemDb Guardian1 { get; set; }
        public virtual GuardianItemDb Guardian2 { get; set; }
        public String Comment { get; set; }
        public double ServicePrice { get; set; }
        public virtual PaymentDoc PaymantDoc { get; set; }
        public virtual AlarmCancelReasonDb CancelReason { get; set; }
        public String CalcelReasonComment { get; set; }

        public bool CancelAlarm(TitanDBContext db, AlarmItemDb alarm, UserItemDb user, bool closeWithoutPenalty, string comment)
        {
            if (comment == "")
                comment = "Тревога отменена пользователем";
            if (Status == AlarmManagementStatus.searching)
            {
                //Режим поиска - просто отменяем тревогу без последствий
                Status = AlarmManagementStatus.canceled;
                Comment = comment;
                if (closeWithoutPenalty) IsClosed = true;
                CloseTime = DateTime.Now;
                ServicePrice = 0;
                if (PaymantDoc != null && PaymantDoc.Processed)
                    PaymantDoc.CancelWithdrow(db, comment);
                db.SaveChanges();
                
                return true;
            }
            if (Status == AlarmManagementStatus.carMoving)
            {
                //закрываем тревогу со штрафом, зависящим от времени, прошедшего с начала тревоги
                Status = AlarmManagementStatus.canceled;
                CloseTime = DateTime.Now;
                if (PaymantDoc != null && PaymantDoc.Processed)
                {
                    PaymantDoc.CancelWithdrow(db, comment);
                }
                ServicePrice = user.Tarif.VisitPrice;
                if (closeWithoutPenalty)
                {
                    IsClosed = true;
                    ServicePrice = 0;
                }
                else
                {
                    if (!IsEarlyCancelation(db, alarm))
                    {
                        ServicePrice = ServicePrice / 10;
                        var paymentDoc = new PaymentDoc();
                        paymentDoc.WithdrawalPayment(db, user, ServicePrice / 10, 0, 0, ActionTypes.Payment);
                    }
                }
                db.SaveChanges();
                return true;
            }
            if (Status == AlarmManagementStatus.carArrived)
            {
                ServicePrice = user.Tarif.VisitPrice;
                //закрываем тревогу по сигналу от пользователя
                Status = AlarmManagementStatus.closed;
                CloseTime = DateTime.Now;
                
                db.SaveChanges();
                return true;
            }

            return false;
        }

        private bool IsEarlyCancelation(TitanDBContext db, AlarmItemDb alarm)
        {
            if ((DateTime.Now - alarm.BeginAlarmTime).TotalSeconds > 120)
                return false;
            else
                return true;
        }

        public AlarmItemDb FindById(TitanDBContext db)
        {
            return db.Alarms.Find(Id);
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.Alarms.ToList<NsgDataItem>();
            return list;
        }
    }
}
