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
        private readonly IDecodeJwt _decodeJwt;

        public AuthController(IUserRepository userRepository,
            IJwtProvider jwtProvider, IRefreshTokenProvider refreshTokenProvider,
            IRefreshTokenValidate refreshTokenValidate, ITokenRepository tokenRepository,
            IDecodeJwt decodeJwt)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _refreshTokenValidate = refreshTokenValidate;
            _tokenRepository = tokenRepository;
            _decodeJwt = decodeJwt;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            
            if (user == null)
            {
                return BadRequest();
            }
            if(await _userRepository.CheckEmailExist(user.Email))
            {
                return BadRequest(new { Message = "Email already exist", code = 4});
            }
            var passToCheck=_userRepository.CheckPasswordStrength(user.Password);
            if(!string.IsNullOrEmpty(passToCheck))
            {
                return BadRequest(new {Message=passToCheck.ToString()});
            }
            await _userRepository.RegisterUser(user);
            return Ok(new {Message="You succesfully registered", code = 3});
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
            if(!PasswordHasher.VerifyPassword(user.Password, GetUser.Password, Convert.FromHexString(GetUser.Salt)))
            {
                return BadRequest(new { Message = "Wrong password", code = 1 });
            }
            string AccessToken = _jwtProvider.GenerateToken(user);
            string RefreshToken = _refreshTokenProvider.GenerateRefreshToken(GetUser);
            DateTime RefreshTokenExp = _decodeJwt.GetJwtExpiration(RefreshToken);
            _tokenRepository.Create(new Auth() { Id=GetUser.Id,RefreshToken=RefreshToken,ExpiredTime=RefreshTokenExp});
            GetUser.Token= AccessToken;
            return Ok(new AuthUserResponse()
            {
                AccessToken=GetUser.Token,
                Payload = new Payload() {Name=GetUser.Name,Email=GetUser.Email },
                RefreshToken=RefreshToken,
                
            });
        }

        /*[HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshAuthRequest refreshAuthRequest)
        {
            bool isValidRefreshToken = _refreshTokenValidate.Validate(refreshAuthRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new { Message = "Invalid Refresh Token",Code="Auth:0070" });
            }

        }*/


    }
}
