using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using azure_converter_project.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoConverter.Api.Data;
using VideoConverter.Api.Services;
using VideoConverter.Api.Utils;

namespace VideoConverter.Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
            _provider = new AzureServiceTokenProvider ();
            _keyVaultClient = new KeyVaultClient (new KeyVaultClient.AuthenticationCallback (_provider.KeyVaultTokenCallback));

        }

        AzureServiceTokenProvider _provider;
        KeyVaultClient _keyVaultClient;

        public IConfiguration Configuration { get; }

        private string GetSecretConnectionString () {
            var secretUrl = Configuration.GetSection ("SqlServer") ["DatabaseConnectionStringSecret"];
            var secret = _keyVaultClient.GetSecretAsync (secretUrl).Result;
            return secret.Value;
        }

        private CosmosDbOptions GetCosmosDbOptions () {
            var options = new CosmosDbOptions ();
            var section = Configuration.GetSection ("CosmosDb");
            var secretUrl = section["KeySecret"];
            var secret = _keyVaultClient.GetSecretAsync (secretUrl).Result;

            options.CollectionId = section["CollectionId"];
            options.DatabaseId = section["DatabaseId"];
            options.Endpoint = section["Endpoint"];
            options.Key = secret.Value;
            return options;
        }

        private RedisOptions GetRedisOptions () {
            var options = new RedisOptions ();
            var section = Configuration.GetSection ("Redis");
            var secretUrl = section["ConnectionStringSecret"];
            var secret = _keyVaultClient.GetSecretAsync (secretUrl).Result;

            options.ConnectionString = secret.Value;
            options.InstanceName = section["InstanceName"];
            return options;
        }

        private AzureStorageAccountOptions GetStorageAccountOptions () {
            var options = new AzureStorageAccountOptions ();
            var section = Configuration.GetSection ("AzureStorageAccount");
            var secretUrl = section["ConnectionStringSecret"];
            var secret = _keyVaultClient.GetSecretAsync (secretUrl).Result;

            options.ConnectionString = secret.Value;
            options.RootContainerName = section["RootContainerName"];
            return options;
        }

        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<TracksDbContext> (opts => {
                opts.UseSqlServer (GetSecretConnectionString ());
            });
            services.AddScoped<DbContext, TracksDbContext> ();
            services.AddScoped (typeof (IAsyncRepository<>), typeof (CosmosDbAsyncRepository<>));
            services.AddScoped<ITracksService, TracksService> ();
            services.AddScoped<IYoutubeAudioExtractor, YoutubeAudioExtractor> ();
            services.AddScoped<IMediaStorageService, AzureMediaStorageService> ();

            services.AddDistributedRedisCache (opt => {
                var options = GetRedisOptions ();
                opt.Configuration = options.ConnectionString;
                opt.InstanceName = options.InstanceName;
            });

            services.AddSingleton<AzureStorageAccountOptions> (GetStorageAccountOptions ());
            services.AddSingleton<CosmosDbOptions> (GetCosmosDbOptions ());

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);
            services.AddSignalR ();
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseHsts ();
            }

            app.UseCors (o => o.AllowAnyHeader ().AllowAnyMethod ().WithOrigins ("http://localhost:4200").AllowCredentials());

            app.UseSignalR (routes => {
                routes.MapHub<CustomHub> ("/hub");
            });
            app.UseMvc ();
        }
    }
}