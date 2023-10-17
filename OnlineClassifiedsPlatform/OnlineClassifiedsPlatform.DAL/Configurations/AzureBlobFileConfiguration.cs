using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;

namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    public class AzureBlobFileConfiguration : IEntityTypeConfiguration<AzureBlobFile>
    {
        public void Configure(EntityTypeBuilder<AzureBlobFile> builder)
        {
            builder.HasOne(x => x.FileType)
                .WithMany(x => x.AzureBlobFiles)
                .HasForeignKey(x => x.BlobTypeId).IsRequired();

            builder.HasOne(x => x.Container)
                .WithMany(x => x.AzureBlobFiles)
                .HasForeignKey(x => x.ContainerId);

            builder.HasOne(x => x.ImageMetadata)
                .WithOne(x => x.Image).HasForeignKey<ImageMetadata>(p => p.ImageId);

            builder.HasOne(x => x.GoodsPhoto)
                 .WithOne(x => x.GoodsImage).HasForeignKey<GoodsPhoto>(p => p.GoodsImageId);

        }
    }
}
