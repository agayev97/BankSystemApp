using Microsoft.AspNetCore.Http;

namespace BankSystem.AccountApi.Middlewares
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; }
        // Susmaya görə status kodu 400 (Bad Request) qəbul edir
        public BusinessException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }

        // Enum istifadə etmək istəsən:
        public BusinessException(string message, ExceptionType exceptionType) : base(message)
        {
            StatusCode = (int)exceptionType;
        }

    }
}
