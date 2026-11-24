using Asp.Versioning;
using Basket.Application.Mapper;
using System.Reflection;
using Basket.Application.Queries.GetBasket;
using Basket.Core.Repositories;
using Basket.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//Register AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProfileMapper>());

//Register Versioning
builder.Services.AddApiVersioning(Options => {
    Options.DefaultApiVersion = new ApiVersion(1, 0);
    Options.AssumeDefaultVersionWhenUnspecified = true;
    Options.ReportApiVersions = true;
});
//Mediator
var assemblies = new Assembly[] {
    Assembly.GetExecutingAssembly(),
    typeof(GetBasketByUserNameQueryHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//Add Swagger Services
builder.Services.AddEndpointsApiExplorer();//برای پشتیبانی از endpointها
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Basket.Api", Version = "v1", Description = "Basket Api" });
});

builder.Services.AddOpenApi();
//Redis Configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
//Build
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();//فعالسازی Swagger
    app.UseSwaggerUI();//فعالسازی رابط گرافیکی UI Swagger
}

app.UseAuthorization();
app.MapControllers();
app.Run();
