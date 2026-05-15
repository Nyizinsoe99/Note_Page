// using Microsoft.AspNetCore.OData;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.OData.ModelBuilder;
// using Node_Page.Model;

// var builder = WebApplication.CreateBuilder(args);

// var modelBuilder = new ODataConventionModelBuilder();
// modelBuilder.EntitySet<Note>("Notes");

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// builder.Services.AddControllers().AddOData(options => 
//     options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
//            .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

// builder.Services.AddCors(options => {
//     options.AddDefaultPolicy(policy => 
//         policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// });

// var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     try
//     {
//         var context = services.GetRequiredService<AppDbContext>();
//         context.Database.EnsureCreated(); 
//     }
//     catch (Exception ex)
//     {
//         var logger = services.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "An error occurred while creating the database.");
//     }
// }


//----------------------------------------------------------------------------------

// app.UseCors();
// app.UseRouting();
// app.MapControllers();

// app.Run();

// using Microsoft.AspNetCore.OData;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.OData.ModelBuilder;
// using Node_Page.Model;

// var builder = WebApplication.CreateBuilder(args);

// // 1. Setup OData Model
// var modelBuilder = new ODataConventionModelBuilder();
// modelBuilder.EntitySet<Note>("Notes");

// // 2. Add Services
// builder.Services.AddControllers().AddOData(options => 
//     options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
//            // --- CHANGE PLACE: "odata" changed to "Node" ---
//            .AddRouteComponents("Node", modelBuilder.GetEdmModel())); 

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
//     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// builder.Services.AddCors(options => {
//     options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// });

// var app = builder.Build();

// // 3. AUTO-CREATE DATABASE LOGIC
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var context = services.GetRequiredService<AppDbContext>();
//     context.Database.EnsureCreated(); 
// }

// app.UseCors();
// app.UseRouting();
// app.MapControllers();

// app.Run();

//----------------------------------------------------------------------------------

// using Microsoft.AspNetCore.OData;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.OData.ModelBuilder;
// using Node_Page.Model; // Ensure this matches your folder name

// var builder = WebApplication.CreateBuilder(args);

// // 1. SETUP ODATA MODEL
// var modelBuilder = new ODataConventionModelBuilder();
// modelBuilder.EntitySet<Note>("Notes"); // This defines the "Notes" endpoint

// // 2. REGISTER DATABASE SERVICE (MUST be before builder.Build())
// // Change 'DefaultConnection' in appsettings.json if you rename it
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// // 3. ADD CONTROLLERS AND ODATA
// builder.Services.AddControllers().AddOData(options => 
//     options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
//            // CHANGE THIS: Change "Node" to whatever you want your URL prefix to be
//            .AddRouteComponents("Node", modelBuilder.GetEdmModel())); 

// // 4. CONFIGURE CORS (For your frontend)
// builder.Services.AddCors(options => {
//     options.AddDefaultPolicy(policy => 
//         policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// });

// var app = builder.Build();

// // 5. AUTOMATIC DATABASE CREATION (The code for Line 78)
// // This scope ensures the database is created the moment you run the app
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     try
//     {
//         var context = services.GetRequiredService<AppDbContext>();
//         context.Database.EnsureCreated(); // Creates DB and Tables automatically
//     }
//     catch (Exception ex)
//     {
//         // This will print an error in your console if MySQL is not running
//         var logger = services.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "Could not create database. Check if MySQL is running.");
//     }
// }

// // 6. MIDDLEWARE PIPELINE
// app.UseCors();
// app.UseRouting();
// app.MapControllers();

// app.Run();

//----------------------------------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Node_Page.Model; // Ensure this matches your namespace in Note.cs

var builder = WebApplication.CreateBuilder(args);

// 1. OData Setup
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Note>("Notes");

// 2. Database Registration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 3. Controllers & OData Prefix
builder.Services.AddControllers().AddOData(options => 
    options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
           .AddRouteComponents("Node", modelBuilder.GetEdmModel()));

// 4. CORS Support
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => 
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// 5. AUTO-CREATE DATABASE LOGIC
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); // Creates the DB if it doesn't exist
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database connection failed. Check if MySQL is running.");
    }
}

// 6. STATIC FILE SUPPORT (Shows index.html)
app.UseDefaultFiles(); // Tells the server to look for 'index.html'
app.UseStaticFiles();  // Tells the server to allow access to the wwwroot folder

app.UseCors();
app.UseRouting();
app.MapControllers();

app.Run();