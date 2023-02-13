using System.Security.Claims;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Models.ServiceObjects;
using BlogApp.Services.UserServices;
using BlogApp.Utilities.JwtUtils;
using BlogApp.Utilities.MappingUtils;
using BlogApp.Validations;
using BloggingApplication.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserCrudService _userCrudService { get; }
        private IResponseMapper _responseMappings { get; }
        private IAuthUtils _authUtils { get; }
        private IServiceObjectMapper _serviceObjectMapper { get; }
        private IUserValidation _userValidation { get; }

        public AuthController(IUserCrudService userCrudService,
                              IResponseMapper responseMappings,
                              IAuthUtils authUtils,
                              IServiceObjectMapper serviceObjectMapper,
                              IUserValidation userValidation)
        {
            _userCrudService = userCrudService;
            _responseMappings = responseMappings;
            _authUtils = authUtils;
            _serviceObjectMapper = serviceObjectMapper;
            _userValidation = userValidation;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<ApiResponse<AuthUserInfoDto>>> Register(RegisterUserDto dto)
        {
            UserServiceObject user = await _userCrudService.FindByEmail(dto.Email);

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
            string token = await _authUtils.GenerateToken(new List<Claim>()
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Email", user.Email),
                        new Claim("Name", user.Name),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    });

            AuthUserInfoDto result = _serviceObjectMapper.Map(user, token);

            //Prepare response object and return
            ApiResponse<AuthUserInfoDto> response = _responseMappings.Map(result);
            return StatusCode(201, response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<ApiResponse<AuthUserInfoDto>>> Login(LoginUserDto dto)
        {
            UserServiceObject user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user == null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Email. Try registring first." }));

            //Verify password
            bool res = await _userValidation.ValidatePassword(user.PasswordHash, dto.Password);
            if(! res)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Password incorrect"}));

            //Prepare dto object
            string token = await _authUtils.GenerateToken(new List<Claim>()
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Email", user.Email),
                        new Claim("Name", user.Name),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    });
            AuthUserInfoDto result = _serviceObjectMapper.Map(user, token);

            return Ok(_responseMappings.Map(result));
        }

        [HttpPost]
        [Route("DeleteAccount")]
        public async Task<ActionResult<ApiResponse<Message>>> DeleteAccount(LoginUserDto dto)
        {
            UserServiceObject user = await _userCrudService.FindByEmail(dto.Email);

            //Chech user exists or not
            if (user == null)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Email. Try registring first." }));

            //Verify password
            bool res = await _userValidation.ValidatePassword(user.PasswordHash, dto.Password);
            if (!res)
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Password incorrect" }));

            ServiceResult result = await _userCrudService.SoftDeleteUser(dto.Email);
            if (result.Succeeded)
                return Ok(_responseMappings.MapMessages(result.Messages.ToArray()));
            else
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "User deletion failed." }));
        }
    }
}
