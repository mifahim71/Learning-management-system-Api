
using LearningManagementSystemApi.Exceptions;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace LearningManagementSystemApi.Middlewares
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode;
            var message = ex.Message;

            switch (ex)
            {
                case UserNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case CourseNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case ProfileCreationFailedException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                
                case CourseCreationFiledException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case CourseDeletionFailedException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case ForbiddenException:
                    statusCode = HttpStatusCode.Forbidden;
                    break;

                case EnrollmentNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Something went wrong.";
                    break;
            }



            var result = JsonSerializer.Serialize(new
            {
                success = false,
                statusCode = (int)statusCode,
                message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            return context.Response.WriteAsync(result);
                
        }

        
    }
}
