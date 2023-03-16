﻿using AngularAuthApi.Authentication.DTOS;
using AngularAuthApi.Entities;

namespace AngularAuthApi.Authentication.Repositories.Abstract
{
    public interface IUserAuthRepository
    {
        Task<User> AuthenticateUser(LoginDto user);
        Task RegisterUser(User user);
        Task<bool> CheckEmailExist(string userEmail);
        string CheckPasswordStrength(string password);
        Task<User> GetUserById(int id);
        Task DeleteUser(User user);
        Task Logout(User user,Auth auth);


    }
}
