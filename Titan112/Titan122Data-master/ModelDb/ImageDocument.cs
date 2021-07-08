using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class ImageDocument : NsgDataItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual ImageDb Image { get; set; }
        public int ImageType { get; set; }
        public String Comment { get; set; }
        public DateTime LastChanges { get; set; }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.ImageDocuments.ToList<NsgDataItem>();
            return list;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
