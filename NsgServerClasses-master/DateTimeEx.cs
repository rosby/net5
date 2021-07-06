using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NsgServerClasses
{
   /* public static class Ex
    {
        static DateTime T1970 = new DateTime(1970, 1, 1);
        public static double ToJS(this DateTime dIn)
        {
            var d = dIn.ToUniversalTime();
            return (d - T1970).Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static DateTime ToSrv(this double d)
        {
            return T1970.AddMilliseconds(d);
        }
    }*/

    public static class NsgService
    {
        //регулярное выражение, проверяющее корректность Guid
        private static Regex reGuid = new Regex(@"^[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}$", RegexOptions.Compiled);
        /// <summary>
        /// Безопасное (без exception) преобразование строки к Guid
        /// </summary>
        /// <param name="id">строка, содержащая представление Guid</param>
        /// <returns>Guid, полученный из строки. Если преобразование невозможно - пустой Guid</returns>
        public static Guid StringToGuid(string id)
        {
            if (id == null || id.Length != 36)
                return Guid.Empty;
            if (reGuid.IsMatch(id))
                return new Guid(id);
            else
                return Guid.Empty;
        }
    }
}
