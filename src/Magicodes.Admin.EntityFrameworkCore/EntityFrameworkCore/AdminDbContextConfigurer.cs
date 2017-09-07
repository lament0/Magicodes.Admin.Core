using Magicodes.Admin.Configuration;
using Magicodes.Admin.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Magicodes.Admin.EntityFrameworkCore
{
    public static class AdminDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AdminDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, p => p.UseRowNumberForPaging());
            //var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            ////��֧��SQL Server 2012�������ݿ�
            //if (configuration.GetValue<bool>("UseRowNumberForPaging"))
            //    builder.UseSqlServer(connectionString, p => p.UseRowNumberForPaging());
            //else
            //    builder.UseSqlServer(connectionString);
        }
    }
}