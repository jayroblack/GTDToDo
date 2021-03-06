using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using ScooterBear.GTD.Application;
using ScooterBear.GTD.AWS.DynamoDb;
using ScooterBear.GTD.AWS.DynamoDb.Core;
using ScooterBear.GTD.Patterns;

namespace ScooterBear.GTD
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver());

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "http://localhost:5000",
                        ValidateIssuer = true,
                        ValidAudience = "api1",
                        ValidateAudience = true
                        //ValidateIssuerSigningKey = true  //<-- Turn this on before going to production!!!!
                    };
                });

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddHttpContextAccessor();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<GtdAutofacModule>();
            builder.RegisterModule<ApplicationAutofacModule>();
            builder.RegisterModule<DynamoDbAutofacModule>();
            builder.RegisterModule<PatternsAutofacModule>();
            builder.RegisterType<DynamoDBLocalFactory>().As<IDynamoDBFactory>();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddDebug()
                    .AddConsole();
            });

            //ToDo: Add custom logic later to dynamically resolve at runtime.
            //TODO: If we are in lambda - then attach to cloud watch - otherwise attach to Console and Debug
            builder.RegisterInstance(loggerFactory).As<ILoggerFactory>();
            builder.Register(c => c.Resolve<ILoggerFactory>().CreateLogger<DynamoDb>()).As<ILogger<DynamoDb>>();
            builder.Register(c => c.Resolve<ILoggerFactory>().CreateLogger("")).As<ILogger>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCors("default");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}