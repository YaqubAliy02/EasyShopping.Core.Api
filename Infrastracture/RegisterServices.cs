﻿using System.Text;
using Application.Abstraction;
using Application.Repository;
using Azure.Storage.Blobs;
using Infrastracture.Data;
using Infrastracture.External.AWSS3;
using Infrastracture.External.Blobs;
using Infrastracture.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastracture
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IEasyShoppingDbContext, EasyShoppingDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            // services.AddScoped<IBlobStorage, BlobStorage>();
            services.AddScoped<IAWSStorage, AWSStorage>();
            services.AddScoped<IProductThumbnailRepository, ProductThumbnailRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ISubCommentRepository, SubCommentRepository>();
            services.AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration["AzureBlobStorage:ConnectionString"];
                return new BlobServiceClient(connectionString);
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = configuration["JWT:AudienceKey"],
                        ValidIssuer = configuration["JWT:IssuerKey"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = (context) =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers["IsTokenExpired"] = "true";
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            return services;
        }
    }
}
