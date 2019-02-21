using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoConverter.Api.Data;
using VideoConverter.Api.Services;
using VideoConverter.Api.Utils;
using Microsoft.Extensions.Caching.Redis;

namespace VideoConverter.Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string GetSecretConnectionString() {
            var provider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(provider.KeyVaultTokenCallback));

            var secretUrl = Configuration.GetSection("Secrets")["DatabaseConnectionString"];
            var secret = keyVaultClient.GetSecretAsync(secretUrl).Result;

            return secret.Value;
        }

        public void ConfigureServices (IServiceCollection services) {
            // services.AddDbContext<TracksDbContext> (opts => {
            //     opts.UseSqlServer (Configuration
            //         .GetConnectionString ("AzureDbConnectionString"));
            // });

            services.AddDbContext<TracksDbContext> (opts => {
                opts.UseSqlServer(GetSecretConnectionString());
            });

            services.AddScoped<IAsyncRepository, EfAsyncRepository> ();
            services.AddScoped<ITracksService, TracksService> ();
            services.AddScoped<IYoutubeAudioExtractor, YoutubeAudioExtractor> ();
            services.AddScoped<IMediaStorageService, AzureMediaStorageService> ();

            services.AddDistributedRedisCache(opt => {
                opt.Configuration = "musicPlayer94.redis.cache.windows.net:6380,password=7FCqXtQ0U6q2yTS8xYgdAQ8U0kr87Y+g2Z9kIwPr2XI=,ssl=True,abortConnect=False";
                opt.InstanceName = "musicplayer";
            });

            services.Configure<AzureStorageAccountOptions> (Configuration.GetSection ("AzureStorageAccountOptions"));

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseHsts ();
            }
            
            app.UseCors (o => o.AllowAnyHeader ().AllowAnyMethod ().AllowAnyOrigin ());

            app.UseHttpsRedirection ();
            app.UseMvc ();
        }
    }
}