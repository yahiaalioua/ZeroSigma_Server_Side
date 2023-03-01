using AngularAuthApi.Entities;
using AngularAuthApi.Entities.Requests;
using AngularAuthApi.Repository;
using AngularAuthApi.Repository.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularAuthApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        public UserInfoController(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo(UserInfoRequest userInfo)
        {
            var user=await _userInfoRepository.GetUserInfo(userInfo.Id);
            if(user == null)
            {
                return NotFound(new { Message="user not found",Code="UUIE1"});
            }
            {
                user.Id= userInfo.Id;                
                user.Linkedin= userInfo.Linkedin;
                user.YouTube = userInfo.YouTube;
                user.AboutMe = userInfo.AboutMe;
                user.Website = userInfo.Website;

            }
            await _userInfoRepository.UpdateUserInfo(user);
            return Ok(new {Message="User succesfully updated",Code="UUIS1"});
        }
    }
}
