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

            app.Use(async (context, next) =>
            {
                var aaa = context.Request.RouteValues["controller"];
                var lcfçhjçgklh = context.Request.RouteValues;
                if (context.Request.RouteValues["controller"].Equals("Login") || context.Request.RouteValues["controller"].Equals("Register"))
                {
                    await next.Invoke();
                }
                else
                {
                    string token = context.Request.Headers.ContainsKey("token") ?
                                    context.Request.Headers["token"].ToString() : "nada";


                    var isValid = TokenManager.ValidateToken(token);

                    if (isValid == null)
                    {
                        //Not Valid
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync($"Status Code: {context.Response.StatusCode}");
                    }
                    else
                    {
                        await next.Invoke();
                    }
                }
            }
           );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
