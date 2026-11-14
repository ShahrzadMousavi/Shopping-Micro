using Catalog.Application.Mapper;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Register AutoMapper
builder.Services.AddAutoMapper(cfg =>cfg.AddProfile<ProfileMapper>());
//Mediator
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
// خط بالا معادل تمامی commandها و handlerهای آنها از جمله همین مثال deleteCommand میباشد=== builder.Services.AddTransient<IRequestHandler<DeleteProductCommand, bool>, DeleteProductCommandHandler>();
//DI
builder.Services.AddScoped<ICatalogContext,CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();

//Add Swagger Services
builder.Services.AddEndpointsApiExplorer();//برای پشتیبانی از endpointها
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Catalog.Api", Version = "v1", Description = "Catalog Api" });
});//برای تولید سند Swagger
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
