using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WPFServer.Data;

namespace WPFServer.Context
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(StaticData.CONNECTION_STRING);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
