using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Titan112Data
{
    [Index(nameof(Phone), IsUnique = true)]
    public class UserItemDb : NsgDataItem
    {
        public Guid Id { get; set; }
        private string phone;

        [Required]
        public string Phone
        {
            get
            { return phone; }
            set
            {
                phone = cleanPhoneNumber(value);

            }
        }

        public string cleanPhoneNumber(string value)
        {
            return Regex.Replace(value, @"\D", "");
        }


        public virtual UserItemDb MasterUser { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyPosition { get; set; }
        public string CompanyName { get; set; }
        public string PhotoUri { get; set; }
        public double Balance { get; set; }
        [Required]
        public virtual TarifItemDb Tarif { get; set; }
        public bool SubscriptionActive { get; set; }
        public DateTime SubscriptionValidTo { get; set; }
        //public int SubscriptionVisits { get; set; }
        public int UserRegistrationStatus { get; set; }
        //На сегодня  255 - разрешена любая зона,
        //любое другое число - Санкт-Петербург
        //В дальнейшем будет заменена на справочник зон
        public int ServiceAreaId { get; set; }


        public UserItemDb()
        {
            Id = Guid.NewGuid();
        }
        public UserItemDb FindCreateUser(string phone, TitanDBContext db)
        {
            var user = db.Users.Where(e => e.Phone == cleanPhoneNumber(phone)).FirstOrDefault();
            if (user != null) return user;
            user = new UserItemDb();
            user.Phone = phone;
            user.Tarif = TarifItemDb.GetDefaultTarif(db);
            user.UserRegistrationStatus = 0;
            db.Users.Add(user);
            db.SaveChanges();
            Console.WriteLine($"created new user for phone {phone}, tarif {user.Tarif}");
            return user;
        }

        public UserItemDb FindUserInDb(TitanDBContext db)
        {
            return db.Users.Find(Id);
        }

        public double GetMinPayment(TitanDBContext db, Guid tarifChangeRequestId)
        {
            //TODO: Доделать проверку баланса
            if (tarifChangeRequestId == Guid.Empty)
                return 100;
            if (SubscriptionActive) return 100;
            var tarifRequest = TarifChangeRequestDb.FindById(db, this, tarifChangeRequestId);
            if (tarifRequest == null) throw new Exception("Не найден запрос на смену тарифа");
            return tarifRequest.Sum;
            //return Tarif.MountlyPrice * 12 + Tarif.VisitPrice - ActualBalanceReg.GetUserBalance(this);
        }

        /*internal UserItem ToUserItem()
        {
            if (Tarif == null) Tarif = TarifItemDb.GetDefaultTarif(TitanController.dbContext);
            return new UserItem()
            {
                Id = this.Id.ToString(),
                Phone = this.Phone,
                FirstName = this.FirstName,
                SecondName = this.SecondName,
                LastName = this.LastName,
                Email = this.Email,
                CompanyPosition = this.CompanyPosition,
                CompanyName = this.CompanyName,
                //TODO: формировать правильный URL картинки
                PhotoUri = this.PhotoUri,
                Balance = this.Balance,
                //TODO: заменить на ссылку на объект тарифа
                TarifId = this.Tarif.Id.ToString(),
                SubscriptionActive = this.SubscriptionActive,
                //TODO: Заменить на расчет тарифа
                SubscriptionValidUntil = this.SubscriptionValidUntil,
                //TODO: Заменить на перечисление
                SubscriptionPaymentMethod = this.SubscriptionPaymentMethod,
                //TODO: Заменить на перечисление
                VisitPaymentMethod = this.VisitPaymentMethod,
                UserRegistrationStatus = this.UserRegistrationStatus
            };
        }*/

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.Users.ToList<NsgDataItem>();
            return list;
        }
    }

    public class UserRegistrationStatusEnum
    {
        public static int notRegistered = 0;
        public static int photoRequired = 3;
        public static int registrationStarted = 10;
        public static int registered = 20;
    }
}
