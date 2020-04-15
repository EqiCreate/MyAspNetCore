using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Routine.Api.Services;
using Routine.Api.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;

namespace Routine.Api
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
            services.AddControllers(setup=>
            {
                setup.ReturnHttpNotAcceptable = true;//����accept ���ʹ����ʱ�򷵻�406
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
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<RoutineDbContext>(opt=> 
            {
                opt.UseSqlite("Data Source=routine.db");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
