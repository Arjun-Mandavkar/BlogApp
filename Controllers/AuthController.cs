using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.MappingServices;
using BlogApp.Services.UserServices;
using BlogApp.Validations;
using BloggingApplication.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Transactions;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserValidation _userValidation { get; }
        private IUserCrudService _userCrudService { get; }
        private IUserMapper _userMappings { get; }
        private IResponseMapper _responseMappings { get; }

        public AuthController(IUserValidation userValidation,
                              IUserCrudService userCrudService,
                              IUserMapper userMappings,
                              IResponseMapper responseMappings)
        {
            _userValidation = userValidation;
            _userCrudService = userCrudService;
            _userMappings = userMappings;
            _responseMappings = responseMappings;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<ApiResponse>> Register(RegisterUserDto dto)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user != null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Email already taken. Try signing in." }));

            using (var tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //Assign blogger role
                dto.Role = RoleEnum.BLOGGER;

                //Create user
                user = await _userCrudService.CreateUser(dto);
                if(user == null)
                    return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "User creation failed." }));

                tr.Complete();
            }

            //Prepare dto object
            AuthUserInfoDto result = await _userMappings.MapAuthUser(user);

            //Prepare response object and return
            ApiResponse response = _responseMappings.Map(result);
            return StatusCode(201, response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<ApiResponse>> Login(LoginUserDto dto)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user == null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Email. Try registring first." }));

            //Verify password
            if (! await _userValidation.ValidatePassword(user, dto.Password))
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Password." }));

            //Prepare dto and return response
            AuthUserInfoDto result = await _userMappings.MapAuthUser(user);

            return Ok(_responseMappings.Map(result));
        }

        [HttpPost]
        [Route("DeleteAccount")]
        public async Task<ActionResult<ApiResponse>> DeleteAccount(LoginUserDto dto)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user == null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Email. Try registring first." }));

            //Verify password
            if (!await _userValidation.ValidatePassword(user, dto.Password))
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Password." }));

            ServiceResult res = await _userCrudService.SoftDeleteUser(dto.Email);
            if (res.Succeeded)
                return Ok(res.Messages);

            else
                return _responseMappings.Map(new Message { Code = "Error", Description = "User deletion failed." });
        }
    }
}
