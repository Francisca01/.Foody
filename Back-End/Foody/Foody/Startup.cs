using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Foody.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Foody
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
            services.AddControllers();

            //services.AddSingleton(ISingleton, Singleton);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            /*
            app.Use(async (context, next) =>
            {
                var requestRouteValues = context.Request.RouteValues["controller"];
                //var lcfçhjçgklh = context.Request.RouteValues;

                if (requestRouteValues.Equals("Login") || requestRouteValues.Equals("Register"))
                {
                    await next.Invoke();
                }
                else
                {
                    var requestHeaders = context.Request.Headers;

                    string token = requestHeaders.ContainsKey("token") ?
                                    requestHeaders["token"].ToString() : "nada";

                    var isValid = TokenManager.ValidateToken(token);

                    if (isValid == null)
                    {
                        var response = context.Response;
                        //Not Valid
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        response.ContentType = "text/plain";
                        await response.WriteAsync($"Status Code: {response.StatusCode}");
                    }
                    else
                    {
                        await next.Invoke();
                    }
                }
            }
           );*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
