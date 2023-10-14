using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;

namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    public class EnvironmentTypeConfiguration : IEntityTypeConfiguration<EnvironmentType>
    {
        public void Configure(EntityTypeBuilder<EnvironmentType> builder)
        {
            builder.HasMany(e => e.BearerTokenSettings)
                .WithOne(b => b.EnvironmentType)
                .HasForeignKey(e => e.EnvironmentTypeId);
        }
    }
}
