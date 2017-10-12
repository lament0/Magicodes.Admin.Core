using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Magicodes.Admin.EntityFrameworkCore
{
    public static class AdminDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AdminDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
            //var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            ////��֧��SQL Server 2012�������ݿ�
            //if (configuration.GetValue<bool>("UseRowNumberForPaging"))
            //    builder.UseSqlServer(connectionString, p => p.UseRowNumberForPaging());
            //else
            //    builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AdminDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}