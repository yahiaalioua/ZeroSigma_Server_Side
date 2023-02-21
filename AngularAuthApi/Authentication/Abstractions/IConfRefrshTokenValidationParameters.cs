using Microsoft.IdentityModel.Tokens;

namespace AngularAuthApi.Authentication.OptionsSetup
{
    public interface IConfRefreshTokenValidationParameters
    {
        TokenValidationParameters Parameters();
    }
}