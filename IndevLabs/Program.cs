using System.Text;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using IndevLabs.ExceptionFilter;
using IndevLabs.Extensions;
using IndevLabs.Mapping;
using IndevLabs.Models;
using IndevLabs.Models.db;
using IndevLabs.Repository;
using IndevLabs.Repository.Interface;
using IndevLabs.Service;
using IndevLabs.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logging

// Общий шаблон для вывода логов
string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {RequestId} {Message:lj} {Exception}{NewLine}";

// Настройка Serilog для логирования
LoggerConfiguration loggerConfiguration;

if (!builder.Environment.IsProduction()) {
    loggerConfiguration = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .Enrich.FromLogContext();

    // Настройка HTTP-логирования для Development
    builder.Services.AddHttpLogging(logging => {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });
} else {
    loggerConfiguration = new LoggerConfiguration()
        .MinimumLevel.Information();
}

// Конфигурация Serilog
var logger = loggerConfiguration
    .WriteTo.File(
        "logs/UserProfileService-.log",
        outputTemplate: outputTemplate,
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true
    )
    .WriteTo.Console(outputTemplate: outputTemplate)
    .CreateLogger();

builder.Host.UseSerilog(logger);

#endregion

builder.Services.AddDbContext<IndevLabsDbContext>();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthorizationService>();

// Настройка JWT-аутентификации
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
//Add repository
builder.Services
    .AddScoped<IWineRepository, WineRepository>();

// Add services to the container.
builder.Services
    .AddScoped<WineService>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT Auth Example", Version = "v1" });

        // Добавьте поддержку JWT в Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    })
    .AddControllers(options =>
    {
        options.Filters.Add<ExceptionFilter>();
    });

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<Program>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IValidationStorage, ValidationStorage>();

var app = builder.Build();

app.MigrateDatabase<IndevLabsDbContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IndevLabs v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/api/public/ping", () => "O Captain! My Captain!");
app.Run();
