using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TemplateSolution.App;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using TemplateSolution.Repository;
using Autofac.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.FileProviders;
using Autofac;
using System.Reflection;
using TemplateSolution.Repository.Interface;

namespace TemplateSolution.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = " Template.WebApi",
                    Description = "by rex.song"
                });

                foreach (var name in Directory.GetFiles(AppContext.BaseDirectory, "*.XML",
                    SearchOption.AllDirectories))
                {
                    option.IncludeXmlComments(name);
                }

                //option.OperationFilter<GlobalHttpHeaderOperationFilter>(); // 添加httpHeader参数
            });
            services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));
            services.AddMemoryCache();
            services.AddCors();
            //在startup里面只能通过这种方式获取到appsettings里面的值，不能用IOptions😰
            var dbType = ((ConfigurationSection)Configuration.GetSection("AppSetting:DbType")).Value;
            if (dbType == AppConstant.DBTYPE_SQLSERVER)
            {
                services.AddDbContext<TemplateDBContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TemplateDBContext")));
            }
            else  //mysql
            {
                services.AddDbContext<TemplateDBContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("TemplateDBContext")));
            }

            services.AddHttpClient();

            //services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Configuration["DataProtection"]));


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //可以访问根目录下面的静态文件
            var staticfile = new StaticFileOptions { FileProvider = new PhysicalFileProvider(AppContext.BaseDirectory) };
            app.UseStaticFiles(staticfile);

            //todo:测试可以允许任意跨域，正式环境要加权限
            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                c.DocExpansion(DocExpansion.None);
                c.OAuthClientId("TemplateSolution.WebApi");  //oauth客户端名称
                c.OAuthAppName("客户端为TemplateSolution.WebApi"); // 描述
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //注册app层
            builder.RegisterAssemblyTypes(Assembly.Load("TemplateSolution.App"));
            
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            builder.RegisterType(typeof(UnitWork)).As(typeof(IUnitWork)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

        }
    }
}
