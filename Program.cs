
using myapp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Define a more flexible CORS policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          // Allow specific origins including your ngrok tunnel and common frontend dev ports
                          policy.WithOrigins(
                                "http://localhost:5173",                // Standard Vite/React port
                                "http://localhost:3000",                // Standard create-react-app port
                                "https://343a5356c607.ngrok-free.app"    // Your ngrok URL
                               )
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// The order of middleware is crucial.
app.UseDefaultFiles();
app.UseStaticFiles();

// UseRouting must be placed before UseCors and UseAuthorization.
app.UseRouting();

// Apply the CORS policy
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
