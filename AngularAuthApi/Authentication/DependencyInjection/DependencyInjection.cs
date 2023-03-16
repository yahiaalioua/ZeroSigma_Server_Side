using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.Authentication.OptionsSetup;
using AngularAuthApi.Authentication.Repositories.Abstract;
using AngularAuthApi.Authentication.Repositories;
using AngularAuthApi.Authentication.Utilities.Abstract;
using AngularAuthApi.Authentication.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AngularAuthApi.Repository.Abstract;
using AngularAuthApi.Repository;
using AngularAuthApi.Core.DcfCalculator.Abstract;
using AngularAuthApi.Core.DcfCalculator.HttpClient;
using AngularAuthApi.Core.DcfCalculator.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AngularAuthApi.Authentication.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthenticationandAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();
            services.AddScoped<IConfRefreshTokenValidationParameters, ConfRefreshTokenValidationParameters>();
            services.AddScoped<IRefreshTokenValidate, RefreshTokenValidate>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IAuthenticator, Authenticator>();
            services.AddScoped<IDecodeJwt, DecodeJwt>();
            services.ConfigureOptions<JwtConfigOptionsSetup>();
            services.ConfigureOptions<RefreshJwtConfigOptionsSetup>();
            //services.ConfigureOptions<JwtBarerConfigOptionsSetup>(); not working, will figure out another time
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "https://localhost:5001",
                    ValidIssuer = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("superSecretKey@345")
                    ),
                    ClockSkew=TimeSpan.Zero                    
                };
            });
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserAuthRepository, UserAuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddCoreDiscountedCashFlow(this IServiceCollection services)
        {
            services.AddScoped<IFinancialPrepHttpCalls, FinancialPrepHttpCalls>();
            services.AddScoped<ICoreDcfService, CoreDcfService>();
            return services;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

    }
}
