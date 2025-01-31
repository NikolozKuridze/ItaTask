using FluentValidation;
using ItaTask.API.Filters;
using ItaTask.Application.Common.Behaviors;
using ItaTask.Domain.Interfaces;
using ItaTask.Domain.Interfaces.Repositories;
using ItaTask.Domain.Interfaces.Services;
using ItaTask.Infrastructure.Persistence;
using ItaTask.Infrastructure.Persistence.Repositories;
using ItaTask.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ItaTask.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options => { options.Filters.Add<ValidationFilter>(); });
        services.AddScoped<ValidationFilter>();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerServices();

        // Register Other Services
        services.AddScoped<IFileService, FileService>();

        // Configure CORS if needed
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(Application.Features.Persons.Commands.CreatePerson.CreatePersonCommand).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);


        return services;
    }

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ItaTaskDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ItaTaskDbContext).Assembly.FullName)));

        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ICityRepository, CityRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFileService, FileService>();

        return services;
    }

    private static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Person Directory API",
                Version = "v1",
                Description = "API for managing person directory"
            });
        });
    }
}