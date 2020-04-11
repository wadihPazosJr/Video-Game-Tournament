using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VGT.Server
{
    public class Startup
    {
        private readonly string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(_myAllowSpecificOrigins,
                builder =>
                {
                    string[] clientSitesAllowed = Configuration.GetSection("ClientSitesAllowed").Get<List<string>>().ToArray();

                    //If a single entry in the appconfig (*) was used then assume that to be a wildcard and add any origin
                    if (clientSitesAllowed.Length == 1 && clientSitesAllowed[0] == "*")
                        builder.AllowAnyOrigin();
                    else
                        builder.WithOrigins(Configuration.GetSection("ClientSitesAllowed").Get<List<string>>().ToArray())
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            });
            services.AddSingleton<CosmosClient>(
                 new CosmosClientBuilder
                         (
                             accountEndpoint: Configuration.GetSection("CosmosDb")["AccountURL"],
                             authKeyOrResourceToken: Configuration.GetSection("CosmosDb")["Key"]
                         ).WithConnectionModeDirect()
                             .Build());
            services.AddSingleton<IConfiguration>(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(_myAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}