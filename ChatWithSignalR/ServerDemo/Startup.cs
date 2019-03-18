using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignalrServerDemo.Identity;
using SignalrServerDemo.SignalR;

namespace SignalrServerDemo {
    public class Startup {
        private readonly IConfiguration _configation;

        public Startup(IConfiguration configation)
        {
            _configation = configation;
        }

        public void ConfigureServices (IServiceCollection services) {
            
            // services.AddDbContext<ApplicationDbContext> (opt => {
            //     opt.UseInMemoryDatabase ("identitydb");
            // });
            // services.AddIdentity<ApplicationUser, IdentityRole> ()
            //     .AddEntityFrameworkStores<ApplicationDbContext> ();

            // services.AddAuthentication (opt => {
            //     opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //     opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // }).AddJwtBearer (opt => {
            //     opt.TokenValidationParameters = new TokenValidationParameters {
            //     ValidateAudience = true,
            //     ValidAudience = "me",
            //     ValidIssuer = "me",
            //     ValidateIssuer = true,
            //     ValidateLifetime = true,
            //     ValidateIssuerSigningKey = true,
            //     IssuerSigningKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes ("mysupersecret_secretkey!123"))
            //     };

            //     opt.Events = new JwtBearerEvents {
            //         OnMessageReceived = context => {
            //             var accessToken = context.Request.Query["access_token"];
            //             var path = context.HttpContext.Request.Path;
            //             if (!string.IsNullOrEmpty (accessToken) && (path.StartsWithSegments ("/hub"))) {
            //                 context.Token = accessToken;
            //             }

            //             return Task.CompletedTask;
            //         }
            //     };
            // });

            services.AddAuthentication(o => {
                o.DefaultAuthenticateScheme = AzureADB2CDefaults.JwtBearerAuthenticationScheme;
                o.DefaultChallengeScheme = AzureADB2CDefaults.BearerAuthenticationScheme;
                o.DefaultScheme = AzureADB2CDefaults.BearerAuthenticationScheme;
            }).AddAzureADB2CBearer(o => {
                _configation.Bind("AzureADB2C", o);
            });

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
            services.AddSignalR ();
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            app.UseCors (c => c.AllowAnyHeader ().AllowAnyMethod ().WithOrigins ("http://localhost:4200").AllowCredentials ());

            app.UseAuthentication ();
            app.UseSignalR (routes => {
                routes.MapHub<MyHub> ("/hub");
            });
            app.UseMvc ();
        }
    }
}