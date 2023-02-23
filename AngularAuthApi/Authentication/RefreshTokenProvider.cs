using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.Authentication.OptionsSetup;
using AngularAuthApi.DTOS;
using AngularAuthApi.Entities;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace AngularAuthApi.Authentication
{
    public class RefreshTokenProvider:IRefreshTokenProvider
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly RefreshJwtConfigOptions _jwtConfigOptions;

        public RefreshTokenProvider(ITokenGenerator tokenGenerator,IOptions<RefreshJwtConfigOptions> jwtConfigOptions)
        {
            _tokenGenerator = tokenGenerator;
            _jwtConfigOptions = jwtConfigOptions.Value;
        }
        public string GenerateRefreshToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };
            
            
            return _tokenGenerator.GenerateToken(_jwtConfigOptions.RefreshTokenSecretKey, _jwtConfigOptions.Issuer, _jwtConfigOptions.Audience,_jwtConfigOptions.RefreshTokenExpirationHours,claims);
            
        }

        
    }
}
