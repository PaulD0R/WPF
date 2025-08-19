using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPFServer.Models;

namespace WPFServer.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Subject)
                .WithMany(s => s.Exercises)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Persons)
                .WithMany(p => p.Exercises);
        }
    }
}
