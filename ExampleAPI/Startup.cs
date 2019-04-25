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
using RDHATEOAS.LinkAdders;
using RDHATEOAS.Options;
using RDHATEOAS.Services;

using Microsoft.EntityFrameworkCore;

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

            var connection = @"Data Source=.\SQLEXPRESS;Initial Catalog=exampleapi;Integrated Security=True";
            services.AddDbContext<PeopleContext>(options => options.UseSqlServer(connection));

            //services.AddDbContext<PersonContext>(options => options.UseInMemoryDatabase("Person"));
            //services.AddEntityFrameworkInMemoryDatabase();

            // hateoas configuration
            var hateoasOptions = new HateoasOptions();
            hateoasOptions.linkAddersModel.Add(new LinkAdderModelDefault<Person>());
            services.AddSingleton(hateoasOptions);
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "HateoasRoute",
                    template: "api/{controller}/{id?}");
            });
        }
    }
}
