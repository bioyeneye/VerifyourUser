using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using VerifyourUser.DataAccess.Context;
using VerifyourUser.Domain.Core;
using VerifyourUser.Domain.Core.Abstract;

namespace VerifyourUser.API.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static string CORS_POLICY = "ApiCorsPolicy";
        public static IServiceCollection AddCoreService(this IServiceCollection service)
        {
            service.AddCors(options => options.AddPolicy(CORS_POLICY, builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            return service;
        }

        public static IServiceCollection AddDatabaseService(this IServiceCollection service, IConfiguration configuration)
        {

            service.AddDbContext<VerifyourUserDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("VerifyourUserDbContext"));

                options.UseOpenIddict();
            });

            return service;
        }

        public static IServiceCollection AddServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddTransient<IAudience, AudienceManager>();
            return service;
        }

        public static IServiceCollection AddOpenIdService(this IServiceCollection service, IConfiguration configuration)
        {

            service.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore().UseDbContext<VerifyourUserDbContext>();
            })
            .AddServer(options =>
            {
                options.UseMvc();
                options.EnableTokenEndpoint("/oauth/token");
                options.AllowPasswordFlow();
                options.AllowClientCredentialsFlow();
                options.AllowRefreshTokenFlow();
                options.DisableHttpsRequirement();
                options.UseJsonWebTokens();
                options.AddEphemeralSigningKey();
                options.RegisterScopes(
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    "roles",
                    "role"
                );
                //options.AddSigningCertificate(new System.Security.Cryptography.X509Certificates.X509Certificate2(Path.Combine("C:\\", "certs", "RebirthTest.pfx")));
                //options.AcceptAnonymousClients();
                options.SetAccessTokenLifetime(TimeSpan.FromHours(1));
            });
            return service;
        }

        public static IServiceCollection AddJWTAuthentication(this IServiceCollection service, IConfiguration configuration)
        {

            service.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["OAuth:Authority"];
                options.Audience = configuration["OAuth:ApiName"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = false
                };
            });

            return service;
        }
    }
}
