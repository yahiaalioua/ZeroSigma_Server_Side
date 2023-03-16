using System.Security.Claims;

namespace AngularAuthApi.Authentication
{
    public interface ITokenGenerator
    {
        string GenerateToken(string secretKey, string issuer, string audience, double expires, List<Claim> claims = null);
    }
}