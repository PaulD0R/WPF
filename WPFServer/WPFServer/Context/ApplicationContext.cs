using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
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
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public ApplicationContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {   
            const string ADMIN_ID = "B9C6AA99-E98D-4E3D-87DE-C93E17592919";
            const string USER_ID = "19EF95CD-413A-49EA-B4F1-448E9D86D81C";

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            List<IdentityRole> roles =
            [
                new()
                {   
                    Id = Guid.Parse(ADMIN_ID).ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },

                new()
                {
                    Id = Guid.Parse(USER_ID).ToString(),
                    Name = "User",
                    NormalizedName = "USER"
                }
            ];

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
