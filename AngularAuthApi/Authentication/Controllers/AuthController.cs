using AngularAuthApi.Authentication;
using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.Authentication.Repositories.Abstract;
using AngularAuthApi.Authentication.Utilities;
using AngularAuthApi.Authentication.Utilities.Abstract;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using AngularAuthApi.Entities.Responses;
using AngularAuthApi.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AngularAuthApi.Authentication.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthRepository _userAuthRepository;
        private readonly IRefreshTokenValidate _refreshTokenValidate;
        private readonly IAuthenticator _authenticator;
        private readonly ITokenRepository _tokenRepository;


        public AuthController(IUserAuthRepository userRepository,
            ITokenRepository tokenRepository, IRefreshTokenValidate refreshTokenValidate,
            IAuthenticator authenticator)
        {
            _userAuthRepository = userRepository;
            _tokenRepository = tokenRepository;
            _refreshTokenValidate = refreshTokenValidate;
            _authenticator = authenticator;
        }


        [HttpPost("new")]
        public async Task<IActionResult> RegisterUser(SignUpRequest user)
        {

            if (user == null)
            {
                return BadRequest();
            }
            if (await _userAuthRepository.CheckEmailExist(user.Email))
            {
                return BadRequest(new { Message = "Email already exist", code = 4 });
            }
            var passToCheck = _userAuthRepository.CheckPasswordStrength(user.Password);
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
            await _userAuthRepository.RegisterUser(newUser);
            return Ok(new { Message = "You succesfully registered", code = 3 });
        }


        [HttpPost("session")]
        public async Task<IActionResult> Authenticate(LoginDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var GetUser = await _userAuthRepository.AuthenticateUser(user);

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

        [HttpPost("session/token")]
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
            User user = await _userAuthRepository.GetUserById(refreshAuthDto.Id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found", Code = "Auth:0072" });
            }
            AuthUserResponse resp = await _authenticator.AuthenticateRefreshToken(user);

            return Ok(resp);

        }
        [Authorize]
        [HttpPost("logout")]

        public async Task<IActionResult> Logout(int id)
        {
            var user = await _userAuthRepository.GetUserById(id);
            var auth = await _tokenRepository.GetAuthDataById(id);
            if(user == null)
            {
                return NotFound(new { Message = "User not found", Code = "Auth:0072" });
            }
            await _userAuthRepository.Logout(user,auth);
            return Ok(new { Message = "User succesfully logged out", Code = "Auth:0074"});
        }

    }
}
