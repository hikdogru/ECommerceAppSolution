using ECommerceApp.Application.Services.Abstract;
using ECommerceApp.Application.Services.Concrete;
using ECommerceApp.Core.Domain.Entities.Language;
using ECommerceApp.Core.Domain.Entities.Product;
using ECommerceApp.Core.Domain.Interfaces;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.Helpers;
using ECommerceApp.Core.Services.Abstract;
using ECommerceApp.Core.Services.Concrete;
using ECommerceApp.Infrastructure.Context;
using ECommerceApp.Infrastructure.Repositories;
using ECommerceApp.WebUI.Mappings.Product;
using ECommerceApp.WebUI.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using MongoDB.Bson;
using NToastNotify;
using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);

var mongoConnectionString = $"{builder.Configuration["MongoConnectionString:Connection"]}/{builder.Configuration["MongoConnectionString:DatabaseName"]}";
var seqServer = $"{builder.Configuration["Seq:ServerURL"]}";
Logger log = new LoggerConfiguration()
.WriteTo.MongoDB(mongoConnectionString, "Logs")
.WriteTo.Seq(seqServer)
.Enrich.FromLogContext()
.MinimumLevel.Information()
.CreateLogger();
builder.Host.UseSerilog(log);

// Add services to the container.
builder.Services.AddControllersWithViews().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true
});

builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.DisableDataAnnotationsValidation = true;
});
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddScoped<IRepository<Category, ObjectId>, BaseRepository<Category>>();
builder.Services.AddScoped<IRepository<Language, ObjectId>, BaseRepository<Language>>();
builder.Services.AddScoped<IRepository<Dictionary, ObjectId>, BaseRepository<Dictionary>>();
builder.Services.AddScoped<IRepository<Brand, ObjectId>, BaseRepository<Brand>>();
builder.Services.AddScoped<IRepository<Tag, ObjectId>, BaseRepository<Tag>>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IDictionaryService, DictionaryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddAutoMapper(typeof(CategoryMapping));
builder.Services.AddTransient<UserLanguageMiddleware>();
builder.Services.AddTransient<GlobalErrorHandlingMiddleware>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICookieService, HttpContextCookieService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseRouting();


app.UseAuthorization();
app.UseNToastNotify();

#region Custom Middleware

app.UseMiddleware<UserLanguageMiddleware>();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();

#endregion


app.MapAreaControllerRoute(
    areaName: "Admin",
    name: "Admin Area",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();
