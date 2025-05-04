
using GWPApi.Repositories;
using GWPApi.Services;
using Microsoft.OpenApi.Models;

namespace GWPApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GWP API", Version = "v1" });
            });

            // Register application services
            builder.Services.AddSingleton<IGwpDataRepository, GwpDataRepository>();
            builder.Services.AddScoped<IGwpCalculatorService, GwpCalculatorService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
