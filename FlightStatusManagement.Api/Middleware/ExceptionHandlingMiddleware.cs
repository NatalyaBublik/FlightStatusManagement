using System.Text.Json;
using FluentValidation;
using FlightStatusManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlightStatusManagement.Api.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        private readonly ICurrentUserService _currentUser;

        public ExceptionHandlingMiddleware(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var userId = _currentUser.UserId?.ToString() ?? "anonymous";

                Log.Error(ex, "Unhandled exception. UserId={UserId} Path={Path}", userId, context.Request.Path);

                await WriteProblemDetailsAsync(context, ex);
            }
        }

        private static Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
        {
            var (status, title, errors) = ex switch
            {
                ValidationException ve => (
                    StatusCodes.Status400BadRequest,
                    "Validation error",
                    ve.Errors
                      .GroupBy(e => e.PropertyName)
                      .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray())
                ),

                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized",
                    null as Dictionary<string, string[]>
                ),

                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    "Not found",
                    null as Dictionary<string, string[]>
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Internal server error",
                    null as Dictionary<string, string[]>
                )
            };

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = status == 500 ? "Unexpected error occurred." : ex.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/problem+json";

            if (errors is not null)
            {
                var validation = new ValidationProblemDetails(errors)
                {
                    Status = status,
                    Title = title,
                    Instance = context.Request.Path
                };
                return context.Response.WriteAsync(JsonSerializer.Serialize(validation, JsonOptions));
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOptions));
        }
    }
}
