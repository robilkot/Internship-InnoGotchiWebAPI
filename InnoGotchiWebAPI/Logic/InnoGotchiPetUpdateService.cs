using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Logic
{
    public class InnoGotchiPetUpdateService
    {
        private readonly IInnoGotchiDBPetService _dbService;
        public InnoGotchiPetUpdateService(IInnoGotchiDBPetService dbService)
        {
            _dbService = dbService;
        }

        public async Task<IEnumerable<DbPetModel>> UpdateAll(string userlogin)
        {
            var pets = await _dbService.GetPets(userlogin);

            foreach (var pet in pets)
            {
                UpdateModel(pet);
            }

            return pets;
        }

        public async Task<DbPetModel> Update(Guid id)
        {
            DbPetModel pet = await _dbService.GetPet(id);

            UpdateModel(pet);

            return pet;
        }

        private static void UpdateModel(DbPetModel pet)
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

        public async Task<DbPetModel> GiveDrink(Guid id)
        {
            DbPetModel pet = await _dbService.GetPet(id);

            UpdateModel(pet);

            if (pet.Dead == false)
            {
                if (pet.Hunger < Hunger.Full)
                {
                    pet.Hunger++;
                }

                pet.LastEatTime = DateTime.Now;
            }

            return pet;
        }
        public async Task<DbPetModel> Feed(Guid id)
        {
            DbPetModel pet = await _dbService.GetPet(id);

            UpdateModel(pet);

            if (pet.Dead == false)
            {
                if (pet.Hunger < Hunger.Full)
                {
                    pet.Hunger++;
                }

                pet.LastEatTime = DateTime.Now;
            }

            return pet;
        }
    }
}
