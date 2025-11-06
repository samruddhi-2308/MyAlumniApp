using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyAlumniApp.Data;
using System.IO; // ✅ Needed for Path.Combine

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure port from Render's PORT environment variable
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://+:{port}");
    Console.WriteLine($"🌐 Configured to listen on port {port}");
}

// ✅ Build connection string from environment variables (for Render/Railway)
var mysqlHost = Environment.GetEnvironmentVariable("MYSQLHOST") ?? "localhost";
var mysqlPort = Environment.GetEnvironmentVariable("MYSQLPORT") ?? "3306";
var mysqlDatabase = Environment.GetEnvironmentVariable("MYSQLDATABASE") ?? "aps_alumni";
var mysqlUser = Environment.GetEnvironmentVariable("MYSQLUSER") ?? "root";
var mysqlPassword = Environment.GetEnvironmentVariable("MYSQLPASSWORD") ?? "";

// Try to get from configuration first, then fall back to environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("${"))
{
    // Build connection string from environment variables
    // Railway/Render MySQL typically requires SSL, but we'll try without first
    connectionString = $"server={mysqlHost};port={mysqlPort};database={mysqlDatabase};user={mysqlUser};password={mysqlPassword};SslMode=Preferred;AllowUserVariables=True;CharSet=utf8mb4;";
}

Console.WriteLine($"🔌 Database: {mysqlDatabase}@{mysqlHost}:{mysqlPort}");

// ✅ Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
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

// ✅ Error handling middleware
app.UseExceptionHandler("/error");
app.UseStatusCodePages();

// ✅ Allow JS fetch calls from any origin
app.UseCors("AllowAll");

// ✅ Serve static files (HTML, images, etc.)
var staticPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
Console.WriteLine("👉 Serving static files from: " + staticPath);

if (Directory.Exists(staticPath))
{
    // ✅ Let index.html and results.html be directly accessible (must come before UseStaticFiles)
    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = new PhysicalFileProvider(staticPath)
    });
    
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(staticPath),
        RequestPath = "" // means served directly under root (e.g. /images/akshay.jpg)
    });
}
else
{
    Console.WriteLine("⚠️ Warning: wwwroot directory not found!");
}

// ✅ Map API endpoints (controllers)
app.MapControllers();

// ✅ Health check endpoint
app.MapGet("/health", async (IServiceProvider serviceProvider) =>
{
    try
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var canConnect = await dbContext.Database.CanConnectAsync();
            return Results.Ok(new 
            { 
                status = "healthy", 
                database = canConnect ? "connected" : "disconnected",
                timestamp = DateTime.UtcNow 
            });
        }
    }
    catch (Exception ex)
    {
        return Results.Ok(new 
        { 
            status = "healthy", 
            database = "error",
            error = ex.Message,
            timestamp = DateTime.UtcNow 
        });
    }
});

// ✅ Test database connection asynchronously (non-blocking)
_ = Task.Run(async () =>
{
    await Task.Delay(2000); // Wait 2 seconds after startup
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var canConnect = await dbContext.Database.CanConnectAsync();
            if (canConnect)
            {
                Console.WriteLine("✅ Database connection successful!");
            }
            else
            {
                Console.WriteLine("⚠️ Database connection test returned false");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database connection failed: {ex.Message}");
    }
});

Console.WriteLine($"🚀 Application starting...");

app.Run();
