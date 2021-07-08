using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class ImageDb : NsgDataItem
    {
        public Guid Id { get; set; }
        public byte[] Image { get; set; }
        public DateTime LastChanges { get; set; }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.Images.ToList<NsgDataItem>();
            return list;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
