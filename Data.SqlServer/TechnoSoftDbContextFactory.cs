using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Data.SqlServer
{
    public class TechnoSoftDbContextFactory : IDesignTimeDbContextFactory<TechnoSoftDbContext>
    {
        public TechnoSoftDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.database.json")
                .Build();

            return CreateDbContext(configuration);
        }

        public TechnoSoftDbContext CreateDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<TechnoSoftDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TechnoSoftDbContext(optionsBuilder.Options);
        }
    }
}
