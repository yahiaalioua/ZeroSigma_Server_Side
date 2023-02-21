using AngularAuthApi.Data_Access;
using AngularAuthApi.DTOS;
using AngularAuthApi.Entities;
using System.Runtime.CompilerServices;

namespace AngularAuthApi.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly UserDbContext _context;

        public TokenRepository(UserDbContext context)
        {
            _context = context;
        }
        public async Task Create(Auth auth)
        {
            await _context.Auth.AddAsync(auth);
            await _context.SaveChangesAsync();
        }
    }
}
