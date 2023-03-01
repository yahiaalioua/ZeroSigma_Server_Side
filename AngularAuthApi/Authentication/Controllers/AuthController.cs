using AngularAuthApi.Authentication;
using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.Authentication.DTOS;
using AngularAuthApi.Authentication.Repositories.Abstract;
using AngularAuthApi.Authentication.Utilities;
using AngularAuthApi.Authentication.Utilities.Abstract;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using AngularAuthApi.Entities.Responses;
using AngularAuthApi.Repository.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AngularAuthApi.Authentication.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenValidate _refreshTokenValidate;
        private readonly IAuthenticator _authenticator;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserInfoRepository _userInfoRepository;


        public AuthController(IUserRepository userRepository,
            ITokenRepository tokenRepository, IRefreshTokenValidate refreshTokenValidate,
            IAuthenticator authenticator, IUserInfoRepository userInfoRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _refreshTokenValidate = refreshTokenValidate;
            _authenticator = authenticator;
            _userInfoRepository = userInfoRepository;
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
            AuthUserResponse response = await _authenticator.AuthenticateResponse(user, GetUser);


            return Ok(response);
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
            AuthUserResponse resp = await _authenticator.AuthenticateRefreshToken(user);

            return Ok(resp);

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { Message = "user not found", Code = "Auth:0072" });
            }
            await _userRepository.DeleteUser(user);
            return Ok(new { Message = "User succesfully delated", Code = "Auth:0073" });
        }
        [HttpGet]
        public async Task<IActionResult> GetUserData(int id)
        {
            var user= await _userRepository.GetUserById(id);
            if(user == null)
            {
                return NotFound(new {Message="user not found",Code="Auth:0074"});
            }
            user.Auth=await _tokenRepository.GetAuthDataById(id);
            user.UserInfo=await _userInfoRepository.GetUserInfo(id);
            UserResponse UserResponse = new()
            {
                Id=user.Id, Name=user.Name, Email=user.Email,
                YouTube=user.UserInfo.YouTube,
                Linkedin=user.UserInfo.Linkedin,Website=user.UserInfo.Website,
                AboutMe=user.UserInfo.AboutMe,
            };
            return Ok(UserResponse);
        }
    }
}
