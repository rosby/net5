using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class TarifChangeRequestDb : NsgDataItem
    {
        public Guid Id { get; set; }
        [Required]
        public virtual UserItemDb User { get; set; }
        [Required]
        public virtual TarifItemDb Tarif { get; set; }
        [Required]
        public virtual TarifPackDb TarifPack { get; set; }
        public DateTime Date { get; set; }
        public double Sum { get; set; }
        public String PromoCode { get; set; }
        public bool IsActive { get; set; }
        public String Result { get; set; }
        static public TarifChangeRequestDb FindById(TitanDBContext db, UserItemDb user, Guid id)
        {
            if (id == Guid.Empty) return null;
            var req = db.TarifChangeRequests.Find(id);
            if (user != null && req != null && req.User.Id != user.Id) return null;
            return req;
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.TarifChangeRequests.ToList<NsgDataItem>();
            return list;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Tarif.ToString()) & string.IsNullOrEmpty(TarifPack.ToString()) & string.IsNullOrEmpty(User.ToString()))
                return Id.ToString();
            else
                return $"User:{User} | Tariff:{Tarif} | TariffPak:{TarifPack}";
        }
    }

}
