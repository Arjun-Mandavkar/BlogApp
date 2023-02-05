﻿using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.MappingServices;
using BlogApp.Services.UserServices;
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

        private PasswordHasher<ApplicationUser> _hasher { get; }
        private IUserAuthService _userAuthService { get; }
        private IUserCrudService _userCrudService { get; }
        private IUserMapper _userMappings { get; }
        private IResponseMapper _responseMappings { get; }

        public AuthController(IUserAuthService userAuthService, IUserCrudService userCrudService, IUserMapper userMappings)
        {
            _hasher = new PasswordHasher<ApplicationUser>();
            _userAuthService = userAuthService;
            _userCrudService = userCrudService;
            _userMappings = userMappings;
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
            return StatusCode(201, _responseMappings.Map(result));
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
            if (! await _userAuthService.IsPasswordCorrect(user, dto.Password))
                return BadRequest(_responseMappings.Map(new Message { Code = "Error", Description = "Invalid Password." }));

            //Prepare dto and return response
            AuthUserInfoDto result = await _userMappings.MapAuthUser(user);

            return Ok(_responseMappings.Map(result));
        }
    }
}