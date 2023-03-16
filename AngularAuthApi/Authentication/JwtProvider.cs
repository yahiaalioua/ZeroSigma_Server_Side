using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AngularAuthApi.Authentication
{
    public class JwtProvider : IJwtProvider
    {   
        private readonly ITokenGenerator _tokenGenerator;
        private readonly JwtOptions _jwtConfigOptions;
        public JwtProvider(IOptions<JwtOptions> jwtConfigOptions, ITokenGenerator tokenGenerator)
        {
            _jwtConfigOptions = jwtConfigOptions.Value;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken(LoginDto user)
        {
            
            
     
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
            };
            return _tokenGenerator.GenerateToken(_jwtConfigOptions.AccessTokenSecretKey,_jwtConfigOptions.Issuer,_jwtConfigOptions.Audience,_jwtConfigOptions.AccessTokenExpirationMinutes,claims);
            

        }
    }
}
