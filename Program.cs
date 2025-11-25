using System.Diagnostics;
using api.Service;
using CRM.API.Data;
using CRM.API.Interface;
using CRM.API.Interface.authentication;
using CRM.API.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MyApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------
// Services
// -------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddSingleton<JsonLocalizationService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthenticationServices, AuthenticationService>();
builder.Services.AddScoped<IApplicatiomEmailSender, ApplicaitonEmailSender>();
builder.Services.AddScoped<ItokenService, TokenService>();
 builder.Services.AddSingleton<IApplicatiomEmailSender, ApplicaitonEmailSender>();
builder.Services.Configure<Emailconfiguration>(
    builder.Configuration.GetSection("EmailConfiguration")
);
builder.Services.AddCustomRateLimiting(); 
builder.Services.AddScoped<TenantContext>();
builder.Services.AddDbContext<TenantDbContext>();
builder.Services.AddScoped<TenantResolver>();
builder.Services.AddScoped<LandlordDbContext>();
// -------------------------------------------------------------
// Database
// -------------------------------------------------------------
builder.Services.AddDbContext<LandlordDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LandlordConnection"));
});

builder.Services.AddDbContext<TenantDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TenantConnection")));

// -------------------------------------------------------------
// Identity
// -------------------------------------------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<LandlordDbContext>()
.AddDefaultTokenProviders();

// -------------------------------------------------------------
// JWT Authentication
// -------------------------------------------------------------
var signingKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]!)
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey
    };
});

// -------------------------------------------------------------
// JSON (Fix Reference Cycles)
// -------------------------------------------------------------
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// -------------------------------------------------------------
// Swagger
// -------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM API", Version = "v1" });

    // Enable JWT input box in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// -------------------------------------------------------------
// Build
// -------------------------------------------------------------
var app = builder.Build();

// -------------------------------------------------------------
// Pipeline
// -------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Auto-open browser
    Process.Start(new ProcessStartInfo
    {
        FileName = "http://localhost:5190/swagger",
        UseShellExecute = true
    });
}
//Middleware rate limiter
app.UseCustomRateLimiting();   
// -------------------------------------------------------------         
app.UseHttpsRedirection();

app.UseAuthentication();   
app.UseAuthorization();

app.MapControllers();
app.Run();

