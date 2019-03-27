using Backend.Models.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend
{
    public class Startup
    {
        // Public Properties
        public IConfiguration Configuration { get; }

        // Constructors
        public Startup(IConfiguration configuration) => 
            Configuration = configuration;

        // Public Methods
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BadmintonClubDBDataContext>(options =>
            {
                string connectionString = Configuration
                    .GetConnectionString("MS_TableConnectionString");
                options.UseSqlServer(connectionString);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseFileServer();
        }
    }
}