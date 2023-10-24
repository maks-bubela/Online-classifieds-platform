using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.DAL.Entities;
using System.Collections.Generic;

namespace OnlineClassifiedsPlatform.DAL.Seeder
{
    public static class DatabaseSeeder
    {
        public static void SeedDataBase(ModelBuilder modelBuilder)
        {

            var defaultAzureStorageAccountDev = new AzureStorageAccount()
            {
                Id = 1,
                StorageAccount = "devstoreaccount1",
                AccountHost = "http://127.0.0.1:10000/"
            };
            IList<AzureBlobType> defaultAzureBlobImageTypes = new List<AzureBlobType>();

            modelBuilder.Entity<EnvironmentType>().HasData(
                    new EnvironmentType { Id = 1, Name = "staging" },
                    new EnvironmentType { Id = 2, Name = "development" },
                    new EnvironmentType { Id = 3, Name = "testing" },
                    new EnvironmentType { Id = 4, Name = "production" }
                );

            modelBuilder.Entity<BearerTokenSetting>().HasData(
                    new BearerTokenSetting { Id = 1, EnvironmentTypeId = 1, LifeTime = 30 },
                    new BearerTokenSetting { Id = 2, EnvironmentTypeId = 2, LifeTime = 30 },
                    new BearerTokenSetting { Id = 3, EnvironmentTypeId = 3, LifeTime = 1 },
                    new BearerTokenSetting { Id = 4, EnvironmentTypeId = 4, LifeTime = 7 }
                );

            modelBuilder.Entity<GoodsCategory>().HasData(
                    new GoodsCategory { Id = 1, CategoryName = "Clothes"},
                    new GoodsCategory { Id = 2, CategoryName = "Estate" },
                    new GoodsCategory { Id = 3, CategoryName = "Transport"},
                    new GoodsCategory { Id = 4, CategoryName = "Furniture"},
                    new GoodsCategory { Id = 5, CategoryName = "Electronics" }
                );

            modelBuilder.Entity<Role>().HasData(
                    new Role() { Id = 1, Name = "admin" },
                    new Role() { Id = 2, Name = "staff" },
                    new Role() { Id = 3, Name = "customer" }
                );

            defaultAzureBlobImageTypes.Add(new AzureBlobType() { Id = 1, BlobType = ".jpg" });
            defaultAzureBlobImageTypes.Add(new AzureBlobType() { Id = 2, BlobType = ".png" });
            defaultAzureBlobImageTypes.Add(new AzureBlobType() { Id = 3, BlobType = ".jpeg" });
            modelBuilder.Entity<AzureBlobType>().HasData(defaultAzureBlobImageTypes);
            modelBuilder.Entity<AzureStorageAccount>().HasData(defaultAzureStorageAccountDev);

            modelBuilder.Entity<AzureBlob>().HasData(new AzureBlob
            {
                Id = 1,
                ContainerName = "temp-image",
                AccountId = defaultAzureStorageAccountDev.Id
            });

            modelBuilder.Entity<AzureBlob>().HasData(new AzureBlob
            {
                Id = 2,
                ContainerName = "success-image",
                AccountId = defaultAzureStorageAccountDev.Id
            });
        }
    }
}
