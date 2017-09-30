using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UISWeb.Data;
using UISWeb.Models;
using UISWeb.Services;

namespace UISWeb
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            string uiscontext = Configuration.GetConnectionString("UISDataConnection");
            services.AddDbContext<UISWebContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UISDataConnection")));

            services.Configure<UISConfig>(this.Configuration);
            
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            /*
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<UISWebContext>();
                dbContext.Database.EnsureCreated();
            }

            IdentityResult roleResult;
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            bool roleExist = false;
            roleExist = RoleManager.RoleExistsAsync("Admin").Result;
            if (!roleExist)
            {
                roleResult = RoleManager.CreateAsync(new IdentityRole("Admin")).Result;
            }
            roleExist = RoleManager.RoleExistsAsync("user").Result;
            if (!roleExist)
            {
                roleResult = RoleManager.CreateAsync(new IdentityRole("user")).Result;
            }
            */
            /*
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var _user = UserManager.FindByEmailAsync("james.malherbe@gmail.com").Result;
            if (_user != null)
            {
                UserManager.AddToRoleAsync(_user, "Admin");
            }
            */
        }
    }
}
