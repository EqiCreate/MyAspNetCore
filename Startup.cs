using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using MyASP.Data;
using MyASP.Model;
using MyASP.Service;

namespace MyASP
{

    /*
    1 在构造函数中获取宿主传递的configura(CreateDefaultBuilder完成appsettings 初始化) 
    2 ConfigureServices 相当于services添加listdelegate管道进行注册对象然后由通过Configure构造函数进行注入对象，同理，startup也是相当于Configure，他可以注入之前宿主注入的对象
    3 实际框架在最终的Configure 代码块里面实现，比如addmvc(=>)并增加配置如 开始的地址是home
         
    */
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration iconfiguration)
        {
            _configuration = iconfiguration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var constr = _configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<DataContext>(options=>
            {
                options.UseSqlServer(constr);
            });
            services.AddSingleton<IWelcome, WelcomeSer>();//只添加单例
            //services.AddSingleton<ImodelEntity<Student>, RepostyStudents>();//添加内存数据
            services.AddScoped<ImodelEntity<Student>, EfcoreServiceRepority>();//添加sql数据.由于sql是线程不安全的，所以用scope

            //services.AddTransient();

            //TODO 添加identityuser
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(constr, mirgra =>
                {
                    mirgra.MigrationsAssembly("MyASP");
                }
                ));

            //注册 identity
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<IdentityDbContext>();

            services.Configure<IdentityOptions>(options=>
            {
                //password settings
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;

                //lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                //user setttings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.@+";
                options.User.RequireUniqueEmail = false;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IWelcome welcome, ILogger<Startup>logger)
        {
            logger.LogInformation("开始执行");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            #region 测试
            ////使用默认页
            //app.UseWelcomePage(new WelcomePageOptions()
            //{
            //    Path="/welcome",
            //});//阻塞型
            //app.UseFileServer();//default和staticfile
            ////洋葱中间件
            //app.Use(next=>
            //{
            //    return async context =>
            //    {
            //        if (context.Request.Path.StartsWithSegments("/first"))
            //        {
            //             await context.Response.WriteAsync("ok");
            //        }
            //        else
            //        {
            //            await next(context);
            //        }
            //    };
            //});
           

            #endregion

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath="/node_modules",
                FileProvider=new PhysicalFileProvider(Path.Combine(env.ContentRootPath,"node_modules"))
            });
            //app.UseMvcWithDefaultRoute();//默认路由mvc

            //使用注册身份认证
            app.UseAuthentication();

            //
            app.UseMvc(builder=>
            {
                builder.MapRoute("Default1","{controller=Home}/{action=Index}/{id?}");
            });
            ////默认输出
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync(welcome.GetMessage());
            //});
        }
    }
}
