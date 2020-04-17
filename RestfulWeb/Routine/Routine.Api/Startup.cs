using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Routine.Api.Services;
using Routine.Api.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Routine.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpCacheHeaders(expires =>
            {
                expires.MaxAge = 60;
                expires.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
            }, validation => {
                validation.MustRevalidate = true;
            });
            services.AddResponseCaching();
            services.AddControllers(setup=>
            {
                setup.ReturnHttpNotAcceptable = true;//设置accept 类型错误的时候返回406
                setup.CacheProfiles.Add("120sCacheProfile",new CacheProfile() { Duration=120});
            })
                .ConfigureApiBehaviorOptions(setup=>//设置返回实体问题422 可考虑使用fluentvalidation 进行第三方库验证
                {
                    setup.InvalidModelStateResponseFactory = context => 
                    {
                        var problemdetails = new ValidationProblemDetails(context.ModelState) { Type = "http://www.baidu.com", Title = "occured error", Status = StatusCodes.Status422UnprocessableEntity, Detail = "请看详细信息", Instance = context.HttpContext.Request.Path };
                        problemdetails.Extensions.Add("traceId",context.HttpContext.TraceIdentifier);
                        return new UnprocessableEntityObjectResult(problemdetails) { ContentTypes = { "application/problem+json" } };
                    };
                })
                .AddNewtonsoftJson(setup=> { setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); })
                .AddXmlDataContractSerializerFormatters()//设置更多的支持xml格式的content type
                ;
            services.Configure<MvcOptions>(config=>
            {
                var newtonjsonOutputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                #region 注册全局content-type
                //if (newtonjsonOutputFormatter != null)
                //    newtonjsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.company.hateoas+json");
                #endregion
            });
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<RoutineDbContext>(opt=> 
            {
                opt.UseSqlite("Data Source=routine.db");
            });
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<IPropertyCheckServices, PropertyCheckServices>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseResponseCaching();
            //app.UseHttpCacheHeaders();//不太好用 适合测试

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
