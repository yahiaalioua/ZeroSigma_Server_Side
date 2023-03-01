using AngularAuthApi.Data_Access;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using AngularAuthApi.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthApi.Repository
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly UserDbContext _context;
        public UserInfoRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<UserInfo> GetUserInfo(int id)
        {
            var userInfo = await _context.UserInfo.FirstOrDefaultAsync(u => u.Id == id);
            return userInfo;
        }
        public async Task UpdateUserInfo(UserInfo UserInfo)
        {
            _context.UserInfo.Update(UserInfo);
            _context.SaveChanges();
        }
        
    }
}
