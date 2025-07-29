using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WPFServer.Data;
using WPFServer.Models;

namespace WPFServer.Context
{
    public class ApplicationContext : IdentityDbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ExercisesFiles> ExercisesFiles { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonsFiles> PersonsFiles { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public ApplicationContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(StaticData.CONNECTION_STRING);
        }

    }
}
