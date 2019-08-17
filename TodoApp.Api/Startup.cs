using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TodoApp.Api.Infrastructure.Extensions;
using TodoApp.Api.Infrastructure.Filters;
using TodoApp.Api.Infrastructure.Middleware;
using TodoApp.Common;
using TodoApp.Common.Interface;
using TodoApp.DataAccess.Interface;
using TodoApp.DataAccess.Repositories;
using Couchbase.Extensions.DependencyInjection;

namespace TodoApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureSwagger();
            services.AddOptions();
            services.AddSingleton<ICryptoHelper, CryptoHelper>();
            services.ConfigureJwtAuthentication(Configuration);
            services.ConfigureAutoMapper();
            services.AddCouchbase(Configuration.GetSection("Couchbase"))
                .AddCouchbaseBucket<ITodoBucketProvider>("todoApp");
            services.AddScoped<IUserRepository, UserRepository>();


            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            var jsonOptions = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            JsonConvert.DefaultSettings = () => jsonOptions;

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                config.Filters.Add(typeof(ValidateModelStateAttribute));
            }).AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = jsonOptions.ReferenceLoopHandling;
                opt.SerializerSettings.ContractResolver = jsonOptions.ContractResolver;
                opt.SerializerSettings.Formatting = jsonOptions.Formatting;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler();

            app.UseCors("CorsPolicy");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMiddleware(typeof(RequestLoggerMiddleware));
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1"); });
            app.UseMvc();

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                app.ApplicationServices.GetRequiredService<ICouchbaseLifetimeService>().Close();
            });
        }
    }
}
