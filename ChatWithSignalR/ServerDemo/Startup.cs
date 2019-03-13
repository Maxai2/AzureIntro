using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ServerDemo.SignalR;

namespace ServerDemo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) {
            services.AddCors();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseCors(c => c.
                AllowAnyHeader().
                AllowAnyMethod().
                AllowCredentials().
                WithOrigins("http://localhost:4200"));

            app.UseSignalR(routes => {
                routes.MapHub<ChatHub> ("/chat");
            });
        }
    }
}
