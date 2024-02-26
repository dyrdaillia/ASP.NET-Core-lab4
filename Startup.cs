using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebApplication4
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapGet("/Library", async context =>
                {
                    await context.Response.WriteAsync("Welcome to the Library!");
                });

                endpoints.MapGet("/Library/Books", async context =>
                {
                    var booksList = await File.ReadAllTextAsync(Configuration["BooksFilePath"]);
                    await context.Response.WriteAsync(booksList);
                });

                endpoints.MapGet("/Library/Profile/{id?}", async context =>
                {
                    
                    var id = context.Request.RouteValues["id"] as string;

                    if (string.IsNullOrEmpty(id))
                    {
                        id = "current";
                    }

                    var userProfile = await File.ReadAllTextAsync($"{Configuration["ProfilesDirectoryPath"]}/{id}.json");
                    await context.Response.WriteAsync(userProfile);
                });
            });
        }
    }
}
