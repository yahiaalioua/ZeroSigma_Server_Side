using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.DTOS;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Responses;
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

        public AuthController(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
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
            GetUser.Token= AccessToken;
            return Ok(new AuthUserResponse()
            {
                AccessToken=GetUser.Token,
                Email=GetUser.Email,
                Name=GetUser.Name
            });
        }


    }
}
