using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;

namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    public class AzureBlobConfiguration : IEntityTypeConfiguration<AzureBlob>
    {
        public void Configure(EntityTypeBuilder<AzureBlob> builder)
        {
            builder.HasMany(x => x.AllowAzureBlobTypes)
                .WithMany(x => x.AzureBlobs);

            builder.HasOne(x => x.Account)
                .WithMany(x => x.AzureBlobs)
                .HasForeignKey(x => x.AccountId).IsRequired();

            builder.HasMany(x => x.AzureBlobFiles)
                .WithOne(x => x.Container)
                .HasForeignKey(x => x.ContainerId);
        }
    }
}
