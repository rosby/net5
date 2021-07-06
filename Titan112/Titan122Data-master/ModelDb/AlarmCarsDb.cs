using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class AlarmCarsDb : NsgDataItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual AlarmItemDb Alarm { get; set; }
        public virtual CarItemDb Car { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int ArrivalTime { get; set; }
        public double Distance { get; set; }
        public bool IsFree { get; set; }
        public DateTime LastChanged { get; set; }
        //[Column(TypeName = "varchar(MAX)")]
        public String InitialRoute { get; set; }
        //[Column(TypeName = "varchar(MAX)")]
        public String Route { get; set; }
        public double Correction { get; set; }

        public AlarmCarsDb findById(TitanDBContext db)
        {
            return db.AlarmCars.Find(Id);
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.AlarmCars.ToList<NsgDataItem>();
            return list;
        }
    
    }
}
