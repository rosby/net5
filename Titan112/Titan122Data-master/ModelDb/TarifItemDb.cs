using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class TarifItemDb : NsgDataItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxSlaveCount { get; set; }
        public virtual TarifItemDb MasterTarif {get; set;}
        public double MinVisitCount { get; set; }
        public double VisitPrice { get; set; }
        public bool DefaultTarif { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveUntil { get; set; }
        public bool IsActive { get; set; }
        public bool AllowToSelect { get; set; }

        public TarifItemDb()
        {
            Id = Guid.NewGuid();
        }
       /* public TarifItem ToTarifItem()
        {
            return new TarifItem()
            {
                Id = this.Id.ToString(),
                Name = this.Name,
                MountlyPrice = this.MountlyPrice,
                VisitPrice = this.VisitPrice
            };
        }*/
        public static TarifItemDb GetDefaultTarif(TitanDBContext db)
        {
            return db.Tarifs.Where(e => e.DefaultTarif).SingleOrDefault();
        }

        public static void InitialFill(DbSet<TarifItemDb> tarifs, TitanDBContext db)
        {
            if (tarifs.Count<TarifItemDb>() > 3) return;
            #region "Персональный"
            var tarif = new TarifItemDb()
            {
                Id = Guid.NewGuid(),
                Name = "Персональный",
                DefaultTarif = true,
                MaxSlaveCount = 0,
                MinVisitCount = 1,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100,1,1),
                IsActive = true,
                AllowToSelect = true,
                VisitPrice = 1500,
                
            };
            tarifs.Add(tarif);
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "1 месяц",
                DefaultPack = true,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 1,
                VisitsToBuy = 0,
                Price = 500
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "3 месяца",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 3,
                VisitsToBuy = 0,
                Price = 450 * 3
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "6 месяцеы",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 6,
                VisitsToBuy = 0,
                Price = 350 * 6
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "Год",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 12,
                VisitsToBuy = 0,
                Price = 290 * 12
            });
            //-----------------------------------
            #endregion
            #region "Автомобилист"
            tarif = new TarifItemDb()
            {
                Id = Guid.NewGuid(),
                Name = "Автомобилист",
                DefaultTarif = false,
                MaxSlaveCount = 0,
                MinVisitCount = 1,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                VisitPrice = 1500,

            };
            tarifs.Add(tarif);
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "1 месяц",
                DefaultPack = true,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 1,
                VisitsToBuy = 0,
                Price = 700
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "3 месяца",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 3,
                VisitsToBuy = 0,
                Price = 600 * 3
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "6 месяцеы",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 6,
                VisitsToBuy = 0,
                Price = 500 * 6
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "Год",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 12,
                VisitsToBuy = 0,
                Price = 390 * 12
            });
            //-----------------------------------
            #endregion
            #region "Мобильный бизнес"
            tarif = new TarifItemDb()
            {
                Id = Guid.NewGuid(),
                Name = "Мобильный бизнес",
                DefaultTarif = false,
                MaxSlaveCount = 0,
                MinVisitCount = 1,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                VisitPrice = 1500,
            };
            tarifs.Add(tarif);
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "1 месяц",
                DefaultPack = true,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 1,
                VisitsToBuy = 0,
                Price = 1500,

            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "3 месяца",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 3,
                VisitsToBuy = 0,
                Price = 1300 * 3
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "6 месяцеы",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 6,
                VisitsToBuy = 0,
                Price = 1100 * 6
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "Год",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 12,
                VisitsToBuy = 0,
                Price = 900 * 12
            });
            //-----------------------------------
            #endregion
            #region "Забота"
            tarif = new TarifItemDb()
            {
                Id = Guid.NewGuid(),
                Name = "Забота",
                DefaultTarif = false,
                MaxSlaveCount = 0,
                MinVisitCount = 1,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                VisitPrice = 1500,
            };
            tarifs.Add(tarif);
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "1 месяц",
                DefaultPack = true,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 1,
                VisitsToBuy = 0,
                Price = 500
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "3 месяца",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 3,
                VisitsToBuy = 0,
                Price = 400 * 3
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "6 месяцеы",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 6,
                VisitsToBuy = 0,
                Price = 300 * 6
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "Год",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 12,
                VisitsToBuy = 0,
                Price = 190 * 12
            });
            //-----------------------------------
            #endregion
            #region "Семейный"
            tarif = new TarifItemDb()
            {
                Id = Guid.NewGuid(),
                Name = "Семья",
                DefaultTarif = false,
                MaxSlaveCount = 5,
                MinVisitCount = 2,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                VisitPrice = 1500,
            };
            tarifs.Add(tarif);
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "1 месяц",
                DefaultPack = true,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 1,
                VisitsToBuy = 0,
                Price = 500
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "3 месяца",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 3,
                VisitsToBuy = 0,
                Price = 450 * 3
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "6 месяцеы",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 6,
                VisitsToBuy = 0,
                Price = 350 * 6
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "Год",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 12,
                VisitsToBuy = 0,
                Price = 290 * 12
            });
            #endregion
            #region Дети"
            var masterTarif = tarif;
            tarif = new TarifItemDb()
            {
                Id = Guid.NewGuid(),
                Name = "Дети",
                DefaultTarif = false,
                MaxSlaveCount = 0,
                MasterTarif = tarif,
                MinVisitCount = 0,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                VisitPrice = 1500,
            };
            tarifs.Add(tarif);
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "1 месяц",
                DefaultPack = true,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 1,
                VisitsToBuy = 0,
                Price = 500
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "3 месяца",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 3,
                VisitsToBuy = 0,
                Price = 400*3
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "6 месяцеы",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 6,
                VisitsToBuy = 0,
                Price = 300*6
            });
            db.TarifPacks.Add(new TarifPackDb()
            {
                Id = Guid.NewGuid(),
                Name = "Год",
                DefaultPack = false,
                Tarif = tarif,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = true,
                MonthToBuy = 12,
                VisitsToBuy = 0,
                Price = 150 * 12
            });
            //-----------------------------------
            #endregion
            #region "Корпоративный"
            tarif = new TarifItemDb()
            {
                Id = Guid.NewGuid(),
                Name = "Корпоративный",
                DefaultTarif = false,
                MaxSlaveCount = 1000,
                MinVisitCount = 10,
                ActiveFrom = DateTime.Now.Date,
                ActiveUntil = new DateTime(2100, 1, 1),
                IsActive = true,
                AllowToSelect = false,
                VisitPrice = 1500,
            };
            tarifs.Add(tarif);
            //-----------------------------------
            #endregion
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.Tarifs.ToList<NsgDataItem>();
            return list;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name.ToString()))
                return Id.ToString();
            else
                return Name.ToString();
        }
    }
}

