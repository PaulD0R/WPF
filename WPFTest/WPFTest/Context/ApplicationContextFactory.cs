using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WPFTest.Data;

namespace WPFTest.Context
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(StaticData.EXERCISE_ROUDE);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
