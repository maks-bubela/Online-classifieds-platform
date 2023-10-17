using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineClassifiedsPlatform.DAL.Entities;


namespace OnlineClassifiedsPlatform.DAL.Configurations
{
    public class AzureStorageAccountConfiguration : IEntityTypeConfiguration<AzureStorageAccount>
    {
        public void Configure(EntityTypeBuilder<AzureStorageAccount> builder)
        {
            builder.HasMany(x => x.AzureBlobs)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId);
        }
    }
}
