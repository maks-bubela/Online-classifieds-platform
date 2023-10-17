using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    class GoodsCategoryConfiguration : IEntityTypeConfiguration<GoodsCategory>
    {
        public void Configure(EntityTypeBuilder<GoodsCategory> builder)
        {
            builder.HasMany(r => r.Goods)
                .WithOne(u => u.GoodsCategory)
                .HasForeignKey(u => u.GoodsCategoryId);
        }
    }
}