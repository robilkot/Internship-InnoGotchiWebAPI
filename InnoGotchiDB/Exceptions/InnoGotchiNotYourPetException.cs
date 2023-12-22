namespace InnoGotchiWebAPI.Exceptions
{
    public class InnoGotchiNotYourPetException : InnoGotchiDBException
    {
        public InnoGotchiNotYourPetException(string? message = "Pet doesn't belong to you") : base(message, 403) { }
    }
}
