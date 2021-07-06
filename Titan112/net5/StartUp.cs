using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan112Data;

namespace net5
{
    class StartUp
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<TitanDBContext>();

            createDbConnection(services);
        }

        private void createDbConnection(IServiceCollection services)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder("server=10.10.4.219;user=nsg-user;password=NSGtitan002!;database=Titan112;");

            services.AddDbContext<TitanDBContext>(options =>
                options.UseSqlServer(builder.ConnectionString));
        }
    }
}
