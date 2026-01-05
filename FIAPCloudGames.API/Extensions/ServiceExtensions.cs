using FIAPCloudGames.API.Auth;
using FIAPCloudGames.Application.Consumers;
using FIAPCloudGames.Application.Services;
using FIAPCloudGames.Domain.Interfaces;
using FIAPCloudGames.Infrastructure.DatabaseContext;
using FIAPCloudGames.Infrastructure.Repositories;
using LiteDB;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FIAPCloudGames.API.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        #region Configuração do JWT
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtConfig["Key"]);

        builder.Services.AddSingleton<IAuthorizationHandler, ApiKeyRequirementHandler>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // somente em dev
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidAudience = jwtConfig["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
            options.AddPolicy("ApiKeyPolicy", policy => policy.AddRequirements(new ApiKeyRequirement()));
        });
        #endregion

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "FIAP Cloud Games", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Digite 'Bearer {seu token}'"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            }
        );

        builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Main")));

        #region Injeção de dependências
        builder.Services.AddSingleton<ILiteDatabase, LiteDatabase>(_ => new LiteDatabase("event_source.db"));
        builder.Services.AddTransient<IEventRepository, EventRepository>();
        builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
        builder.Services.AddTransient<IPaymentService, PaymentService>();
        builder.Services.AddTransient<IPurchaseRepository, PurchaseRepository>();
        builder.Services.AddTransient<IPurchaseService, PurchaseService>();
        #endregion

        #region Configuração do MassTransit com RabbitMQ
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<PaymentProcessingConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqHost = builder.Configuration["RabbitMq:Host"] ?? "amqp://guest:guest@localhost:5672";

                cfg.Host(rabbitMqHost);

                cfg.ConfigureEndpoints(context);
            });
        });
        #endregion

        builder.Services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        });
    }

    public static void UseServices(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseCustomExceptionHandling();
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
