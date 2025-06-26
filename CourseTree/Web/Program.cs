using System.Data;
using Microsoft.Data.SqlClient;
using Data.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Системная конфигурация автоматически читает appsettings.json и appsettings.{Environment}.json

// 1. Регистрируем MVC
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddTransient<IDbConnection>(
    sp => new SqlConnection(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map API controllers
app.MapControllers();

app.Run();