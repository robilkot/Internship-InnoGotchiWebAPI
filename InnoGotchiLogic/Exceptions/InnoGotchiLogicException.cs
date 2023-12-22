namespace InnoGotchiWebAPI.Exceptions
{
    public class InnoGotchiLogicException : Exception
    {
        public int? StatusCode = null;

        public InnoGotchiLogicException(string? message, int? statusCode = default) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
