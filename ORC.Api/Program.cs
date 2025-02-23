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

            #region Configure Services
            // Configure Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.AddEventLog();

            // Add services to the container
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            // Configure Swagger/OpenAPI with enhanced security
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

            // Add DbContext with SQL Server and enhanced configuration
            builder.Services.AddDbContext<OrcDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        sqlOptions.CommandTimeout(30);
                    });
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // Configure CORS with more secure settings
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy => policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            // Add response compression
            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });
            #endregion

            var app = builder.Build();

            // Ensure database exists and is up to date
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var context = services.GetRequiredService<OrcDbContext>();
                    context.Database.Migrate();
                    logger.LogInformation("Database migration completed successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database");
                }
            }

            #region Configure Pipeline
            // Configure Error Handling
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ORC Battle API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Configure Security Headers
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
                await next();
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            
            // Add compression before endpoints
            app.UseResponseCompression();
            
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
