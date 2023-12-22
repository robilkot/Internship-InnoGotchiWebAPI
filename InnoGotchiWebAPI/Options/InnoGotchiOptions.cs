namespace InnoGotchiWebAPI.Options
{
    public class InnoGotchiOptions
    {
        public const string Position = "InnoGotchi";

        public string DefaultPetName { get; set; }
        public int PetEatInterval { get; set; }
        public int PetDrinkInterval { get; set; }
        public InnoGotchiOptions() { }
    }
}
