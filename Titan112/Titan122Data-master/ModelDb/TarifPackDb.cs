using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class TarifPackDb : NsgDataItem
    {
        public Guid Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        public virtual TarifItemDb Tarif { get; set; }
        public bool DefaultPack { get; set; }
        public bool RecommendedPack { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveUntil { get; set; }
        public bool IsActive { get; set; }
        public bool AllowToSelect { get; set; }
        public int MonthToBuy { get; set; }
        public int VisitsToBuy { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        static public TarifPackDb FindById(TitanDBContext db, Guid id)
        {
            return db.TarifPacks.Find(id);
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.TarifPacks.ToList<NsgDataItem>();
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
