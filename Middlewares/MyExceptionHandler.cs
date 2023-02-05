﻿using BlogApp.MappingServices;
using BlogApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloggingApplication.Services
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MyExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly string _path;
        private ExceptionToResponseMapper _mapper = new ExceptionToResponseMapper();

        public MyExceptionHandler(RequestDelegate next, string path)
        {
            _next = next;
            _path = path;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //"/Error"  "/ErrorDevEnv"

            Console.WriteLine("_________Inside MyExceptionHandler middleware.");
            try
            {
                await _next(httpContext);
                Console.WriteLine("_________Exited MyExceptionHandler: without exception.");
            }
            catch(Exception exception) 
            {
                Console.WriteLine("_________Exception caught in MyExceptionHandler.");
                httpContext.Response.StatusCode = ExceptionStatusCodeMapper(exception);
                httpContext.Response.ContentType = "application/json";

                if (_path == "/Error")
                {
                    await httpContext.Response.WriteAsJsonAsync(_mapper.Map(exception));
                    Console.WriteLine("_________Response returned from MyExceptionHandler.");
                }
                else if (_path == "/ErrorDevEnv")
                {
                    ApiResponse apiResponse = _mapper.Map(exception);
                    apiResponse.Data.Append(new Message { Code="StackTrace",Description=exception.ToString()});
                    await httpContext.Response.WriteAsJsonAsync(apiResponse);
                    Console.WriteLine("_________Response returned from MyExceptionHandler. [Dev Env]");
                }
                else
                {
                    throw new Exception($"Invalid path parameter for MyException handler: '{_path}'. It should be either'/Error' or '/ErrorDevEnv'.");
                }
            }
            
        }
        private int ExceptionStatusCodeMapper(Exception exception)
        {
            if (exception is SqlException) return (int)HttpStatusCode.BadRequest;
            else if (exception is InvalidOperationException) return (int)HttpStatusCode.BadRequest;
            else return (int)HttpStatusCode.InternalServerError;    // Internal Server Error by default
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseMyExceptionHandler(this IApplicationBuilder builder, string path)
        {
            Console.WriteLine("_________MyExceptionHandler middleware added to container.");
            return builder.UseMiddleware<MyExceptionHandler>(path);
        }
    }
}
