using ItaTask.API.Extensions;
using ItaTask.API.Filters;
using ItaTask.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
app.ConfigureApplication();

app.Run();