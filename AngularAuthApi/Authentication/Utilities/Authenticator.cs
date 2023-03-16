using AngularAuthApi.Authentication.Abstractions;

using AngularAuthApi.Entities.Responses;
using AngularAuthApi.Entities;
using AngularAuthApi.Authentication.Utilities.Abstract;
using AngularAuthApi.Authentication.Repositories.Abstract;
using AngularAuthApi.Entities.Requests;

namespace AngularAuthApi.Authentication.Utilities
{
    public class Authenticator : IAuthenticator
    {
        private readonly IUserAuthRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IRefreshTokenValidate _refreshTokenValidate;

        private readonly ITokenRepository _tokenRepository;


        public Authenticator(IUserAuthRepository userRepository,
            IJwtProvider jwtProvider, IRefreshTokenProvider refreshTokenProvider,
            ITokenRepository tokenRepository, IRefreshTokenValidate refreshTokenValidate)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _tokenRepository = tokenRepository;
            _refreshTokenValidate = refreshTokenValidate;
        }

        public async Task<AuthUserResponse> AuthenticateResponse(LoginDto user, User GetUser)
        {
            string AccessToken = _jwtProvider.GenerateToken(user);
            string RefreshToken = _refreshTokenProvider.GenerateRefreshToken(GetUser);
            GetUser.Token = AccessToken;
            Auth auth = await _tokenRepository.GetAuthDataById(GetUser.Id);
            Auth updatedAuth = await _tokenRepository.UpdatedAuth(GetUser, RefreshToken);
            await _tokenRepository.UpdateAuth(auth.Id, updatedAuth);


            return new AuthUserResponse()
            {
                
                AccessToken = GetUser.Token,
                Payload = new Payload() { Id = GetUser.Id, Name = GetUser.Name, Email = GetUser.Email },
                RefreshToken = RefreshToken,

            };
        }
        public async Task<AuthUserResponse> AuthenticateRefreshToken(User user)
        {
            string AccessToken = _jwtProvider.GenerateToken(new LoginDto() { Email = user.Email, Password = user.Password });
            string RefreshToken = _refreshTokenProvider.GenerateRefreshToken(user);
            await _tokenRepository.UpdateAccesToken(user.Id, AccessToken);
            await _tokenRepository.RotateRefreshToken(user, RefreshToken);
            user.Token = AccessToken;
            return new AuthUserResponse()
            {
                AccessToken = user.Token,
                Payload = new Payload() { Name = user.Name, Email = user.Email },
                RefreshToken = RefreshToken,

            };
        }
    }
}
