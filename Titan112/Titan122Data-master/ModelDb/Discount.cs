using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class Discount : NsgDataItem
    {
        public int Id { get; set; }
        public virtual TarifItemDb Tarif { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public String PromoCode { get; set; }
        public String Comment { get; set; }
        public double Procent { get; set; }

        public static double GetDiscount(TitanDBContext db, TarifItemDb tarif, String promoCode)
        {
            var discount = FindDiscount(db, tarif, promoCode);
            if (discount == null) return 0.0;
            else return discount.Procent;
        }
        public static Discount FindDiscount(TitanDBContext db, TarifItemDb tarif, String promoCode)
        {
            var discount = db.Discounts.AsQueryable().
                Where(e => e.ValidFrom < DateTime.Now && e.ValidTo > DateTime.Now &&
                (e.Tarif == tarif || e.Tarif == null) && (e.PromoCode == promoCode)
                && e.IsActive).FirstOrDefault();
            return discount;
        }
        public static void InitialFill(DbSet<Discount> discounts, TitanDBContext db)
        {
            if (discounts.Count() > 0) return;
            var e = new Discount()
            {
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddYears(1),
                IsActive = true,
                Procent = 10,
                PromoCode = "TITAN",
                Comment = "ДЕМО скидка",

            };
            discounts.Add(e);
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.Discounts.ToList<NsgDataItem>();
            return list;
        }
    }
}
