using System.Diagnostics;
using CRM.API.AuthenticationController.auth;
using CRM.API.Data;
using CRM.API.Interface;
using CRM.API.Interface.authentication;
using CRM.API.Model;
using CRM.API.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddSingleton<JsonLocalizationService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuthenticationServices, AuthenticationService>();
builder.Services.AddScoped<IApplicatiomEmailSender, ApplicaitonEmailSender>();
builder.Services.Configure<Emailconfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
 builder.Services.AddSingleton<IApplicatiomEmailSender, ApplicaitonEmailSender>();
// Database
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM API", Version = "v1" });
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();

builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1"));

    // Open the default web browser with Swagger UI when the application starts
    var url = "http://localhost:5190/swagger";
    Process.Start(new ProcessStartInfo
    {
        FileName = url,
        UseShellExecute = true
    });

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
