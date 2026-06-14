using Basket.Api.Data;
using Basket.Api.Models;
using BuildingBlocks.Behaviours;
using Carter;
using Marten;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddMarten(options =>
{
    options.Connection(
        builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, BasketCashingRepository>();

builder.Services.AddStackExchangeRedisCache(
    options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
    });
    
// Carter
builder.Services.AddCarter();

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddGrpcClient<Discount.Grpc.DiscountProtoService.DiscountProtoServiceClient>(
    options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

// Carter Endpoints
app.MapCarter();

app.Run();

