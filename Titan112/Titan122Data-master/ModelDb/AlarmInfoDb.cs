using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class AlarmInfoDb : NsgDataItem
    {
        public int Guid { get; set; }
        public virtual AlarmItemDb Alarm { get; set; }

    }
}
