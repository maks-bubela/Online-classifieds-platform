using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;

namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    public class ImageMetadataConfiguration : IEntityTypeConfiguration<ImageMetadata>
    {
        public void Configure(EntityTypeBuilder<ImageMetadata> builder)
        {
            builder.HasOne(x => x.Image)
                .WithOne(x => x.ImageMetadata);
        }
    }
}
