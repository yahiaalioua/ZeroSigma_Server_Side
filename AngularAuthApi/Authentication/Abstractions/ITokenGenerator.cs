using System.Security.Claims;

namespace AngularAuthApi.Authentication
{
    public interface ITokenGenerator
    {
        string GenerateToken(string secretKey, string issuer, string audience, int expires, List<Claim> claims = null);
    }
}