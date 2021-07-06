using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class PaymentRequest : NsgDataItem
    {
        public int Id { get; set; }
        public String RequestId { get; set; }
        public DateTime Date { get; set; }
        public virtual UserItemDb User { get; set; }
        public virtual TarifChangeRequestDb tarifRequest { get; set; }
        public double Sum { get; set; }
        public bool Processed { get; set; }
        public bool Successful { get; set; }
        public String Comment { get; set; }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.PaymentRequests.ToList<NsgDataItem>();
            return list;
        }
    }
}


