using System.Text;
using FunkoApi.config;
using FunkoApi.Models;
using FunkoApi.Repository;
using FunkoApi.Service;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


Console.OutputEncoding = Encoding.UTF8; 
var builder = WebApplication.CreateBuilder(args);
// negociacion de serializables
builder.Services.AddControllers(options =>
    {
        options.RespectBrowserAcceptHeader = true;
        options.ReturnHttpNotAcceptable = true;
    })
    .AddXmlSerializerFormatters()
    .AddXmlDataContractSerializerFormatters();

//base de datos en memoria
builder.Services.AddDbContext<FunkoDbContext>(options =>
    options.UseInMemoryDatabase("FunkoInMemoryDb"));
// repositorios
builder.Services.AddScoped<IRepository<Categoria,string>,CategoryRepository>();
builder.Services.AddScoped<IFunkoRepository, FunkoRepository>();
// servicio
builder.Services.AddScoped<IService, FunkoService>();
builder.Services.AddMemoryCache();
// Add services to the container.
builder.Services.AddControllersWithViews();
// Configurar límite de tamaño de formularios
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
});

// Configurar límite del request body
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
});
// storage
builder.Services.Configure<StorageSettings>(
    builder.Configuration.GetSection("Storage")
);

// Opcional: Singleton si solo tienes una instancia
builder.Services.AddSingleton<IOptions<StorageSettings>>(
    sp => Options.Create(sp.GetRequiredService<IConfiguration>()
        .GetSection("Storage").Get<StorageSettings>())
);
var app = builder.Build();
app.MapControllers();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


app.MapStaticAssets();



app.Run();