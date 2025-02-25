using System.Net;
using System.Text.Json;
using WebSocketChatApp.DTOs.GenericDTOs;

namespace WebSocketChatApp.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILoggerManager _logger;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Thiết lập mã trạng thái mặc định
            var statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";
            Console.WriteLine(exception.Message);

            // Thiết lập mã trạng thái và trả về response
            context.Response.StatusCode = statusCode;
            var response = new ExceptionResponse
            {
                StatusCode = statusCode,
                Message = message
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
