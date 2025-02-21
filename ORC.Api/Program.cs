using Microsoft.EntityFrameworkCore;
using ORC.Api.Data;
using Microsoft.OpenApi.Models;
using ORC.Api.Models;

namespace ORC.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ORC Battle API",
                    Version = "v1",
                    Description = "API for managing ORC Battle game"
                });
            });

            // Add Email Service
            builder.Services.AddScoped<IEmailService, EmailService>();

            // Add DbContext with SQL Server
            builder.Services.AddDbContext<OrcDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy => policy
                        .WithOrigins("http://localhost:5173") // Allow requests from the frontend
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Enable Swagger only in Development
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ORC Battle API V1");
                });
            }

            app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
            app.UseCors("AllowFrontend"); // Apply the CORS policy
            app.UseAuthorization(); // Enable Authorization middleware

            app.MapControllers(); // Map controller routes

            app.Run(); // Run the application
        }
    }
}
