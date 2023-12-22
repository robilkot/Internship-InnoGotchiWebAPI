namespace InnoGotchiWebAPI.Options
{
    public class DBOptions
    {
        public const string Position = "DB";

        public string DefaultRole { get; set; }
        public string ConnectionString { get; set; }
        public string DefaultPetName { get; set; }
        public DBOptions() { }
    }
}
