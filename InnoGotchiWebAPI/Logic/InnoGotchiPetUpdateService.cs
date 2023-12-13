using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Logic
{
    public class InnoGotchiPetUpdateService
    {
        private readonly IInnoGotchiDBService _dbService;
        public InnoGotchiPetUpdateService(IInnoGotchiDBService dbService)
        {
            _dbService = dbService;
        }
        public async Task UpdateAll()
        {
            var pets = await _dbService.GetPets();

            foreach (var pet in pets)
            {
                Update(pet);
            }
        }
        public void Update(DbPetModel pet)
        {
            if (pet.Dead)
            {
                pet.Updated = DateTime.Now;
                return;
            }

            bool stateChanged = false;

            // Protects against double decrementations
            if ((DateTime.Now - pet.Updated).TotalSeconds > AppConstants.PetDrinkInterval)
            {
                var thirstDifference = (DateTime.Now - pet.LastDrinkTime).TotalSeconds / AppConstants.PetDrinkInterval;
                pet.Thirst -= thirstDifference < (int)pet.Thirst ? (Thirst)thirstDifference : pet.Thirst;
                stateChanged = true;
            }
            if ((DateTime.Now - pet.Updated).TotalSeconds > AppConstants.PetEatInterval)
            {
                var hungerDifference = (DateTime.Now - pet.LastEatTime).TotalSeconds / AppConstants.PetEatInterval;
                pet.Hunger -= hungerDifference < (int)pet.Hunger ? (Hunger)hungerDifference : pet.Hunger;
                stateChanged = true;
            }

            if (pet.Thirst == Thirst.Dead || pet.Hunger == Hunger.Dead)
            {
                pet.Dead = true;
            }
            else if (pet.Thirst >= Thirst.Normal && pet.Hunger >= Hunger.Normal)
            {
                pet.HappinessDaysCount += (DateTime.Now - pet.Updated).Days;
            }

            if (stateChanged)
            {
                pet.Updated = DateTime.Now;
            }
        }

        public void GiveDrink(DbPetModel pet)
        {
            Update(pet);

            if (pet.Dead)
            {
                return;
            }

            if (pet.Thirst < Thirst.Full)
            {
                pet.Thirst++;
            }

            pet.LastDrinkTime = DateTime.Now;
        }
        public void Feed(DbPetModel pet)
        {
            Update(pet);

            if (pet.Dead)
            {
                return;
            }

            if (pet.Hunger < Hunger.Full)
            {
                pet.Hunger++;
            }

            pet.LastEatTime = DateTime.Now;
        }
    }
}
