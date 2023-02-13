using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.UserServices;
using BlogApp.Utilities.JwtUtils;
using BlogApp.Utilities.MappingUtils;
using BloggingApplication.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserAuthService _userAuthService { get; }
        private IUserCrudService _userCrudService { get; }
        private IUserMapper _userMappings { get; }
        private IResponseMapper _responseMappings { get; }
        private IAuthUtils _authUtils { get; }
        public AuthController(IUserAuthService userAuthService,
                              IUserCrudService userCrudService,
                              IUserMapper userMappings,
                              IResponseMapper responseMappings,
                              IAuthUtils authUtils)
        {
            _userAuthService = userAuthService;
            _userCrudService = userCrudService;
            _userMappings = userMappings;
            _responseMappings = responseMappings;
            _authUtils = authUtils;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<ApiResponse<AuthUserInfoDto>>> Register(RegisterUserDto dto)
        {
            UserInfoDto user = await _userCrudService.FindByEmail(dto.Email);

            //Check user exists or not
            if (user != null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Email already taken. Try signing in." }));

            //Assign blogger role
            dto.Role = RoleEnum.BLOGGER;

            //Create user
            user = await _userCrudService.CreateUser(dto);
            if (user == null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "User creation failed." }));

            //Prepare dto object
            string token = await _authUtils.GenerateToken(user);
            AuthUserInfoDto result = _userMappings.Map(user, token);

            //Prepare response object and return
            ApiResponse<AuthUserInfoDto> response = _responseMappings.Map(result);
            return StatusCode(201, response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<ApiResponse<AuthUserInfoDto>>> Login(LoginUserDto dto)
        {
            UserInfoDto user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user == null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Email. Try registring first." }));

            //Verify password
            ServiceResult res = await _userAuthService.VerifyPassword(user, dto.Password);
            if(! res.Succeeded)
                return BadRequest(_responseMappings.Map(res));

            //Prepare dto object
            string token = await _authUtils.GenerateToken(user);
            AuthUserInfoDto result = _userMappings.Map(user, token);

            return Ok(_responseMappings.Map(result));
        }

        [HttpPost]
        [Route("DeleteAccount")]
        public async Task<ActionResult<ApiResponse<Message>>> DeleteAccount(LoginUserDto dto)
        {
            UserInfoDto user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user == null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Email. Try registring first." }));

            //Verify password
            ServiceResult res = await _userAuthService.VerifyPassword(user, dto.Password);
            if (!res.Succeeded)
                return BadRequest(_responseMappings.Map(res));

            res = await _userCrudService.SoftDeleteUser(dto.Email);
            if (res.Succeeded)
                return Ok(res.Messages);
            else
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "User deletion failed." }));
        }
    }
}
