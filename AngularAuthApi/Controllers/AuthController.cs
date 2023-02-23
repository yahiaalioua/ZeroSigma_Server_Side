using AngularAuthApi.Authentication;
using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.DTOS;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using AngularAuthApi.Entities.Responses;
using AngularAuthApi.Repositories;
using AngularAuthApi.Repositories.Abstract;
using AngularAuthApi.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AngularAuthApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IRefreshTokenValidate _refreshTokenValidate;

        private readonly ITokenRepository _tokenRepository;


        public AuthController(IUserRepository userRepository,
            IJwtProvider jwtProvider, IRefreshTokenProvider refreshTokenProvider,
            ITokenRepository tokenRepository, IRefreshTokenValidate refreshTokenValidate)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _tokenRepository = tokenRepository;
            _refreshTokenValidate = refreshTokenValidate;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(SignUpRequest user)
        {

            if (user == null)
            {
                return BadRequest();
            }
            if (await _userRepository.CheckEmailExist(user.Email))
            {
                return BadRequest(new { Message = "Email already exist", code = 4 });
            }
            var passToCheck = _userRepository.CheckPasswordStrength(user.Password);
            if (!string.IsNullOrEmpty(passToCheck))
            {
                return BadRequest(new { Message = passToCheck.ToString() });
            }
            User newUser = new()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
            await _userRepository.RegisterUser(newUser);
            return Ok(new { Message = "You succesfully registered", code = 3 });
        }


        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(LoginDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var GetUser = await _userRepository.AuthenticateUser(user);

            if (GetUser == null)
            {
                return NotFound(new { Message = "User do not exist", code = 2 });
            }
            if (!PasswordHasher.VerifyPassword(user.Password, GetUser.Password, Convert.FromHexString(GetUser.Salt)))
            {
                return BadRequest(new { Message = "Wrong password", code = 1 });
            }
            string AccessToken = _jwtProvider.GenerateToken(user);
            string RefreshToken = _refreshTokenProvider.GenerateRefreshToken(GetUser);
            GetUser.Token = AccessToken;
            Auth auth = await _tokenRepository.GetAuthDataById(GetUser.Id);
            Auth updatedAuth = await _tokenRepository.UpdatedAuth(GetUser, RefreshToken);
            await _tokenRepository.UpdateAuth(auth.Id, updatedAuth);
           

            return Ok(new AuthUserResponse()
            {
                AccessToken = GetUser.Token,
                Payload = new Payload() { Name = GetUser.Name, Email = GetUser.Email },
                RefreshToken = RefreshToken,

            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshAuthRequest refreshAuthRequest)
        {
            
            bool isValidRefreshToken = _refreshTokenValidate.Validate(refreshAuthRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new { Message = "Invalid Refresh Token", Code = "Auth:0070" });
            }
            Auth refreshAuthDto = await _tokenRepository.GetByRefreshToken(refreshAuthRequest.RefreshToken);
            if (refreshAuthDto == null)
            {
                return BadRequest(new { Message = "Refresh Token Not Found", Code = "Auth:0071" });
            }
            User user = await _userRepository.GetUserById(refreshAuthDto.Id);
            if (user == null)
            {
                return NotFound(new { Message = "User do not exist", code = 2 });
            }
            string AccessToken = _jwtProvider.GenerateToken(new LoginDto() { Email = user.Email, Password = user.Password });
            string RefreshToken = _refreshTokenProvider.GenerateRefreshToken(user);

            user.Token = AccessToken;
            return Ok(new AuthUserResponse()
            {
                AccessToken = user.Token,
                Payload = new Payload() { Name = user.Name, Email = user.Email },
                RefreshToken = RefreshToken,

            });


        }
    }
}
