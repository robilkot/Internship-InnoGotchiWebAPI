namespace InnoGotchiWebAPI.Logic
{
    public class InnoGotchiException : Exception
    {
        public int? StatusCode = null;

        public InnoGotchiException(string? message, int? statusCode = default) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
