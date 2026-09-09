using DW01.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    Console.WriteLine(">>> Entrando Middleware 1");
    await next();
    Console.WriteLine("<<< Saindo Middleware 1");
});
app.Use(async (context, next) =>
{
    Console.WriteLine("     >>> Entrando Middleware 2");
    if(context.Request.Path == "/bloqueado")
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Acesso bloqueado.");
        return;
    }
    await next();
    Console.WriteLine("     <<< Saindo Middleware 2");
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
