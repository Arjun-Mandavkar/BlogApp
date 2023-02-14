using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Models.ServiceObjects;
using BlogApp.Services.UserServices;
using BlogApp.Utilities.MappingUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMIN")]
    public class UserController : ControllerBase
    {
        private IUserCrudService _userCrudService { get; }
        private IResponseMapper _responseMapper { get; }
        private IServiceObjectMapper _serviceObjectMapper { get; }

        public UserController(IUserCrudService userCrudService,
                              IResponseMapper responseMapper,
                              IServiceObjectMapper serviceObjectMapper)
        {
            _userCrudService = userCrudService;
            _responseMapper = responseMapper;
            _serviceObjectMapper = serviceObjectMapper;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetById(string userId)
        {
            UserServiceObject user = await _userCrudService.FindById(userId);
            if (user != null)
            {
                UserInfoDto dto = _serviceObjectMapper.Map(user);
                return Ok(_responseMapper.Map(dto));
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
            UserServiceObject user = await _userCrudService.FindByEmail(email);
            if (user != null)
            {
                UserInfoDto dto = _serviceObjectMapper.Map(user);
                return Ok(_responseMapper.Map(dto));
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
            UserServiceObject user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user != null)
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "Email already taken." }));

            //Create user
            user = await _userCrudService.CreateUser(dto);
            if (user == null)
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "User creation failed." }));

            UserInfoDto userDto = _serviceObjectMapper.Map(user);

            return StatusCode(201, _responseMapper.Map(userDto));
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
