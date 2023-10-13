using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.DAL.Entities;
using OnlineClassifiedsPlatform.DAL.Seeder;
using System.Reflection;

namespace OnlineClassifiedsPlatform.DAL.Context
{
    public class OnlineClassifiedsPlatformContext : DbContext
    {
        public OnlineClassifiedsPlatformContext(string connectionString) : base(GetOptions(connectionString)) { }
        public OnlineClassifiedsPlatformContext(DbContextOptions<OnlineClassifiedsPlatformContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Role> Roles { get; set; }


        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            DatabaseSeeder.SeedDataBase(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
