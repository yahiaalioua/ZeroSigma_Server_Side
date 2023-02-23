using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.Authentication;
using AngularAuthApi.Data_Access;
using AngularAuthApi.DTOS;
using AngularAuthApi.Entities;
using AngularAuthApi.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using AngularAuthApi.Entities.Requests;

namespace AngularAuthApi.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly UserDbContext _context;
     
        private readonly IDecodeJwt _decodeJwt;

        public TokenRepository(UserDbContext context, IDecodeJwt decodeJwt)
        {
            _context = context;
            _decodeJwt = decodeJwt;
        }
        public async Task Create(Auth auth)
        {
            await _context.Auth.AddAsync(auth);
            await _context.SaveChangesAsync();
        }
        public async Task<Auth> GetAuthDataById(int id)
        {
            Auth auth= await _context.Auth.FirstOrDefaultAsync(a=>a.Id == id);
            return auth;
        }
        public async Task UpdateAuth(int id, Auth auth)
        {
            Auth Getauth= await _context.Auth.FirstOrDefaultAsync(a => a.Id == id);
            { Getauth.IssuedDate = auth.IssuedDate;
                Getauth.RefreshToken = auth.RefreshToken;
                Getauth.ExpiredDate= auth.ExpiredDate;
                Getauth.IsExpired = auth.IsExpired;
            }
            _context.Auth.Update(Getauth);
            _context.SaveChanges();
            
        }
        public async Task <Auth>UpdatedAuth(User user,string refreshToken)
        {
            string RefreshToken = refreshToken;
            DateTime RefreshTokenExp = _decodeJwt.GetJwtExpiration(RefreshToken);
            DateTime RefreshTokenIssued = _decodeJwt.GetJwtIssued(RefreshToken);
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
        public async Task<Auth>GetByRefreshToken(string refreshToken)
        {
            Auth RefreshToken=await _context.Auth.FirstOrDefaultAsync(a=>a.RefreshToken==refreshToken);
            return RefreshToken;
        }        
    }
}
