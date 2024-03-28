using AutoPartsAPI.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("WEB API START");

try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    builder.Logging.AddFilter((category, level) =>
    {
        if (category.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase) && level >= Microsoft.Extensions.Logging.LogLevel.Warning)
        {
            return true;
        }
        else if (category.StartsWith("System", StringComparison.OrdinalIgnoreCase) && level >= Microsoft.Extensions.Logging.LogLevel.Warning)
        {
            return true;
        }
        return false;
    }).AddConsole();

    builder.Logging.AddDebug();
    builder.Logging.AddEventSourceLogger();

    builder.Host.UseNLog();

    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidateAudience = true,
                ValidAudience = AuthOptions.AUDIENCE,
                ValidateLifetime = false,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true
            };
        });

    builder.Services.AddControllers();
    builder.Services.AddDbContext<Paint2API.Models.PaintContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("con")));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PaintAPI", Version = "v1" });
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
        });
    });

    var app = builder.Build();

    app.UseRouting();

    app.UseAuthentication();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.Run();
}
catch (Exception ex)
{
    // Логирование ошибки в случае возникновения исключения
    logger.Error(ex, "WEB API ERROR");
    throw;
}
finally
{
    // Завершение работы логгера
    LogManager.Shutdown();
}