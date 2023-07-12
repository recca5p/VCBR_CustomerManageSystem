using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using VoTanPhatVCBRDemo.Authorization.Roles;
using VoTanPhatVCBRDemo.Authorization.Users;
using VoTanPhatVCBRDemo.MultiTenancy;
using VoTanPhatVCBRDemo.Domain;
using System.Reflection;

namespace VoTanPhatVCBRDemo.EntityFrameworkCore
{
    public class VoTanPhatVCBRDemoDbContext : AbpZeroDbContext<Tenant, Role, User, VoTanPhatVCBRDemoDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public VoTanPhatVCBRDemoDbContext(DbContextOptions<VoTanPhatVCBRDemoDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
