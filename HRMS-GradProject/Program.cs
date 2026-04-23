
using Application.Interfaces;
using Application.Services.Implementations;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;
using Application.DTOs.Auth;
using HRMS_API.Middleware;
using HRMS_API.Filters;


var builder = WebApplication.CreateBuilder(args);

// CORS: allow Angular dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
        policy.SetIsOriginAllowed(origin => true) // Allow any localhost port or URL for dev
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<RegisterDtoSchemaFilter>();
});

builder.Services.AddControllers(options =>
    {
        // Apply ValidateModelAttribute globally to all controllers
        options.Filters.Add<ValidateModelAttribute>();
    })
    .AddJsonOptions(options =>
    {
        // This makes JSON serialization handle Enums as strings (e.g. "Admin") instead of numbers
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Auhentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Global exception handler — must be first in the pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularDev");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public class RegisterDtoSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(RegisterDto))
        {
            schema.Example = new OpenApiObject
            {
                ["username"] = new OpenApiString("saad"),
                ["email"] = new OpenApiString("saadalrabadi2@gmail.com"),
                ["password"] = new OpenApiString("Password123!"),
                ["role"] = new OpenApiString("Employee"), 
                ["employeeId"] = new OpenApiNull()
            };
        }
    }
}