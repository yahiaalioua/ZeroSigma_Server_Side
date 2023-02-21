using AngularAuthApi.Entities;

namespace AngularAuthApi.Repositories
{
    public interface ITokenRepository
    {
        Task Create(Auth auth);
    }
}