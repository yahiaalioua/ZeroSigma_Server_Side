using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;

namespace AngularAuthApi.Repositories
{
    public interface ITokenRepository
    {
        Task Create(Auth auth);
        Task<Auth> GetAuthDataById(int id);

        Task UpdateAuth(int id, Auth auth);
        Task<Auth> UpdatedAuth(User user, string refreshToken);
        Task<Auth> GetByRefreshToken(string refreshToken);
    };
}