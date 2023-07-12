using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace VoTanPhatVCBRDemo.EntityFrameworkCore
{
    public static class VoTanPhatVCBRDemoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<VoTanPhatVCBRDemoDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<VoTanPhatVCBRDemoDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
