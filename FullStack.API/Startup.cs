using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FullStack.API.Helpers;
using FullStack.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FullStack.Data;
using Microsoft.EntityFrameworkCore;
using FullStack.Data.Repositories;
using FullStack.Data.DbContexts;
using Microsoft.AspNetCore.Http;

namespace FullStack.API
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
            services.AddCors();
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters();


            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //TODO: Add the DbContext and repositoy

            services.AddDbContext<FullStackDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FullStackConnection")));
            services.AddScoped<IUserRepository, UserRepository>();


            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserValidator, UserValidator>();
            services.AddScoped<IUserMapper, UserMapper>();

            services.AddScoped<IAdvertService, AdvertService>();
            services.AddScoped<IAdvertValidator, AdvertValidator>();
            services.AddScoped<IAdvertMapper, AdvertMapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FullStackDbContext userDbContext)
        {
             // migrate any database changes on startup (includes initial db creation)
            userDbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<ApiExceptionHandlingMiddleware>();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
