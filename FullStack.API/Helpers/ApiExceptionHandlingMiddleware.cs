using FullStack.API.Exceptions;
using FullStack.API.Helpers.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FullStack.API.Helpers
{
    public class ApiExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ApiExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ApiException ex)
            {
                if (ex is ValidationApiException)
                {
                    await HandleValidationExceptionAsync(context, ex as ValidationApiException);
                }
                if (ex is DuplicateUserApiException)
                {
                    await HandleDuplicateUserExceptionAsync(context, ex as DuplicateUserApiException);
                }
                if (ex is UnauthorizedApiException)
                {
                    await HandleUnauthorizedExceptionAsync(context, ex as UnauthorizedApiException);
                }
                if (ex is NotFoundApiException)
                {
                    await HandleNotFoundExceptionAsync(context, ex as NotFoundApiException);
                }
                if (ex is CheckPasswordApiException)
                {
                    await HandleCheckPasswordExceptionAsync(context, ex as CheckPasswordApiException);
                }
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationApiException exception)
        {
            var result = JsonConvert.SerializeObject(exception.Errors);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            return context.Response.WriteAsync(result);
        }
        private static Task HandleDuplicateUserExceptionAsync(HttpContext context, DuplicateUserApiException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            return context.Response.WriteAsync(exception.Message);
        }
        private static Task HandleUnauthorizedExceptionAsync(HttpContext context, UnauthorizedApiException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return context.Response.WriteAsync(exception.Message);
        }
        private static Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundApiException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return context.Response.WriteAsync(exception.Message);
        }
        private static Task HandleCheckPasswordExceptionAsync(HttpContext context, CheckPasswordApiException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsync(exception.Message);
        }
    }
}
