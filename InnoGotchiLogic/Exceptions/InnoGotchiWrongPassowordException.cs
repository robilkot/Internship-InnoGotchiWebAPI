namespace InnoGotchiWebAPI.Exceptions
{
    public class InnoGotchiWrongPassowordException : InnoGotchiLogicException
    {
        public InnoGotchiWrongPassowordException(string? message = "Wrong password") : base(message, 401) { }
    }
}
