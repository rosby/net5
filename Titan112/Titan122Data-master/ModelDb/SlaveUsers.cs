using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class SlaveUser : NsgDataItem
    {
        public int Id { get; set; }
        public virtual UserItemDb Master { get; set; }
        public virtual UserItemDb Slave { get; set; }
        public virtual DateTime ActivationDate { get; set; }
        public virtual bool IsActive { get; set; }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.SlaveUsers.ToList<NsgDataItem>();
            return list;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Slave.ToString()))
                return Id.ToString();
            else
                return Slave.ToString();
        }
    }
}
