using APIAccessProDependencies.Helpers.ActionFilters;
using APIAccessProDependencies.Helpers.Common;
using APIAccessProDependencies.Helpers.ConfigurationSettings;
using APIAccessProDependencies.Helpers.Logger;
using APIAccessProDependencies.Interfaces;
using APIAccessProDependencies.Repositories;
using APIAccessProDependencies.Services;
using MerchantTransactionCore.Helpers.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace APIAccessPro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigurationSettingsHelper.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });

            services.AddControllers(opt =>
            {
                opt.Filters.Add(typeof(ValidationActionFilter));
                opt.ReturnHttpNotAcceptable = false;
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Error;
            })
            .ConfigureApiBehaviorOptions(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            MyHttpContextAccessor.HttpContextAccessor = httpContextAccessor;

            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();
            services.AddCosmosDBServices();
            services.AddScoped<IInputValidation, InputValidation>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Capital Placement Collections", Version = "v1", Description = "Manage your Programs using Capital Placement Robust API" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        // Configure HTTP request Pipeline on runtime
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IHttpContextAccessor httpContextAccessor)
        {
            MyHttpContextAccessor.HttpContextAccessor = httpContextAccessor;
            LogWriter.Logger = logger;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MBCollectionGENERICAPI v1"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CORSPolicy");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "CapitalPlacement_Api v1"));

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
