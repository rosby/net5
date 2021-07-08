using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class RegistrationRecord : NsgDataItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual UserItemDb User { get; set; }
        public int ActionType { get; set; }
        public double Sum { get; set; }
        public int Visits { get; set; }
        public int Months { get; set; }
        public String Comment { get; set; }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.RegistrationJournal.ToList<NsgDataItem>();
            return list;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Sum.ToString()))
                return Id.ToString();
            else 
                return Sum.ToString();
        }
    }
}
