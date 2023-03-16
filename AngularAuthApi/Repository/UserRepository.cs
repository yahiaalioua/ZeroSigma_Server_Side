using AngularAuthApi.Data_Access;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using AngularAuthApi.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }
        public async Task<User>GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task<UserInfo> GetUserInfo(int id)
        {
            var userInfo = await _context.UserInfo.FirstOrDefaultAsync(u => u.Id == id);
            return userInfo;
        }
        public async Task UpdateUserInfo(UserInfo UserInfo)
        {
            _context.UserInfo.Update(UserInfo);
            await _context.SaveChangesAsync();
        }
        public  async Task UpdateUser(User user)
        {
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser(User user)
        {

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

    }
}
