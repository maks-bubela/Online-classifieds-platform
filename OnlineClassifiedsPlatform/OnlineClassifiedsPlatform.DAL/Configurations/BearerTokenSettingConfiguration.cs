using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;

namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    public class BearerTokenSettingConfiguration : IEntityTypeConfiguration<BearerTokenSetting>
    {
        public void Configure(EntityTypeBuilder<BearerTokenSetting> builder)
        {
            builder.HasOne(b => b.EnvironmentType)
                .WithMany(e => e.BearerTokenSettings)
                .HasForeignKey(b => b.EnvironmentTypeId).IsRequired();
        }
    }
}
