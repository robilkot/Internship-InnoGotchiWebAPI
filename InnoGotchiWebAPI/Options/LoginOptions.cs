namespace InnoGotchiWebAPI.Options
{
    public class LoginOptions
    {
        public const string Position = "Login";

        public string DefaultRole { get; set; }
        public string TokenIssuer { get; set; }
        public double TokenLifeTime { get; set; }
        public LoginOptions() { }
    }
}
