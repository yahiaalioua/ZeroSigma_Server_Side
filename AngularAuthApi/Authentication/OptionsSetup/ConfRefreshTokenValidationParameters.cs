using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AngularAuthApi.Authentication.OptionsSetup
{
    public class ConfRefreshTokenValidationParameters : IConfRefreshTokenValidationParameters
    {
        private readonly RefreshJwtConfigOptions _refreshJwtConfigoptions;

        public ConfRefreshTokenValidationParameters(IOptions<RefreshJwtConfigOptions> refreshJwtConfigOptions)
        {
            _refreshJwtConfigoptions = refreshJwtConfigOptions.Value;
        }

        public TokenValidationParameters Parameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _refreshJwtConfigoptions.Issuer,
                ValidAudience = _refreshJwtConfigoptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jdsfjhdsfhoi3ho3t84398oisehføkshasuperSecretKey@345fhftftffhjg6r66")),
                ClockSkew = TimeSpan.Zero
            };

        }

    }
}
