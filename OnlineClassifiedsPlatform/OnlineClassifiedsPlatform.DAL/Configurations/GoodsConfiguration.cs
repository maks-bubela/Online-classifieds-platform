using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;


namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    public class GoodsConfiguration : IEntityTypeConfiguration<Goods>
    {
        public void Configure(EntityTypeBuilder<Goods> builder)
        {
            builder.HasOne(u => u.GoodsCategory)
                .WithMany(r => r.Goods)
                .HasForeignKey(u => u.GoodsCategoryId).IsRequired();

            builder.HasMany(u => u.GoodsPhotos)
                .WithOne(r => r.Goods)
                .HasForeignKey(u => u.GoodsImageId).IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(r => r.Goods)
                .HasForeignKey(u => u.UserId).IsRequired();
        }
    }
}
