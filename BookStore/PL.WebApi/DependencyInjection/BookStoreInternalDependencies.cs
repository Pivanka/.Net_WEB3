using System.Text;
using BLL.Services;
using BLL.Services.Contracts;
using DAL.Context;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace PL.WebApi.DependencyInjection;

public static class BookStoreInternalDependencies
{
    public static void AddBookStoreInternalDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInternalDependencies(configuration);
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        
        AddSwagger(services);
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Authentication Login",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JsonWebToken",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
            options.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
                Example = new OpenApiString("2023-01-01")
            });
        });
    }

    private static void AddInternalDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookStoreDbContext>
            (o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<BookStoreDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IRepository<Book>, Repository<Book>>();
        services.AddScoped<IRepository<Review>, Repository<Review>>();
        services.AddScoped<IRepository<Rating>, Repository<Rating>>();
        services.AddScoped<IRepository<Order>, Repository<Order>>();
        services.AddScoped<IRepository<ShoppingCart>, Repository<ShoppingCart>>();
        
        services.AddScoped<ITokenManager, TokenManager>();
    }
}