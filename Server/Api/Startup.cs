using Api.Interfaces;
using Api.Middleware;
using Api.Services;
using Api.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
                options.Phone = Configuration.GetSection("ReceiptChecker:Phone").Value;
                options.Password = Configuration.GetSection("ReceiptChecker:Password").Value;
            });
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<ISearchService, SearchService>();
            services.AddSingleton<IExternalService, ExternalService>();
            services.AddSingleton<ErrorLogService>();
            services.AddSingleton<ReceiptLogService>();
            services.AddDirectoryBrowser();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<MainMiddleware>();
            app.UseStaticFiles();
            app.UseDirectoryBrowser();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
