using AngularAuthApi.DTOS;
using AngularAuthApi.Entities;

namespace AngularAuthApi.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task<User> AuthenticateUser(LoginDto user);
        Task RegisterUser(User user);
        Task<bool> CheckEmailExist(string userEmail);
        string CheckPasswordStrength(string password);
        Task<User> GetUserById(int id);
    }
}
