using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Titan112Data
{
    //[Index(nameof(Phone), IsUnique = true)]
    public class RegistrationRequestDb : NsgDataItem
    {
        public Guid Id { get; set; }

        [Required]
        public virtual UserItemDb User { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool AutomaticVerification { get; set; }
        public bool ClientServiceVerification { get; set; }
        public bool SecurityCheck { get; set; }
        public bool Processed { get; set; }
        public String Message { get; set; }
        public bool MessageForUser { get; set; }
        public int PassportType { get; set; }
        [MaxLength(5)]
        public string PassportSeries { get; set; }
        [MaxLength(20)]
        public string PassportNumber { get; set; }
        [MaxLength(100)]
        public string PassportIssueBy { get; set; }
        public DateTime PassportIssueDate { get; set; }
        [MaxLength(8)]
        public string PassportIssueByCode { get; set; }
        public DateTime PassportValidUntil { get; set; }
        public int DocumentType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CompanyPosition { get; set; }
        public string CompanyName { get; set; }
        public virtual UserItemDb MasterUser { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.RegistrationRequests.ToList<NsgDataItem>();
            return list;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(User.ToString()) & string.IsNullOrEmpty(Date.ToString()))
                return Id.ToString();
            else if (string.IsNullOrEmpty(Date.ToString()))
                return User.ToString();
            else
                return $"{User} ({Date})";
        }
    }
}
