using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class AlarmManagementStatus
    {
        public static int empty = 0;
        public static int searching = 10;
        public static int carMoving = 20;
        public static int carArrived = 30;
        public static int closed = 100;
        public static int canceled = 200;
        public static int error = -1;
    }
}
