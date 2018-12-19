using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OdeToFood
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, Greeter>();

            //Needs to be added to be able to use the MVC framework.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IGreeter greeter)
        {
            if (env.IsDevelopment())
            {
                //Middleware that catches a thrown exception an gives detailed information about the error.
                app.UseDeveloperExceptionPage();
            }

            //Makes files like index.html to be used as default files.
            //app.UseDefaultFiles();

            //This calls makes the static files under wwwroot available.
            app.UseStaticFiles();

            //Uses the default route for MVC
            //app.UseMvcWithDefaultRoute();

            //MVC without default route
            app.UseMvc(ConfigureRoutes);

            app.Run(async (context) =>
            {
                var greeting = greeter.GetMessageOfTheDay();
                //Adding mime type for the response to the browser.
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"Not found!!!");
            });
        }

        /// <summary>
        /// Conventional template routing
        /// </summary>
        /// <param name="routeBuilder"></param>
        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //The framework adds the word controller to the path e.g Home/index.html becomes HomeController/index.html

            //The default route
            //The ? sign means the parameter value is optional.
            // /Home/index/4
            //routeBuilder.MapRoute("Default", "{controller}/{action}/{id?}");
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
