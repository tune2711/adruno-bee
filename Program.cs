using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using myapp.Data;
using myapp.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Define a more flexible CORS policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins(
                                "http://localhost:5173",
                                "http://localhost:3000",
                                "https://*.ngrok-free.app"
                               )
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .SetIsOriginAllowedToAllowWildcardSubdomains(); // Allow wildcard subdomains for ngrok
                      });
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"))),
        RoleClaimType = ClaimTypes.Role
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add this section to configure Swagger for JWT Authentication
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // CORRECTED: Use Http type with Bearer scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http, // Use Http type for Bearer token
        Scheme = "bearer", // Scheme must be lowercase 'bearer'
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Enter \'Bearer\' [space] and then your token in the text input below.\n\nExample: \"Bearer 12345abcdef\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product { Name = "Laptop", Price = 1200, Description = "A powerful laptop", ImageUrl = "https://via.placeholder.com/150", Category = "Electronics" },
            new Product { Name = "Mouse", Price = 25, Description = "A wireless mouse", ImageUrl = "https://via.placeholder.com/150", Category = "Accessories" },
            new Product { Name = "Keyboard", Price = 45, Description = "A mechanical keyboard", ImageUrl = "https://via.placeholder.com/150", Category = "Accessories" }
        );
        context.SaveChanges();
    }
}

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

// Add Authentication middleware BEFORE Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
