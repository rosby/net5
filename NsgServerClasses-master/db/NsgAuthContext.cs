using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsgServerClasses
{
    public class NsgAuthContext : DbContext
    {
        public NsgAuthContext(DbContextOptions<NsgAuthContext> options)
            : base(options)
        {
        }

        public DbSet<NsgTokenItem> TokenItems { get; set; }
    }
}
