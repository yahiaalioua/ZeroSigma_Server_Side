using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AngularAuthApi.Authentication.OptionsSetup
{
    public class JwtBarerConfigOptionsSetup : IConfigureOptions<JwtBearerOptions>
    {
        private readonly JwtConfigOptions _jwtConfigOptions;
        public JwtBarerConfigOptionsSetup(IOptions<JwtConfigOptions> JwtConfigOptions)
        {
            _jwtConfigOptions = JwtConfigOptions.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters =new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtConfigOptions.Issuer,
                ValidAudience = _jwtConfigOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jdsfjhdsfhoi3ho3t84398oisehføksha")),
                ClockSkew=TimeSpan.Zero
            };
        }
    }
}
