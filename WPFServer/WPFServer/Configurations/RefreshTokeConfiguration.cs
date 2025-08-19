using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPFServer.Models;

namespace WPFServer.Configurations
{
    public class RefreshTokeConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Token).IsUnique();

            builder.HasOne(x => x.Person).WithMany().HasForeignKey(x => x.PersonId);
        }
    }
}
