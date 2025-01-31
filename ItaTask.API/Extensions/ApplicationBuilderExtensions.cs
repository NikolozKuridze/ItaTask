using ItaTask.API.Middleware;
using ItaTask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItaTask.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            using (var scope = ((IApplicationBuilder)app).ApplicationServices.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<ItaTaskDbContext>();
                    context.Database.Migrate();
                    logger.LogInformation("Database migrated successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database");
                    throw;
                }
            }
        }


        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<LocalizationMiddleware>();

        // Configure Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Configure the HTTP request pipeline
        app.UseHttpsRedirection();

        // Configure CORS
        app.UseCors("AllowAll");

        app.UseStaticFiles(); // For image uploads

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}