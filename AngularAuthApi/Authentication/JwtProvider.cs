using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.DTOS;
using AngularAuthApi.Entities;
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
        private readonly JwtConfigOptions _jwtConfigOptions;
        public JwtProvider(IOptions<JwtConfigOptions> jwtConfigOptions)
        {
            _jwtConfigOptions = jwtConfigOptions.Value;
        }

        public string GenerateToken(LoginDto user)
        {
            
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jdsfjhdsfhoi3ho3t84398oisehføksha"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
            };
            JwtSecurityToken token = new JwtSecurityToken
                (
                issuer: _jwtConfigOptions.Issuer,
                audience: _jwtConfigOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwtConfigOptions.AccessTokenExpirationHours),
                signingCredentials: signinCredentials
                );
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;

        }
    }
}
