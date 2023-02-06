using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.BlogServices;
using BlogApp.Services.MappingServices;
using BlogApp.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Transactions;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
    public class UserController : ControllerBase
    {
        private IUserCrudService _userCrudService { get; }
        private IUserMapper _userMapper { get; }
        private IBlogOwnerService _blogOwner { get; }
        private IBlogCommentService _blogCommentService { get; }

        public UserController(IUserCrudService userCrudService, IUserMapper userMapper)
        {
            _userCrudService = userCrudService;
            _userMapper = userMapper;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<UserInfoDto>> GetById(string userId)
        {
            ApplicationUser user = await _userCrudService.FindById(userId);
            return Ok(_userMapper.Map(user));
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<ActionResult<UserInfoDto>> GetByEmail(string email)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(email);
            return Ok(_userMapper.Map(user));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<UserInfoDto>> Create(RegisterUserDto dto)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user != null)
                return BadRequest("Email already taken.");

            using (var tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //Create user
                user = await _userCrudService.CreateUser(dto);
                if (user == null)
                    return BadRequest("User creation failed.");

                tr.Complete();
            }
            return StatusCode(201, _userMapper.Map(user)); 
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult<UserInfoDto>> Delete(int userId)
        {
            ApplicationUser user = await _userCrudService.FindById(userId.ToString());

            //Chech user exists or not
            if (user == null)
                return BadRequest("User not found.");

            using (var tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //Delete the user
                ServiceResult res = await _userCrudService.DeleteUser(user);
                if (! res.Succeeded)
                    return BadRequest("Invalid user id.");

                //set isOwnerExists false in owners table
                if (!await _blogOwner.UpdateOwnerEntryForUserDeletion(user))
                    return StatusCode(500, "Failed to update owner table.");

                //Set isUserExists false in comments table
                if (! await _blogCommentService.UpdateCommentForUserDeletion(user))
                    return StatusCode(500, "Failed to update comment table.");

                tr.Complete();
            }
            return Ok(_userMapper.Map(user));
        }
    }
}
