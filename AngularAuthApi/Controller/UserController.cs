using AngularAuthApi.Data_Access;
using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using AngularAuthApi.Repository;
using AngularAuthApi.Repository.Abstract;
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
        public UserController(IUserRepository userInfoRepository, UserDbContext context)
        {
            _userInfoRepository = userInfoRepository;
            _context = context;
        }
        [HttpPut("user-info")]
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

    }
}
