using AngularAuthApi.Authentication.DTOS;
using AngularAuthApi.Authentication.Repositories.Abstract;
using AngularAuthApi.Authentication.Utilities;
using AngularAuthApi.Data_Access;
using AngularAuthApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AngularAuthApi.Authentication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateUser(LoginDto user)
        {
            var FindUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            return FindUser;

        }
        public async Task RegisterUser(User user)
        {
            user.Password = PasswordHasher.HashPasword(user.Password, out var salt);
            user.Token = " ";
            user.Salt = Convert.ToHexString(salt);
            user.Auth = new Auth() { Id = user.Id };
            user.UserInfo= new UserInfo() { Id = user.Id };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckEmailExist(string userEmail)
        {
            return await _context.Users.AnyAsync(u => u.Email == userEmail);
        }
        public string CheckPasswordStrength(string password)
        {
            StringBuilder message = new StringBuilder();
            if (password.Length < 8)
            {
                message.Append("Your password should be at least 8 characters");
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                message.Append("Your password should contain at least one number and one upper and lowercase letter");
            }
            if (!Regex.IsMatch(password, "[`,~,!,@,#,$,%,^,&,*,(,),_,-,+,=,{,[,},},|,\\,:,;,\",',<,,,>,.,?,/]"))
            {
                message.Append("Your password should contain at least one special character");
            }
            return message.ToString();
        }
        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task DeleteUser(User user)
        {
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        

    }
}
