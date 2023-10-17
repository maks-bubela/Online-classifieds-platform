using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;

namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    public class AzureBlobTypeConfiguration : IEntityTypeConfiguration<AzureBlobType>
    {
        public void Configure(EntityTypeBuilder<AzureBlobType> builder)
        {
            builder.HasMany(x => x.AzureBlobs)
                .WithMany(x => x.AllowAzureBlobTypes);

            builder.HasMany(x => x.AzureBlobFiles)
                .WithOne(x => x.FileType)
                .HasForeignKey(x => x.BlobTypeId);
        }
    }
}
