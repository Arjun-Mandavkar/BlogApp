using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.BlogServices;
using BlogApp.Services.UserServices;
using BlogApp.Utilities.MappingUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private IResponseMapper _responseMapper { get; }

        public UserController(IUserCrudService userCrudService, IUserMapper userMapper)
        {
            _userCrudService = userCrudService;
            _userMapper = userMapper;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetById(string userId)
        {
            UserInfoDto user = await _userCrudService.FindById(userId);
            if (user != null)
            {
                return Ok(_responseMapper.Map(user));
            }
            else
            {
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "Invalid user id." }));
            }

        }

        [HttpGet]
        [Route("{email}")]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetByEmail(string email)
        {
            UserInfoDto user = await _userCrudService.FindByEmail(email);
            if (user != null)
            {
                return Ok(_responseMapper.Map(user));
            }
            else
            {
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "Invalid email address." }));
            }

        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> Create(RegisterUserDto dto)
        {
            UserInfoDto user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user != null)
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "Email already taken." }));

            //Create user
            user = await _userCrudService.CreateUser(dto);
            if (user == null)
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "User creation failed." }));

            return StatusCode(201, _responseMapper.Map(user));
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<ApiResponse<List<Message>>>> Update(RegisterUserDto dto)
        {
            ServiceResult result = await _userCrudService.UpdateUser(dto);
            ApiResponse<List<Message>> response = _responseMapper.Map(result);
            return result.Succeeded == true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> Delete(string email)
        {
            //Soft Delete the user
            ServiceResult res = await _userCrudService.SoftDeleteUser(email);
            if (!res.Succeeded)
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "User deletion failed." }));

            return Ok(_responseMapper.Map(new Message { Code = "Error", Description = "User deleted successfully." }));
        }
    }
}
