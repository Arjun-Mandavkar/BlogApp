using BlogApp.MappingServices;
using BlogApp.MappingServices.Implementations;
using BloggingApplication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMyExceptionHandler("/ErrorDevEnv", new ExceptionToResponseMapper());
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseMyExceptionHandler("/Error", new ExceptionToResponseMapper());
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