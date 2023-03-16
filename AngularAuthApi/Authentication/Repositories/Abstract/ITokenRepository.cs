using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;

namespace AngularAuthApi.Authentication.Repositories.Abstract
{
    public interface ITokenRepository
    {
        Task Create(Auth auth);
        Task<Auth> GetAuthDataById(int id);

        Task UpdateAuth(int id, Auth auth);
        Task<Auth> UpdatedAuth(User user, string refreshToken);
        Task<Auth> GetByRefreshToken(string refreshToken);
        Task UpdateAccesToken(int id, string accessToken);
        Task RotateRefreshToken(User user, string refreshToken);
        Task RevokeAccessToken(User user);
        Task RevokeRefreshToken(Auth auth);
    };
}