using System.IdentityModel.Tokens.Jwt;
using AngularAuthApi.Authentication.Utilities.Abstract;

namespace AngularAuthApi.Authentication.Utilities
{
    public class DecodeJwt : IDecodeJwt
    {
        public DateTime GetJwtExpiration(string token)
        {
            var jwt = token;
            var handler = new JwtSecurityTokenHandler();
            var Token = handler.ReadJwtToken(jwt);
            return Token.ValidTo;
        }
        public DateTime GetJwtIssued(string token)
        {
            var jwt = token;
            var handler = new JwtSecurityTokenHandler();
            var Token = handler.ReadJwtToken(jwt);
            return Token.IssuedAt;
        }
        public bool IsExpired(string token)
        {
            var jwt = token;
            var handler = new JwtSecurityTokenHandler();
            var Token = handler.ReadJwtToken(jwt);
            var dateNow = DateTime.UtcNow;
            return Token.ValidTo > dateNow;
        }
        public bool IsActive(string token)
        {
            var jwt = token;
            var handler = new JwtSecurityTokenHandler();
            var Token = handler.ReadJwtToken(jwt);
            var dateNow = DateTime.UtcNow;
            return Token.ValidTo < dateNow;
        }
    }
}
