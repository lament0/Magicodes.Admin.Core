using System;
using System.Linq;
using Abp.AspNetCore;
using Abp.AspNetZeroCore.Web.Authentication.JwtBearer;
using Abp.Castle.Logging.Log4Net;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Hangfire;
using Abp.Timing;
using Castle.Facilities.Logging;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Magicodes.Admin.Authorization;
using Magicodes.Admin.Authorization.Roles;
using Magicodes.Admin.Authorization.Users;
using Magicodes.Admin.Configuration;
using Magicodes.Admin.EntityFrameworkCore;
using Magicodes.Admin.Identity;
using Magicodes.Admin.Install;
using Magicodes.Admin.MultiTenancy;
using Magicodes.Admin.Web.Authentication.JwtBearer;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Swashbuckle.AspNetCore.Swagger;
using Magicodes.Admin.Web.IdentityServer;
using System.IO;
using Abp.PlugIns;
using Magicodes.SwaggerUI;

namespace Magicodes.Admin.Web.Startup
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            _hostingEnvironment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(DefaultCorsPolicyName));
            });

            //Configure CORS for angular2 UI
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    builder
                        //.WithOrigins(_appConfiguration["App:CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray())
                        .AllowAnyOrigin() //TODO: Will be replaced by above when Microsoft releases microsoft.aspnetcore.cors 2.0 - https://github.com/aspnet/CORS/pull/94
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            IdentityRegistrar.Register(services);

            //����JwtBearer��֤
            AuthConfigurer.Configure(services, _appConfiguration);


            //����Զ���API�ĵ�����(֧���ĵ�����)
            services.AddCustomSwaggerGen(_appConfiguration, _hostingEnvironment);

            //����ABP�Լ����ģ������
            return services.AddAbp<AppHostModule>(options =>
            {
                //������־
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });
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
