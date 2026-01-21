using System.Text;
using FunkoApi.config;
using FunkoApi.handler.funko;
using FunkoApi.Infrastructures;
using FunkoApi.Models;
using FunkoApi.Repository;
using FunkoApi.Repository.Category;
using FunkoApi.Repository.funkos;
using FunkoApi.Service;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using TiendaApi.Apis.Infrastructures;

Log.Logger= SerilogConfig.Configure().CreateLogger();
Console.OutputEncoding = Encoding.UTF8; 
var builder = WebApplication.CreateBuilder(args);
//configuracion log
builder.Host.UseSerilog();
var services = builder.Services;
// negociacion de serializables
services.AddMvcControllers();

//base de datos en memoria
builder.Services.AddDbContext<FunkoDbContext>(options =>
    options.UseInMemoryDatabase("FunkoInMemoryDb"));
// repositorios
services.AddRepositories();
// servicios
services.AddServices();
// cache
services.AddCache();
services.AddStorage();
services.AddWebSockets();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();




app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets();
app.MapWebSocketEndpoints();
app.UseStaticFiles();
app.MapControllers();
app.InitializeDatabaseAsync();
app.InitializeStorage();


try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "💥 La aplicación falló al iniciar");
    throw;
}
finally
{
    Log.CloseAndFlush();
}