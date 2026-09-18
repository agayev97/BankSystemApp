using BankSystem.Common.Exceptions;
using BankSystem.Common.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Common.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            //  Xətanın növünə görə Status Code və Mesajı müəyyən edirik

            var (statusCode, title, errors) = exception switch
            {
                NotFoundException => (
                 StatusCodes.Status404NotFound,
                 "Məlumat tapılmadı",
                 null),

                BusinessException => (
               StatusCodes.Status400BadRequest,
               "Biznes qaydası xətası",
               null),

                ValidationException valEx => (
                 StatusCodes.Status400BadRequest,
                 "Validasiya xətası",
                 valEx.Errors),

                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    "Giriş icazəsi yoxdur",
                    null),

                Microsoft.Data.SqlClient.SqlException sqlEx when sqlEx.Number >= 50000 => (
                    StatusCodes.Status400BadRequest,
                    "Biznes qaydası xətası",
                    null),


                // Digər bütün gözlənilməz sistem xətaları (SQL xətası, NullReference və s.)
                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Daxili server xətası baş verdi",
                    null)
            };

            // Serilog / ILogger ilə qeyd edirik
            if (statusCode >= 500)
            {
                _logger.LogError(exception,
                    "Kritik Xəta! Path: {Path}, Xəta mesajı: {Message}",
                    httpContext.Request.Path, exception.Message);
            }
            else
            {
                _logger.LogWarning(
                    "Gözlənilən Xəta ({StatusCode}). Path: {Path}, Mesaj: {Message}",
                    statusCode, httpContext.Request.Path, exception.Message);
            }

            //  Response-u hazırlayırıq
            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Title = title,
                Message = statusCode >= 500
                    ? "Gözlənilməz bir xəta baş verdi. Zəhmət olmasa sistem inzibatçısına müraciət edin."
                    : exception.Message,
                Instance = httpContext.Request.Path,
                Timestamp = DateTime.UtcNow,
                Errors = errors
            };


            //  HTTP Cavabını göndəririk
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true; // Xətanın idarə edildiyini bildirir
        }

    }
}
