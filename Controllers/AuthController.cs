using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.UserServices;
using BlogApp.Utilities.JwtUtils;
using BlogApp.Utilities.MappingUtils;
using BlogApp.Validations;
using BloggingApplication.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
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
        private IAuthUtils _authUtils { get; }
        public AuthController(IUserValidation userValidation,
                              IUserCrudService userCrudService,
                              IUserMapper userMappings,
                              IResponseMapper responseMappings,
                              IAuthUtils authUtils)
        {
            _userValidation = userValidation;
            _userCrudService = userCrudService;
            _userMappings = userMappings;
            _responseMappings = responseMappings;
            _authUtils = authUtils;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<ApiResponse<AuthUserInfoDto>>> Register(RegisterUserDto dto)
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
            string token = await _authUtils.GenerateToken(user);
            AuthUserInfoDto result = _userMappings.Map(user, token);

            //Prepare response object and return
            ApiResponse<UserInfoDto> response = _responseMappings.Map(result);
            return StatusCode(201, response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<ApiResponse<AuthUserInfoDto>>> Login(LoginUserDto dto)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user == null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Email. Try registring first." }));

            //Verify password
            if (! await _userValidation.ValidatePassword(user, dto.Password))
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Password." }));

            //Prepare dto object
            string token = await _authUtils.GenerateToken(user);
            AuthUserInfoDto result = _userMappings.Map(user, token);

            return Ok(_responseMappings.Map(result));
        }

        [HttpPost]
        [Route("DeleteAccount")]
        public async Task<ActionResult<ApiResponse<Message>>> DeleteAccount(LoginUserDto dto)
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
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "User deletion failed." }));
        }
    }
}
