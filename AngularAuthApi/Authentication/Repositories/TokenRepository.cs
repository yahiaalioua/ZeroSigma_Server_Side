using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.Data_Access;
using AngularAuthApi.Entities;
using Microsoft.EntityFrameworkCore;
using AngularAuthApi.Authentication.Repositories.Abstract;
using AngularAuthApi.Authentication.Utilities.Abstract;

namespace AngularAuthApi.Authentication.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly UserDbContext _context;
        private readonly IDecodeJwt _decodeJwt;
        private readonly IRefreshTokenProvider _refreshTokenProvider;

        public TokenRepository(UserDbContext context, IDecodeJwt decodeJwt, IRefreshTokenProvider refreshTokenProvider)
        {
            _context = context;
            _decodeJwt = decodeJwt;
            _refreshTokenProvider = refreshTokenProvider;
        }
        public async Task Create(Auth auth)
        {
            await _context.Auth.AddAsync(auth);
            await _context.SaveChangesAsync();
        }
        public async Task<Auth> GetAuthDataById(int id)
        {
            Auth auth = await _context.Auth.FirstOrDefaultAsync(a => a.Id == id);
            return auth;
        }
        public async Task UpdateAuth(int id, Auth auth)
        {
            Auth Getauth = await _context.Auth.FirstOrDefaultAsync(a => a.Id == id);
            {
                Getauth.IssuedDate = auth.IssuedDate;
                Getauth.RefreshToken = auth.RefreshToken;
                Getauth.ExpiredDate = auth.ExpiredDate;
                Getauth.IsExpired = auth.IsExpired;
            }
            _context.Auth.Update(Getauth);
            _context.SaveChanges();

        }
        public async Task UpdateAccesToken(int id, string accessToken)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.Token = accessToken;
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public async Task<Auth> UpdatedAuth(User user, string refreshToken)
        {
            string RefreshToken = refreshToken;
            DateTime RefreshTokenExp = _decodeJwt.GetJwtExpiration(RefreshToken);
            DateTime RefreshTokenIssued = DateTime.UtcNow;
            bool RefreshTokenExpired = _decodeJwt.IsExpired(RefreshToken);
            bool RefreshTokenActive = _decodeJwt.IsActive(RefreshToken);
            Auth UpdatetdAUth = new Auth()
            {
                RefreshToken = RefreshToken,
                ExpiredDate = RefreshTokenExp,
                IssuedDate = RefreshTokenIssued,
                IsExpired = RefreshTokenExpired,
                IsActive = RefreshTokenActive
            };
            return UpdatetdAUth;
        }
        public async Task<Auth> GetByRefreshToken(string refreshToken)
        {
            Auth RefreshToken = await _context.Auth.FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);
            return RefreshToken;
        }

        public async Task RotateRefreshToken(User user, string refreshToken)
        {
            Auth CurrentAuth = await GetAuthDataById(user.Id);
            CurrentAuth.RefreshToken = refreshToken;
            _context.Auth.Update(CurrentAuth);
            _context.SaveChanges();
        }
    }
}
