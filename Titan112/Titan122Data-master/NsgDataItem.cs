using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class NsgDataItem
    {
        

        public virtual List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            throw new Exception();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
