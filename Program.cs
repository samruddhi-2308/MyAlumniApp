using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyAlumniApp.Data;
using System.IO; // ✅ Needed for Path.Combine

var builder = WebApplication.CreateBuilder(args);

// ✅ Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34))
    )
);

// ✅ Controllers + CORS
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// ✅ Allow JS fetch calls from any origin
app.UseCors("AllowAll");

// ✅ Serve static files (HTML, images, etc.)
var staticPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
Console.WriteLine("👉 Serving static files from: " + staticPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticPath),
    RequestPath = "" // means served directly under root (e.g. /images/akshay.jpg)
});

// ✅ Let index.html and results.html be directly accessible
app.UseDefaultFiles();

// ✅ Map API endpoints (controllers)
app.MapControllers();

app.Run();
