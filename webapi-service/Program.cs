using webapi.ActionFilters;
using webapi.Converters;
using webapi.Exceptions;
using webapi.Extensions;
using log4net;
using Microsoft.AspNetCore.Mvc;



public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        //builder.Services.AddCors(options =>
        //{
        //    options.AddDefaultPolicy(
        //                          policy =>
        //                          {
        //                              policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
        //                              .AllowAnyHeader()
        //                              .AllowAnyMethod();
        //                          });
        //});

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddPersistenceServices(builder.Configuration);
        builder.Services.AddApplicationServices();

        //builder.Services.AddDbContext<AppDbContext>(options =>options);
        builder.Services.AddControllers(opt =>
        {
            opt.Filters.Add<HttpResponseExceptionFilter>();
        });

        //json serializer options configuration
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });

        builder.Services.AddValidationFilter();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Logging
        builder.Services.AddLog4net();

        var app = builder.Build();

        //PrepDb(app);

        //var logger = app.Services.GetRequiredService<ILog>().Logger;

        //configure the global error handling middleware
        app.ConfigureExceptionHandler();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors();
        }


        app.UseHttpsRedirection();

        //app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}