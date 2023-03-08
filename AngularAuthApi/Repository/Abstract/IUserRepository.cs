using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;

namespace AngularAuthApi.Repository.Abstract
{
    public interface IUserRepository
    {
        Task<UserInfo> GetUserInfo(int id);
        Task UpdateUserInfo(UserInfo UserInfo);
        Task<User> GetUser(int id);
        Task UpdateUser(User user);


    }
}