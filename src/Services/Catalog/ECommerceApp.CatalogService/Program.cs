using Asp.Versioning;
using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Application.Services.Concrete;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.Domain.Interfaces;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Infrastructure.Context;
using ECommerceApp.Infrastructure.Redis;
using ECommerceApp.Infrastructure.Repositories;
using MongoDB.Bson;
using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var mongoConnectionString = $"{builder.Configuration["MongoConnectionString:Connection"]}/{builder.Configuration["MongoConnectionString:DatabaseName"]}";
var seqServer = $"{builder.Configuration["Seq:ServerURL"]}";
Logger log = new LoggerConfiguration()
.WriteTo.MongoDB(mongoConnectionString, "Logs")
.WriteTo.Seq(seqServer)
.Enrich.FromLogContext()
.MinimumLevel.Information()
.CreateLogger();
builder.Host.UseSerilog(log);


builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddScoped<IRepository<Category, ObjectId>, BaseRepository<Category>>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddApiVersioning(options =>
   {
       options.DefaultApiVersion = new ApiVersion(1, 0);
       options.AssumeDefaultVersionWhenUnspecified = true;
       options.ReportApiVersions = true;
       options.ApiVersionReader = new UrlSegmentApiVersionReader();
   });


builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["Redis:ConnectionString"];
        options.InstanceName = builder.Configuration["Redis:InstanceName"];
    });



builder.Services.AddScoped<IRedisContext, RedisContext>();

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
