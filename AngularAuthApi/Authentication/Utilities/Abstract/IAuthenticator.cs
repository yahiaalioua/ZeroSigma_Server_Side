using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using AngularAuthApi.Entities.Responses;

namespace AngularAuthApi.Authentication.Utilities.Abstract
{
    public interface IAuthenticator
    {
        Task<AuthUserResponse> AuthenticateResponse(LoginDto user, User GetUser);
        Task<AuthUserResponse> AuthenticateRefreshToken(User user);
    }
}