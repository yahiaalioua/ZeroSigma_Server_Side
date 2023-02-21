using AngularAuthApi.Entities;

namespace AngularAuthApi.Authentication.Abstractions
{
    public interface IRefreshTokenProvider
    {
        string GenerateRefreshToken(User user);
    }
}
