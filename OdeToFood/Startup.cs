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
using Microsoft.Extensions.Logging;
using OdeToFood.Data;
using OdeToFood.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace OdeToFood
{
    /* 
    Class used for registering own services.
    Also use for configuring the HttRequest Pipeline.
     */
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(options => {
                _configuration.Bind("AzureAd", options);
            })
            .AddCookie();


            services.AddDbContext<OdeToFoodDBContext>(options => 
                options.UseSqlServer(_configuration.GetConnectionString("StorageDesktop"))
            );

            services.AddSingleton<IGreeter, Greeter>();

            //Creates a new instance per HttpRequest and throws it away when not needed anymore.
            //Typically the service type used for Data retrieval objects.
            //Ensures that it will be used by max 1 logical thread.
            services.AddScoped<IRestaurantData, SqlRestaurantData>();

            //Needs to be added to be able to use the MVC framework.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IGreeter greeter, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                //Middleware that catches a if an exception has been thrown from the middleware below and 
                //gives detailed information about the error.
                app.UseDeveloperExceptionPage();
            }

            //Middleware for always use SSL (https)
            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            //Testing own middleware
            app.Use(next => {
                //This function is called for each HttpRequest made.
                return async context => {
                    logger.LogInformation("Request incoming");
                    //If the path segment starts with /mym/ the this is the only thing  done before returning the HttpResponse.
                    if(context.Request.Path.StartsWithSegments("/mym")){
                        await context.Response.WriteAsync("Hit!!!!");
                        logger.LogInformation("Request handled");
                    }
                    //Next calls the next middleware in the pipeline
                    else{
                        await next(context);
                        logger.LogInformation("Request outgoing!!!");
                    }
                };
            });

            //Makes files like index.html to be used as default files.
            //app.UseDefaultFiles();

            //This access all the files in the server.
            //app.UseFileServer();

            //This calls makes the static files under wwwroot available.
            app.UseStaticFiles();

            app.UseAuthentication();

            //Uses the default route for MVC
            //app.UseMvcWithDefaultRoute();

            //MVC without default route
            app.UseMvc(ConfigureRoutes);

            //The app.Run() method take as a parameter a delegate of the type 
            //function<RequestDelegate, RequestDelegate>.
            //The type RequestDelegate is a function that returns a Task there for the async and await
            //must be used.
            app.Run(async (context) =>
            //This middleware gets executed if  no url matches with the middleware routes from app.UseMvc(ConfigureRoutes)
            {
                var greeting = greeter.GetMessageOfTheDay();
                //Adding mime type for the response to the browser.
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"Not found!!! {env.EnvironmentName}");
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
