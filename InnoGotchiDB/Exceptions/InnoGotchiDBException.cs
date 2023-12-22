namespace InnoGotchiWebAPI.Exceptions
{
    public class InnoGotchiDBException : Exception
    {
        public int? StatusCode = null;

        public InnoGotchiDBException(string? message, int? statusCode = default) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
