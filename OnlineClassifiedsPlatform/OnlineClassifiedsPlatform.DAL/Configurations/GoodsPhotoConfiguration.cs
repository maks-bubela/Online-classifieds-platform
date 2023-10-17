using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    class GoodsPhotoConfiguration : IEntityTypeConfiguration<GoodsPhoto>
    {
        public void Configure(EntityTypeBuilder<GoodsPhoto> builder)
        {
            builder.HasOne(x => x.Goods)
                .WithMany(x => x.GoodsPhotos)
                .HasForeignKey(x => x.GoodsImageId).IsRequired();

            builder.HasOne(x => x.GoodsImage)
                .WithOne(x => x.GoodsPhoto);
        }
    }
}
