﻿using AngularAuthApi.Authentication.Repositories.Abstract;
using AngularAuthApi.Data_Access;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using AngularAuthApi.Entities.Responses;
using AngularAuthApi.Repository;
using AngularAuthApi.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthApi.Controller
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userInfoRepository;
        private readonly UserDbContext _context;
        private readonly ITokenRepository _tokenRepository;
        public UserController(IUserRepository userInfoRepository, UserDbContext context, ITokenRepository tokenRepository)
        {
            _userInfoRepository = userInfoRepository;
            _context = context;
            _tokenRepository = tokenRepository;
        }
        [Authorize]
        [HttpPut("account/info")]
        public async Task<IActionResult> UpdateUserInfo(UserInfoRequest userInfo)
        {
            var user = await _userInfoRepository.GetUserInfo(userInfo.Id);
            if (user == null)
            {
                return NotFound(new { Message = "user not found", Code = "UUIE1" });
            }
            {
                user.Id = userInfo.Id;
                user.Linkedin = userInfo.Linkedin;
                user.YouTube = userInfo.YouTube;
                user.AboutMe = userInfo.AboutMe;
                user.Website = userInfo.Website;

            }
            await _userInfoRepository.UpdateUserInfo(user);
            return Ok(new { Message = "User succesfully updated", Code = "UUIS1" });
        }

        [Authorize]
        [HttpPut("email/{id:int}")]
        public async Task<IActionResult> PutUserEmail(int id, [FromBody]string email)
        {
            var user= await _userInfoRepository.GetUser(id);
            if(user == null)
            {
                return NotFound(new { Message = "user not found", Code = "UUE1" });
            }            
            if (await _context.Users.AnyAsync(user => user.Email == email))
            {
                return BadRequest(new {Message="this email is already in use",Code="UUE2"});
            }
            user.Email = email;
            await _userInfoRepository.UpdateUser(user);
            return Ok(new { Message = "user email succesfully updated" });

        }
        [Authorize]
        [HttpPut("name/{id:int}")]
        public async Task<IActionResult> PutUserName(int id, [FromBody]string name)
        {
            var user = await _userInfoRepository.GetUser(id);
            if (user == null)
            {
                return NotFound(new { Message = "user not found", Code = "UUN1" });
            }
            //flere bruker med sammen navn er tillatt så vi trenger ikke å sjekke i db om det finnes samme navn
            user.Name = name;
            await _userInfoRepository.UpdateUser(user);
            return Ok(new { Message = "user name succesfully updated" });

        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userInfoRepository.GetUser(id);
            if (user == null)
            {
                return NotFound(new { Message = "user not found", Code = "Auth:0072" });
            }
            await _userInfoRepository.DeleteUser(user);
            return Accepted(new { Message = "User succesfully delated", Code = "Auth:0073" });
        }
        [Authorize]
        [HttpGet("account/info/{id:int}")]
        public async Task<IActionResult> GetUserData(int id)
        {
            var user= await _userInfoRepository.GetUser(id);
            if(user == null)
            {
                return NotFound(new {Message="User not found", Code = "Auth:0072" });
            }
            user.Auth=await _tokenRepository.GetAuthDataById(id);
            user.UserInfo=await _userInfoRepository.GetUserInfo(id);
            UserResponse userResponse = new()
            {
                Id=user.Id, Name=user.Name, Email=user.Email,
                YouTube=user.UserInfo.YouTube,
                Linkedin=user.UserInfo.Linkedin,Website=user.UserInfo.Website,
                AboutMe=user.UserInfo.AboutMe,
            };
            return Ok(userResponse);
        }

    }
}
