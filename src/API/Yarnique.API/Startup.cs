using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Compact;
using System.Text;
using Yarnique.API.Configuration;
using Yarnique.API.Configuration.ExecutionContext;
using Yarnique.API.Configuration.Validation;
using Yarnique.API.Modules.Designs;
using Yarnique.API.Modules.EventsBus;
using Yarnique.API.Modules.OrderSubmitting;
using Yarnique.Common.Application;
using Yarnique.Common.Domain;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Modules.Designs.Infrastructure.Configuration;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration;
using Yarnique.Modules.UsersManagement.Infrastructure.Configuration;
using ILogger = Serilog.ILogger;

namespace Yarnique.API
{
    public class Startup
    {
        private static ILogger _logger;
        private static ILogger _loggerForApi;
        private readonly IConfiguration _configuration;
        private readonly YarniqueConfig _config;

        public Startup(IWebHostEnvironment env)
        {
            ConfigureLogger();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables("Yarnique_")
                .Build();

            _config = BindApplicationConfig();

            _loggerForApi.Information("Connection string:" + _config.ConnectionStrings.YarniqueConnectionString);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthentication(services, _config);


            services.AddControllers();

            services.AddSwaggerDocumentation();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new EventsBusAutofacModule(_config.Rabbitmq, _logger));
            containerBuilder.RegisterModule(new DesignsAutofacModule());
            containerBuilder.RegisterModule(new OrderSubmittingAutofacModule());
            containerBuilder.RegisterModule(new UsersManagementAutofacModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            var container = app.ApplicationServices.GetAutofacRoot();

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            InitializeModules(container);

            app.UseMiddleware<CorrelationMiddleware>();

            app.UseSwaggerDocumentation();

            if (env.IsDevelopment())
            {
                app.UseProblemDetails();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void ConfigureLogger()
        {
            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();

            _loggerForApi = _logger.ForContext("Module", "API");

            _loggerForApi.Information("Logger configured");
        }

        private void InitializeModules(ILifetimeScope container)
        {
            var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

            var eventsBus = container.Resolve<IEventsBus>();

            DesignsStartup.Initialize(
                _config.ConnectionStrings.YarniqueConnectionString,
                executionContextAccessor,
                _logger,
                eventsBus);

            OrderSubmittingStartup.Initialize(
               _config.ConnectionStrings.YarniqueConnectionString,
               _config.PaymentUrl,
               executionContextAccessor,
               _logger,
               eventsBus);

            UsersManagementStartup.Initialize(
               _config.ConnectionStrings.YarniqueConnectionString,
               _config.Identity,
               executionContextAccessor,
               _logger,
               null);
        }

        private YarniqueConfig BindApplicationConfig()
        {
            YarniqueConfig config = new YarniqueConfig();
            _configuration.Bind(config);
            return config;
        }

        private void ConfigureAuthentication(IServiceCollection services, YarniqueConfig config)
        {
            var key = Encoding.ASCII.GetBytes(config.Identity.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenValidationException))
                            context.Response.Headers.Append("Token-Expired", "true");
                        return Task.CompletedTask;
                    }
                };
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = config.Identity.JwtIssuer,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                };
            });
        }
    }
}