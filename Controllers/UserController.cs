using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.BlogServices;
using BlogApp.Services.MappingServices;
using BlogApp.Services.UserServices;
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
        private IBlogOwnerService _blogOwnerService { get; }
        private IBlogCommentService _blogCommentService { get; }
        private IUserMapper _userMapper { get; }
        private IResponseMapper _responseMapper { get; }

        public UserController(IUserCrudService userCrudService, IUserMapper userMapper)
        {
            _userCrudService = userCrudService;
            _userMapper = userMapper;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<ApiResponse>> GetById(string userId)
        {
            ApplicationUser user = await _userCrudService.FindById(userId);
            if (user == null)
            {
                UserInfoDto userDto = _userMapper.Map(user);
                return Ok(_responseMapper.Map(userDto));
            }
            else
            {
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "Invalid user id." }));
            }
            
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<ActionResult<ApiResponse>> GetByEmail(string email)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(email);
            if (user == null)
            {
                UserInfoDto userDto = _userMapper.Map(user);
                return Ok(_responseMapper.Map(userDto));
            }
            else
            {
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "Invalid email address." }));
            }

        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<ApiResponse>> Create(RegisterUserDto dto)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user != null)
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "Email already taken." }));

            using (var tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //Create user
                user = await _userCrudService.CreateUser(dto);
                if (user == null)
                    return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "User creation failed." }));

                tr.Complete();
            }

            UserInfoDto userDto = _userMapper.Map(user);
            return StatusCode(201, _responseMapper.Map(userDto)); 
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<ApiResponse>> Update(RegisterUserDto dto)
        {
            ServiceResult result = await _userCrudService.UpdateUser(_userMapper.Map(dto));
            ApiResponse response = _responseMapper.Map(result);
            return result.Succeeded == true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult<ApiResponse>> Delete(int userId)
        {
            ApplicationUser user = await _userCrudService.FindById(userId.ToString());

            //Chech user exists or not
            if (user == null)
                return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "User not found." }));

            using (var tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //Delete the user
                ServiceResult res = await _userCrudService.DeleteUser(user);
                if (! res.Succeeded)
                    return BadRequest(_responseMapper.Map(new Message { Code = "Error", Description = "Invalid user id." }));

                //set isOwnerExists false in owners table
                if (!await _blogOwnerService.UpdateOwnerEntryForUserDeletion(user))
                    return StatusCode(200, _responseMapper.Map(new Message { Code = "Error", Description = "Failed to update owner table." },
                                                               new Message { Code = "Message", Description = "User deleted successfully." }));

                //Set isUserExists false in comments table
                if (! await _blogCommentService.UpdateCommentForUserDeletion(user))
                    return StatusCode(200, _responseMapper.Map(new Message { Code = "Error", Description = "Failed to update comment table." },
                                                               new Message { Code = "Message", Description = "User deleted successfully." }));

                tr.Complete();
            }

            UserInfoDto userDto = _userMapper.Map(user);
            return Ok(_responseMapper.Map(userDto));
        }
    }
}
