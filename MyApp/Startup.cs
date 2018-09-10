using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data.Data;
using Data.Data.Repositories;
using Models.Entities;

namespace MyApp
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddDbContext<MyAppContext>(options =>
            {
                options.UseSqlServer(_config.GetConnectionString("MyAppConnection"),
                b => b.MigrationsAssembly("MyApp"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<MyAppContext>()
            .AddDefaultTokenProviders();
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "57893826620-rhsoqkl8n9l54v3l4j4oeblndbhjmsmr.apps.googleusercontent.com";
                googleOptions.ClientSecret = "I8QUZWrYEPM06sb__nj0K5MH";
            });
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "311458062951253";
                facebookOptions.AppSecret = "c7cbc058ae61e6065ef1b2d5b0295248";
            });
            
            services.AddTransient<DbSeeder>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Account", action = "Login" }
                    );
            });
            //comment out when migration
            //seeder.SeedDatabase().Wait();

        }
    }
}
