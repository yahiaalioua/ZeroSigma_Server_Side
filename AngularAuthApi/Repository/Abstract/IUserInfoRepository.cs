using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;

namespace AngularAuthApi.Repository.Abstract
{
    public interface IUserInfoRepository
    {
        Task<UserInfo> GetUserInfo(int id);
        Task UpdateUserInfo(UserInfo UserInfo);
        
    }
}