using ECommerceApp.Core.Domain.Entities;
using ECommerceApp.Core.Domain.Interfaces;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using ECommerceApp.Core.Services.Abstract;
using ECommerceApp.Core.Services.Concrete;
using ECommerceApp.Infrastructure.Context;
using ECommerceApp.Infrastructure.Repositories;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddScoped<IRepository<Category, ObjectId>, BaseRepository<Category>>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
