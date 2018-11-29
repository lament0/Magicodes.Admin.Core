using System;
using System.IO;
using System.Linq;
using Abp.AspNetCore;
using Abp.AspNetZeroCore.Web.Authentication.JwtBearer;
using Abp.Castle.Logging.Log4Net;
using Abp.Castle.Logging.NLog;
using Abp.Extensions;
using Castle.Facilities.Logging;
using Magicodes.Admin.Configuration;
using Magicodes.Admin.Identity;
using Magicodes.SwaggerUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Host.Startup
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        public Startup(IHostingEnvironment env, ILogger<Startup> logger)
        {
            _appConfiguration = env.GetAppConfiguration();
            _hostingEnvironment = env;
            _logger = logger;
            _logger.LogInformation($"EnvironmentName:{env.EnvironmentName}");
            _logger.LogInformation($"ContentRootPath:{env.ContentRootPath}");
            _logger.LogInformation($"WebRootPath:{env.WebRootPath}");
            _logger.LogInformation($"CurrentDirectory:{Directory.GetCurrentDirectory()}");
            _logger.LogWarning($"ConnectionString:{_appConfiguration["ConnectionStrings:Default"]}");
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(DefaultCorsPolicyName));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1); ;


            
            //Configure CORS
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            IdentityRegistrar.Register(services);

            //����JwtBearer��֤
            AuthConfigurer.Configure(services, _appConfiguration);


            //����Զ���API�ĵ�����(֧���ĵ�����)
            services.AddCustomSwaggerGen(_appConfiguration, _hostingEnvironment);

            try
            {
                _logger.LogWarning("abp  Begin...");
                //����ABP�Լ����ģ������
                return services.AddAbp<AppHostModule>(options =>
                {
                    //������־
                    options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                        f =>
                        {
                            var logType = _appConfiguration["Abp:LogType"];
                            _logger.LogInformation($"LogType:{logType}");
                            if (logType != null && logType == "NLog")
                            {
                                f.UseAbpNLog().WithConfig("nlog.config");
                            }
                            else
                            {
                                f.UseAbpLog4Net().WithConfig("log4net.config");
                            }
                        });

                    //Ĭ�ϲ��������Ŀ¼�����Ƽ����ģʽ��
                    //options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "Plugins"), SearchOption.AllDirectories);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("����Abp���ִ���", ex);
                return null;
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //��ʼ�� ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false; //used below: UseAbpRequestLocalization
            });

            //���ÿ�����Դ����
            app.UseCors(DefaultCorsPolicyName);

            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            app.UseStaticFiles();

            //if (DatabaseCheckHelper.Exist(_appConfiguration["ConnectionStrings:Default"]))
            //{
            //    app.UseAbpRequestLocalization();
            //}

            //�����Զ���API�ĵ�(֧���ĵ�����)
            app.UseCustomSwaggerUI(_appConfiguration);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });

            
        }
    }
}
