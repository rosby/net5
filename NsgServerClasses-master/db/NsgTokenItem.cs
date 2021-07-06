using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NsgServerClasses;

namespace NsgServerClasses
{
    public class NsgTokenItem : INsgTokenExtension
    {
        [Key]
        public string Token { get; set; }
        public Guid UserId { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public DateTime LastAccessTime { get; set; }

        public NsgToken CreateNsgToken()
        {
            return new NsgToken(this);
        }

        public List<Claim> GetClaimsAndSetPrincipal(HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
