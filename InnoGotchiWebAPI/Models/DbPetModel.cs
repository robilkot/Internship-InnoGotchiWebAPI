namespace InnoGotchiWebAPI.Models
{
    public class DbPetModel
    {
        public string Name { get; set; } = AppConstants.DefaultPetName;
        public Guid Id { get; init; } = Guid.NewGuid();
        public Body Body { get; set; } = Body.Medium;
        public Eyes Eyes { get; set; } = Eyes.Brown;
        public Nose Nose { get; set; } = Nose.Medium;
        public Mouth Mouth { get; set; } = Mouth.Medium;

        private DateTime _created = DateTime.Now;
        public DateTime Created
        {
            get => _created;
            set => _created = value;
        }

        private DateTime _updated = DateTime.Now;
        public DateTime Updated
        {
            get => _updated;
            set => _updated = value;
        }

        private DateTime _lastEatTime = DateTime.Now;
        public DateTime LastEatTime
        {
            get => _lastEatTime;
            set => _lastEatTime = value;
        }

        private DateTime _lastDrinkTime = DateTime.Now;
        public DateTime LastDrinkTime
        {
            get => _lastDrinkTime;
            set => _lastDrinkTime = value;
        }

        private Hunger _hunger = Hunger.Full;
        public Hunger Hunger
        {
            get => _hunger;
            set => _hunger = value;
        }

        private Thirst _thirst = Thirst.Full;
        public Thirst Thirst
        {
            get => _thirst;
            set => _thirst = value;
        }

        private int _happinessDaysCount = 0;
        public int HappinessDaysCount
        {
            get => _happinessDaysCount;
            set => _happinessDaysCount = value;
        }

        private bool _dead = false;
        public bool Dead
        {
            get => _dead;
            set => _dead = value;
        }
    }
}