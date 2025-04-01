using webapi.ActionFilters;
using webapi.Dtos;
using webapi.Exceptions;
using webapi.Models;
using webapi.Persistence;
using webapi.Persistence.Repositories;
using webapi.Persistence.Repositories.Implementation;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace webapi.Extensions
{
    public static class ApplicationServiceExtensions
    {

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {



            services.AddDbContext<AppDbContext>(options =>
            {

                options.UseNpgsql(configuration.GetConnectionString("WebApiDatabaseConnectionString"))
                                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                //.LogTo(Console.WriteLine, LogLevel.Information)
                                .EnableSensitiveDataLogging()
                                .EnableDetailedErrors();
                //options.UseSqlite(configuration.GetConnectionString("WebApiDatabaseConnectionString"))
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                ////.LogTo(Console.WriteLine, LogLevel.Information)
                //.EnableSensitiveDataLogging()
                //.EnableDetailedErrors();
            });

            //add scoped used application repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // add to services the AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }


        public static void AddLog4net(this IServiceCollection services)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            services.AddSingleton(LogManager.GetLogger(typeof(Program)));
        }

        public static void AddValidationFilter(this IServiceCollection services)
        {
            // a 
            //services.AddScoped<ValidationFilterAttribute>();
        }


    }
}
