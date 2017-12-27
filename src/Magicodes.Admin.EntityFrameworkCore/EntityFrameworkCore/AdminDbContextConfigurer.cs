using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Magicodes.Admin.EntityFrameworkCore
{
    public static class AdminDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AdminDbContext> builder, string connectionString)
        {
            //builder.UseSqlServer(connectionString);
            //��֧��SQL Server 2012�������ݿ�
            builder.UseSqlServer(connectionString, p => p.UseRowNumberForPaging());
        }

        public static void Configure(DbContextOptionsBuilder<AdminDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}