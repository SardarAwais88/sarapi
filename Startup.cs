using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using sarapi.Configurations;
using sarapi.IRepository;
using sarapi.Models;
using sarapi.Repository;
using sarapi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sarapi
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
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnStr")));

            // we need to add  response cache 
            //services.AddResponseCaching();
            services.AddMemoryCache();

            services.ConfigureRateLimiting();
            services.AddHttpContextAccessor();

            services.ConfigureHttpCacheHeaders();

            // we need to add these service  to authentication and configure identity
            services.AddAuthentication();
            services.ConfigureIdentity();

            // now we call  jwt configure service and pass object Configuration
            services.ConfigureJWT(Configuration);
            
            // give accees to some who use our api
            services.AddCors(o => {

                o.AddPolicy("Allow ALL", builder =>
               builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
            });
          
            // we need to show that we use auto mapper in our app
            services.AddAutoMapper(typeof(MapperInitilizer));
            services.AddTransient<IUnitOfWork ,UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "sarapi", Version = "v1" });
            });
            services.AddControllers(config => {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                {
                    Duration = 120

                }) ;

            }).AddNewtonsoftJson(op =>
            op.SerializerSettings.ReferenceLoopHandling
            = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;
            // when we want to make some changes in any api there is no need for user to wait for the api 
            // we can make another controller to make some changes in our api
            services.ConfigureVersioning();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
             //   app.UseSwagger();
             //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "sarapi v1"));
            }
         
       //  env.IsProduction();
            app.UseSwagger();
            
            string virDir = Configuration.GetSection("VirtualDirectory").Value;
            app.UseSwaggerUI(c =>
            {
               // string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint(virDir ="/swagger/v1/swagger.json", "sarapi v1");
            });
            // we also use this 
            //change it when we want to deploy it on server
            //  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "sarapi v1"));
            //use this  when we want to deploy it on server
            /*           app.UseSwaggerUI(c =>
                 {
                     string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "sarapi v1"); 
             });*/

            // use this for exception handling
            app.ConfigureExceptionHandler();
            //app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseCors("Allow ALL");

            // we can add the middlware above  routing middleware 
            //which set up the user response cache 
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();
            app.UseIpRateLimiting();
    //        app.UseIpRateLimiting();   


            app.UseRouting();
           
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
