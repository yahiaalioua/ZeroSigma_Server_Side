using AngularAuthApi.DTOS;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Responses;

namespace AngularAuthApi.Utilities
{
    public interface IAuthenticator
    {
        Task<AuthUserResponse> AuthenticateResponse(LoginDto user, User GetUser);
        Task<AuthUserResponse> AuthenticateRefreshToken(User user);
    }
}