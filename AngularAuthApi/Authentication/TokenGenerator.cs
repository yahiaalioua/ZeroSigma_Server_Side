using AngularAuthApi.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AngularAuthApi.Authentication
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken(string secretKey, string issuer, string audience, double expires, List<Claim> claims = null)
        {
            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signinCredentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expires),
                signingCredentials: signinCredentials
                );
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
