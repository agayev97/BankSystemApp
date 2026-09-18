using BankSystem.AccountApi.DTOs;
using System.Net;
using System.Text.Json;

namespace BankSystem.AccountApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;   
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string userMessage;

            if (exception is BusinessException businessEx)
            {
                // İstifadəçinin etdiyi yanlışlıqlar və biznes qaydaları (400, 404 və s.)
                statusCode = businessEx.StatusCode;
                userMessage = businessEx.Message;

                // Serilog-a xəbərdarlıq (Warning) kimi yazılır
                _logger.LogWarning(exception, "Biznes xətası baş verdi: {Message}", businessEx.Message);
            }
            else
            {
                // Server tərəfdə gözlənilmədən baş verən sistem xətaları (500)
                statusCode = (int)HttpStatusCode.InternalServerError;
                userMessage = "Sistemdə gözlənilməz xəta baş verdi. Xahiş olunur bir qədər sonra yenidən cəhd edin.";

                // Serilog-a ƏSL xəta və StackTrace ətraflı yazılır
                _logger.LogError(exception, "Gözlənilməyən Server Xətası (500): {Message}", exception.Message);
            }

            context.Response.StatusCode = statusCode;

            var errorResponse = new ErrorResponseDto
            {
                StatusCode = statusCode,
                Message = userMessage
            };

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, jsonOptions));
        }
    }
}
