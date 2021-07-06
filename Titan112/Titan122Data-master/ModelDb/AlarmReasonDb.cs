using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class AlarmCancelReasonDb : NsgDataItem 
    {
        public string Id { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        [MaxLength(255)]
        public bool EnableComment { get; set; }

        public static void InitialFill(DbSet<AlarmCancelReasonDb> alarmCancelReasons, TitanDBContext db)
        {
            if (alarmCancelReasons.Count<AlarmCancelReasonDb>() > 0) return;
            alarmCancelReasons.Add(new AlarmCancelReasonDb()
            {
                Id = Guid.NewGuid().ToString(),
                Description = "Случайное нажатие",
                EnableComment = false
            });
            alarmCancelReasons.Add(new AlarmCancelReasonDb()
            {
                Id = Guid.NewGuid().ToString(),
                Description = "Исчезла причина тревоги",
                EnableComment = false
            });
            alarmCancelReasons.Add(new AlarmCancelReasonDb()
            {
                Id = Guid.NewGuid().ToString(),
                Description = "Не поминайте лихом",
                EnableComment = false
            });
            alarmCancelReasons.Add(new AlarmCancelReasonDb()
            {
                Id = Guid.NewGuid().ToString(),
                Description = "Другое",
                EnableComment = true
            });
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.AlarmCancelReasons.ToList<NsgDataItem>();
            return list;
        }
    }
}
