using System.Text;
using FunkoApi.config;
using FunkoApi.Infrastructures;
using Microsoft.EntityFrameworkCore;
using Serilog;

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