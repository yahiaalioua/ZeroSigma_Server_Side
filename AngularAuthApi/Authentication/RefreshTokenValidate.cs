using AngularAuthApi.Authentication.OptionsSetup;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AngularAuthApi.Authentication
{
    public class RefreshTokenValidate : IRefreshTokenValidate
    {
        private readonly IConfRefreshTokenValidationParameters _ConfRefreshParameters;

        public RefreshTokenValidate(IConfRefreshTokenValidationParameters confRefreshParameters)
        {
            _ConfRefreshParameters = confRefreshParameters;
        }

        public  bool Validate(string refreshToken)
        {
            TokenValidationParameters tokenValidationParameters = _ConfRefreshParameters.Parameters();
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var token);
                return true;
            }
            catch (Exception)
            {

                return false;
            };

        }
    }
}
