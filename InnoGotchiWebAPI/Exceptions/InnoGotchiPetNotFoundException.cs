namespace InnoGotchiWebAPI.Exceptions
{
    public class InnoGotchiPetNotFoundException : InnoGotchiException
    {
        public InnoGotchiPetNotFoundException(string? message = "Specified pet not found") : base(message, 404) { }
    }
}
