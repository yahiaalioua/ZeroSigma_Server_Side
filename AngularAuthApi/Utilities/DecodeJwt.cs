using System.IdentityModel.Tokens.Jwt;

namespace AngularAuthApi.Utilities
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
    }
}
