using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class CarItemDb : NsgDataItem
    {
        public Guid Id { get; set; }
        [MaxLength(20)]
        public String Number { get; set; }
        [MaxLength(20)]
        public String CarType { get; set; }
        [MaxLength(20)]
        public String Team { get; set; }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.Cars.ToList<NsgDataItem>();
            return list;
        }
    }
}
