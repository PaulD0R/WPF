using Microsoft.EntityFrameworkCore;
using WPFTest.Data;
using WPFTest.MVVM.Model.Exercise;

namespace WPFTest.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<LightExercise> Exercises { get; set; }

        public ApplicationContext()
        {

        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(StaticData.EXERCISE_ROUDE);
        }
    }
}
