namespace BankSystem.AccountApi.Middlewares
{
    public enum ExceptionType
    {
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409
    }
}
