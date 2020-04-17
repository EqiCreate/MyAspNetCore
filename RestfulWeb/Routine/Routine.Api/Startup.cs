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
                setup.ReturnHttpNotAcceptable = true;//����accept ���ʹ����ʱ�򷵻�406
                setup.CacheProfiles.Add("120sCacheProfile",new CacheProfile() { Duration=120});
            })
                .ConfigureApiBehaviorOptions(setup=>//���÷���ʵ������422 �ɿ���ʹ��fluentvalidation ���е���������֤
                {
                    setup.InvalidModelStateResponseFactory = context => 
                    {
                        var problemdetails = new ValidationProblemDetails(context.ModelState) { Type = "http://www.baidu.com", Title = "occured error", Status = StatusCodes.Status422UnprocessableEntity, Detail = "�뿴��ϸ��Ϣ", Instance = context.HttpContext.Request.Path };
                        problemdetails.Extensions.Add("traceId",context.HttpContext.TraceIdentifier);
                        return new UnprocessableEntityObjectResult(problemdetails) { ContentTypes = { "application/problem+json" } };
                    };
                })
                .AddNewtonsoftJson(setup=> { setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); })
                .AddXmlDataContractSerializerFormatters()//���ø����֧��xml��ʽ��content type
                ;
            services.Configure<MvcOptions>(config=>
            {
                var newtonjsonOutputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                #region ע��ȫ��content-type
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
            //app.UseHttpCacheHeaders();//��̫���� �ʺϲ���

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
