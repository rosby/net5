using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Titan112Data
{
    public class InformationDb : NsgDataItem
    {
        public int Id { get; set; }
        public int InformationType { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }

        public static void InitialFill(DbSet<InformationDb> information, TitanDBContext db)
        {
            if (information.Count<InformationDb>() > 0) return;
            information.Add(new InformationDb()
            {
                Order = 1,
                InformationType = InformationTypes.LicenceInfo,
                Text = "1.1. ООО «ЯНДЕКС» (далее — «Яндекс») предлагает пользователю сети Интернет (далее — Пользователь) — использовать сервис Яндекс.Карты, доступный по адресу: http://maps.yandex.ru (далее — «Сервис»)."
            });
            information.Add(new InformationDb()
            {
                Order = 2,
                InformationType = InformationTypes.LicenceInfo,
                Text = "1.2. Настоящие Условия представляют собой дополнение к Пользовательскому соглашению сервисов Яндекса относительно порядка использования Сервиса. Во всем, что не предусмотрено настоящими Условиями, отношения между Яндексом и Пользователем в связи с использованием Сервиса регулируются Пользовательским соглашением сервисов Яндекса (https://yandex.ru/legal/rules), а также Лицензией на использование поисковой системы Яндекса (https://yandex.ru/legal/termsofuse), Политикой конфиденциальности (https://yandex.ru/legal/confidential)."
            });
            information.Add(new InformationDb()
            {
                Order = 3,
                InformationType = InformationTypes.LicenceInfo,
                Text = "1.3. Начиная использовать Сервис/его отдельные функции, Пользователь считается принявшим настоящие Условия, а также условия всех указанных выше документов, в полном объеме, без всяких оговорок и исключений. В случае несогласия Пользователя с какими-либо из положений указанных документов, Пользователь не вправе использовать Сервис."
            });
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list =db.Information.ToList<NsgDataItem>();
            return list;
        }
    }
}
