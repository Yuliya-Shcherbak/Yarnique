using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hellang.Middleware.ProblemDetails;
using Serilog;
using Serilog.Formatting.Compact;
using Yarnique.API.Configuration;
using Yarnique.API.Configuration.ExecutionContext;
using Yarnique.API.Configuration.Validation;
using Yarnique.API.Modules.Designs;
using Yarnique.API.Modules.OrderSubmitting;
using Yarnique.Common.Application;
using Yarnique.Common.Domain;
using Yarnique.Modules.Designs.Infrastructure.Configuration;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration;
using ILogger = Serilog.ILogger;

namespace Yarnique.API
{
    public class Startup
    {
        private const string YarniqueConnectionString = "YarniqueConnectionString";
        private static ILogger _logger;
        private static ILogger _loggerForApi;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment env)
        {
            ConfigureLogger();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables("Yarnique_")
                .Build();

            _loggerForApi.Information("Connection string:" + _configuration.GetConnectionString(YarniqueConnectionString));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerDocumentation();

            //services.ConfigureIdentityService();

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
            containerBuilder.RegisterModule(new DesignsAutofacModule());
            containerBuilder.RegisterModule(new OrderSubmittingAutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            var container = app.ApplicationServices.GetAutofacRoot();

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            InitializeModules(container);

            app.UseMiddleware<CorrelationMiddleware>();

            app.UseSwaggerDocumentation();

            //app.AddIdentityService();

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

            app.UseRouting();

            // app.UseAuthentication();
            //app.UseAuthorization();

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

            //var emailsConfiguration = new EmailsConfiguration(_configuration["EmailsConfiguration:FromEmail"]);

            DesignsStartup.Initialize(
                _configuration.GetConnectionString(YarniqueConnectionString),
                executionContextAccessor,
                _logger,
                null);

            OrderSubmittingStartup.Initialize(
               _configuration.GetConnectionString(YarniqueConnectionString),
               executionContextAccessor,
               _logger,
               null);
        }
    }
}