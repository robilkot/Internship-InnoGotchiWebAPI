using System.Text;
using InnoGotchiWebAPI;

namespace InnoGotchiWebAPI.Models
{
    public class Pet : IComparable
    {
        public static readonly int EatInterval = 3600;
        public static readonly int DrinkInterval = 3600;
        public static readonly string DefaultName = "Unnamed Pet";
        public Guid Id { get; init; } = Guid.NewGuid();
        public Body Body { get; set; } = Body.Medium;
        public Eyes Eyes { get; set; } = Eyes.Brown;
        public Nose Nose { get; set; } = Nose.Medium;
        public Mouth Mouth { get; set; } = Mouth.Medium;
        public string Name { get; set; } = DefaultName;

        private DateTime _created = DateTime.Now;
        public DateTime Created
        {
            get => _created;
            init => _created = value;
        }

        private DateTime _updated = DateTime.Now;
        public DateTime Updated
        {
            get => _updated;
            init => _updated = value;
        }

        private DateTime _lastEatTime = DateTime.Now;
        public DateTime LastEatTime
        {
            get => _lastEatTime;
            init => _lastEatTime = value;
        }

        private DateTime _lastDrinkTime = DateTime.Now;
        public DateTime LastDrinkTime
        {
            get => _lastDrinkTime;
            init => _lastDrinkTime = value;
        }

        private Hunger _hunger = Hunger.Full;
        public Hunger Hunger
        {
            get => _hunger;
            init => _hunger = value;
        }

        private Thirst _thirst = Thirst.Full;
        public Thirst Thirst
        {
            get => _thirst;
            init => _thirst = value;
        }

        private int _happinessDaysCount = 0;
        public int HappinessDaysCount
        {
            get => _happinessDaysCount;
            init => _happinessDaysCount = value;
        }

        public int Age
        {
            // Returns age as 1 full real week = 1 in-game year
            get
            {
                return (DateTime.Now - Created).Days / 7;
            }
        }

        private bool _dead = false;
        public bool Dead
        {
            get => _dead;
            init => _dead = value;
        }

        public Pet() { }
        public Pet(Body body, Eyes eyes, Nose nose, Mouth mouth, string name)
        {
            Body = body;
            Eyes = eyes;
            Nose = nose;
            Mouth = mouth;
            Name = name;
        }

        public void UpdateState()
        {
            if (Dead)
            {
                _updated = DateTime.Now;
                return;
            }

            bool stateChanged = false;

            // Protects against double decrementations
            if ((DateTime.Now - _updated).TotalSeconds > DrinkInterval)
            {
                var thirstDifference = (DateTime.Now - LastDrinkTime).TotalSeconds / DrinkInterval;
                _thirst -= thirstDifference < (int)Thirst ? (Thirst)thirstDifference : Thirst;
                stateChanged = true;
            }
            if ((DateTime.Now - _updated).TotalSeconds > EatInterval)
            {
                var hungerDifference = (DateTime.Now - LastEatTime).TotalSeconds / EatInterval;
                _hunger -= hungerDifference < (int)Hunger ? (Hunger)hungerDifference : Hunger;
                stateChanged = true;
            }

            if (Thirst == Thirst.Dead || Hunger == Hunger.Dead)
            {
                _dead = true;
            }
            else if (Thirst >= Thirst.Normal && Hunger >= Hunger.Normal)
            {
                _happinessDaysCount += (DateTime.Now - _updated).Days;
            }

            if (stateChanged)
            {
                _updated = DateTime.Now;
            }
        }
        public void GiveDrink()
        {
            UpdateState();

            if (Dead)
            {
                return;
            }

            if (Thirst < Thirst.Full)
            {
                _thirst++;
            }

            _lastDrinkTime = DateTime.Now;
        }
        public void Feed()
        {
            UpdateState();

            if (Dead)
            {
                return;
            }

            if (Hunger < Hunger.Full)
            {
                _hunger++;
            }

            _lastEatTime = DateTime.Now;
        }
        public override string ToString()
        {
            StringBuilder str = new(128);

            if (Dead)
            {
                str.Append("(Dead) ");
            }

            str.Append(Name); str.Append(", ");
            str.Append(Age); str.Append(" y.o.\n");

            str.Append("Body: "); str.Append(Body.ToString());
            str.Append(", Eyes: "); str.Append(Eyes.ToString());
            str.Append(", Mouth: "); str.Append(Mouth.ToString());
            str.Append(", Nose: "); str.Append(Nose.ToString());

            str.Append("\nHunger: "); str.Append(Hunger.ToString());
            str.Append("\tLast meal:\t"); str.Append(LastEatTime);
            str.Append("\nThirst: "); str.Append(Thirst.ToString());
            str.Append("\tLast drink:\t"); str.Append(LastDrinkTime);

            str.Append("\nHappiness days: "); str.Append(HappinessDaysCount);

            return str.ToString();
        }
        public int CompareTo(object? obj)
        {
            if (obj is Pet pet)
            {
                if (HappinessDaysCount < pet.HappinessDaysCount)
                {
                    return -1;
                }
                else if (HappinessDaysCount > pet.HappinessDaysCount)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}