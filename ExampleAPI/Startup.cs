using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RDHATEOAS.Middleware;
using RDHATEOAS.Options;
using RDHATEOAS.Services;

namespace ExampleAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);    // otherwise UrlHelper crashes https://github.com/aspnet/AspNetCore/issues/4418
            services.AddDbContext<PersonContext>(options => options.UseInMemoryDatabase("Person"));
            services.AddEntityFrameworkInMemoryDatabase();
            //services.AddScoped<ILinkService, LinkService>();
            //services.Configure<HATEOASLinksOptions>(Configuration);
            //services.AddOptions<HATEOASLinksOptions>()
            //    .Bind(Configuration.GetSection("HATEOASLinksOptions"))
            //    .ValidateDataAnnotations();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseHttpsRedirection();
            //app.UseMiddleware<HATEOASSupportMiddleware>();    // not applicable 
            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Testroute",
                    template: "api/{controller=Values}/{id?}");
            }); // urlhelper
        }
    }
}
