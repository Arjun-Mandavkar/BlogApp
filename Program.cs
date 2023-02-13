using BlogApp.DbConnection.Implementations;
using BlogApp.DbConnection;
using BlogApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogApp.Models;
using BlogApp.Services.UserServices;
using BlogApp.Services.UserServices.Implementations;
using BlogApp.Repositories.Implementations;
using BlogApp.Repositories;
using BlogApp.Validations;
using BlogApp.Validations.Implementations;
using BlogApp.Services.BlogServices;
using BlogApp.Services.BlogServices.Implementation;
using BlogApp.Utilities.MappingUtils;
using BlogApp.Utilities.MappingUtils.Implementations;
using BlogApp.Utilities.JwtUtils;
using BlogApp.Utilities.JwtUtils.Implementations;

namespace BlogApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration.GetSection("Jwt:Secret").Value
                                )
                            ),
                        //https://stackoverflow.com/questions/70597009/what-is-the-meaning-of-validateissuer-and-validateaudience-in-jwt
                        //https://stackoverflow.com/questions/61976960/asp-net-core-jwt-authentication-always-throwing-401-unauthorized
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetSection("AuthSettings:Issuer").Value,
                        ValidateAudience = false,
                        ValidAudience = builder.Configuration.GetSection("AuthSettings:Audience").Value

                    };
                });

            builder.Services.AddControllers();

            //Auxiliary objects
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactoryImpl>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddTransient<PasswordHasher<ApplicationUser>>();

            //Services should be of scope singleton. Services should be stateless, and hence they don't need more than one instance.
            //User services
            builder.Services.AddSingleton<IUserCrudService, UserCrudService>();
            builder.Services.AddSingleton<IUserRoleService, UserRoleService>();

            //Blog services
            builder.Services.AddSingleton<IBlogCrudService, BlogCrudService>();
            builder.Services.AddSingleton<IBlogEditorService, BlogEditorService>();
            builder.Services.AddSingleton<IBlogOwnerService, BlogOwnerService>();
            builder.Services.AddSingleton<IBlogRolesService, BlogRolesService>();
            builder.Services.AddSingleton<IBlogCommentService, BlogCommentService>();
            builder.Services.AddSingleton<IBlogLikeService, BlogLikeService>();

            //Utility services
            //Mapping utils
            builder.Services.AddSingleton<IUserMapper, UserMapper>();
            builder.Services.AddSingleton<IBlogMapper, BlogMapper>();
            builder.Services.AddSingleton<IXResultServiceMapper, XResultServiceMapper>();
            builder.Services.AddSingleton<IResponseMapper, ResponseMapper>();
            builder.Services.AddSingleton<IServiceObjectMapper, ServiceObjectMapper>();

            //Jwt utils (Authentication)
            builder.Services.AddTransient<IAuthUtils, AuthUtils>();

            //Validations
            builder.Services.AddSingleton<IUserValidation, UserValidation>();
            builder.Services.AddSingleton<IBlogValidation, BlogValidation>();
            builder.Services.AddSingleton<IBlogRoleValidation, BlogRoleValidation>();

            //Repository objects should be transient as they are using dbConnection objects
            builder.Services.AddTransient<IUserStore<ApplicationUser>, UserStoreImpl>();
            builder.Services.AddTransient<IRoleStore<IdentityRole>, RoleStoreImpl>();
            builder.Services.AddTransient<IMyUserStore, UserStoreImpl>();
            builder.Services.AddTransient<IBlogStore<Blog>, BlogStoreImpl>();
            builder.Services.AddTransient<IBlogLikesStore<Blog, ApplicationUser>, BlogLikesStoreImpl>();
            builder.Services.AddTransient<IBlogCommentsStore<BlogComment>, BlogCommentsStoreImpl>();
            builder.Services.AddTransient<IBlogOwnersStore<BlogOwner>, BlogOwnersStoreImpl>();
            builder.Services.AddTransient<IBlogEditorsStore<Blog, ApplicationUser>, BlogEditorsStoreImpl>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMyExceptionHandler("/ErrorDevEnv", new ResponseMapper());
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseMyExceptionHandler("/Error", new ResponseMapper());
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}