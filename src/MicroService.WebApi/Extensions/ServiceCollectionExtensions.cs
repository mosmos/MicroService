﻿using System;
using System.IO;
using System.Reflection;
using MicroService.Common.Constants;
using MicroService.Common.Health;
using MicroService.Service.Configuration;
using MicroService.WebApi.Extensions.Constants;
using MicroService.WebApi.Extensions.Swagger;
using MicroService.WebApi.Services;
using MicroService.WebApi.Services.Cron;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroService.WebApi.Extensions
{
    /// <summary>
    ///     Service Collection Extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Display Configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public static void DisplayConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var config = configuration.Get<ApplicationOptions>();
            var shapeCronExpressionDescription = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(config.ShapeConfiguration.CronExpression);

            Console.WriteLine($"Environment: {environment?.EnvironmentName}");
            Console.WriteLine($"PostgreSql: {config.ConnectionStrings.PostgreSql}");
            Console.WriteLine($"ShapeRootDirectory Config: {config.ShapeConfiguration.ShapeRootDirectory}");
            Console.WriteLine($"ShapeRootDirectory: {Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), config.ShapeConfiguration.ShapeRootDirectory))}");
            Console.WriteLine($"Shape CronExpression: {config.ShapeConfiguration.CronExpression}");
            Console.WriteLine($"Shape Cron Description: {shapeCronExpressionDescription}");
        }

        /// <summary>
        ///     CORS Configuration
        /// </summary>
        /// <param name="services"></param>
        public static void AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                   builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

                options.AddPolicy(
                    ApiConstants.CorsPolicy,
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        /// <summary>
        /// Api Versioning
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            _ = services.AddApiVersioning(options => { options.ReportApiVersions = true; });

            _ = services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
        }

        /// <summary>
        ///     Swagger Configuration
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            // Swagger
            _ = services.AddSwaggerGen(
               options =>
               {
                   options.OperationFilter<SwaggerDefaultValues>();
                   options.IncludeXmlComments(GetXmlCommentsPath());
               });
        }

        /// <summary>
        ///   Custom Health Check.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.Get<ApplicationOptions>();

            var shapeDirectory = config.ShapeConfiguration.ShapeRootDirectory;
            string shapePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), shapeDirectory));

            services.AddHealthChecks()
                .AddCheck<VersionHealthCheck>("Version Health Check")
                .AddCheck<CronJobServiceHealthCheck>("Cron Job Health Check", tags: new[] { HealthCheckType.ReadinessCheck.ToString() })
                .AddFolderHealthCheck(shapePath, "Shape Root Directory")
                .AddNpgSql(config.ConnectionStrings.PostgreSql);

            return services;
        }

        /// <summary>
        ///  Custom Controllers
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomControllers(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.Get<ApplicationOptions>();

            services.AddControllers(setupAction => { }).ConfigureApiBehaviorOptions(setupAction =>
             {
               setupAction.InvalidModelStateResponseFactory = context =>
               {
                  var problemDetails = new ValidationProblemDetails(context.ModelState)
                  {
                     Type = "https://courselibrary.com/modelvalidationproblem",
                     Title = "One or more model validation errors occurred.",
                     Status = StatusCodes.Status422UnprocessableEntity,
                     Detail = "See the errors property for details.",
                     Instance = context.HttpContext.Request.Path,
                  };

                  problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                  return new UnprocessableEntityObjectResult(problemDetails)
                      {
                          ContentTypes = { "application/problem+json" },
                      };
                  };
            });

            return services;
        }

        /// <summary>
        /// Configure Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="provider"></param>
        public static void ConfigureSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"MicroService.WebApi - {description.GroupName.ToUpperInvariant()}");
                    }
                });
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T">CronJobService Type</typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options)
            where T : CronJobService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }

            var scheduleConfig = new ScheduleConfig<T>();
            options.Invoke(scheduleConfig);
            if (string.IsNullOrWhiteSpace(scheduleConfig.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<IScheduleConfig<T>>(scheduleConfig);
            services.AddHostedService<T>();

            return services;
        }

        private static string GetXmlCommentsPath()
        {
            var basePath = AppContext.BaseDirectory;
            var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
            var fileName = Path.GetFileName(assemblyName + ".xml");

            return Path.Combine(basePath, fileName);
        }
    }
}
