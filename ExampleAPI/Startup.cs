using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RDHATEOAS.LinkAdders;
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


            //services.AddScoped<IActionContextAccessor, ActionContextAccessor>();
            //services.AddScoped<IUrlHelper>(x =>
            //{
            //    var actionContext = x.GetService<IActionContextAccessor>().ActionContext;
            //    return new UrlHelper(actionContext);
            //});



            //// boilerplate in order to initialise the UrlHelper with the correct context
            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //services.AddScoped<IUrlHelper>(x =>
            //{
            //    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            //    var factory = x.GetRequiredService<IUrlHelperFactory>();
            //    return factory.GetUrlHelper(actionContext);
            //});



            // hateoas configuration
            var hateoasOptions = new HateoasOptions();
            hateoasOptions.linkAddersModel.Add(new LinkAdderModelDefault<Person>());
            services.AddSingleton(hateoasOptions);


            //services.AddScoped<ILinkService, LinkService>();
            //services.Configure<HATEOASLinksOptions>(Configuration);
            //services.AddOptions<HATEOASLinksOptions>()
            //    .Bind(Configuration.GetSection("HATEOASLinksOptions"))
            //    .ValidateDataAnnotations();
            // add here
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
                    name: "HateoasRoute",
                    template: "api/{controller=Values}/{id?}");
            }); // urlhelper
        }
    }
}
