namespace InnoGotchiWebAPI.Exceptions
{
    public class InnoGotchiNotFoundException : InnoGotchiDBException
    {
        public InnoGotchiNotFoundException(string? message = "Specified entry not found") : base(message, 404) { }
    }
}
