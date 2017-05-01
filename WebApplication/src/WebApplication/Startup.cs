using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace UIS
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = ConfigurationExtensions.GetConnectionString(this.Configuration, "DefaultConnection");
            services.AddMvc();
            services.AddDbContext<UISContext>(options => options.UseSqlServer(connectionString));
            services.AddOptions();
            services.Configure<UISConfig>(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, UISContext db)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseMiddleware<BasicAuthentication>();
            app.UseStaticFiles();
            app.Use(async (context, next) =>
            {
                if (!Path.HasExtension(context.Request.Path.Value) && !context.Request.Path.ToString().StartsWith("/api/") && context.Request.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                {
                    await context.Response.WriteAsync(File.ReadAllText("wwwroot\\index.html"));
                }

                await next();
            });
            app.UseMvc();
            //db.Database.EnsureCreated();
            //db.Database.Migrate();
        }
    }
}
