using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;

namespace AngularAuthApi.Authentication.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateToken(LoginDto user);
    }
}
