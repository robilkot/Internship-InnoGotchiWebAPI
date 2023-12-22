using InnoGotchiWebAPI.Exceptions;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using InnoGotchiWebAPI.Options;
using Microsoft.Extensions.Options;

namespace InnoGotchiWebAPI.Logic
{
    public class PetUpdateService
    {
        private readonly IDBService _dbService;

        public PetUpdateService(IDBService dbService)
        {
            _dbService = dbService;
        }

        public async Task<IEnumerable<DbPetModel>> UpdateAll(string userlogin, IOptions<LogicOptions> innogotchiOptions)
        {
            var pets = await _dbService.GetPets(userlogin);

            foreach (var pet in pets)
            {
                UpdateModel(pet, innogotchiOptions);

                await _dbService.UpdatePet(pet, userlogin);
            }

            return pets;
        }

        public async Task<DbPetModel> Update(Guid id, string userlogin, IOptions<LogicOptions> innogotchiOptions)
        {
            DbPetModel pet = await _dbService.GetPet(id, userlogin);

            UpdateModel(pet, innogotchiOptions);

            await _dbService.UpdatePet(pet, userlogin);

            return pet;
        }

        private static void UpdateModel(DbPetModel pet, IOptions<LogicOptions> innogotchiOptions)
        {
            if (pet.Dead)
            {
                pet.Updated = DateTime.Now;
                return;
            }

            bool stateChanged = false;

            // Protects against double decrementations
            if ((DateTime.Now - pet.Updated).TotalSeconds > innogotchiOptions.Value.PetDrinkInterval)
            {
                var thirstDifference = (DateTime.Now - pet.LastDrinkTime).TotalSeconds / innogotchiOptions.Value.PetDrinkInterval;
                pet.Thirst -= thirstDifference < (int)pet.Thirst ? (Thirst)thirstDifference : pet.Thirst;
                stateChanged = true;
            }
            if ((DateTime.Now - pet.Updated).TotalSeconds > innogotchiOptions.Value.PetEatInterval)
            {
                var hungerDifference = (DateTime.Now - pet.LastEatTime).TotalSeconds / innogotchiOptions.Value.PetEatInterval;
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

        public async Task<DbPetModel> GiveDrink(Guid id, string userlogin, IOptions<LogicOptions> innogotchiOptions)
        {
            DbPetModel pet = await _dbService.GetPet(id, userlogin);

            UpdateModel(pet, innogotchiOptions);

            if (pet.Dead == false)
            {
                if (pet.Hunger < Hunger.Full)
                {
                    pet.Hunger++;
                }

                pet.LastEatTime = DateTime.Now;
            }

            await _dbService.UpdatePet(pet, userlogin);

            return pet;
        }
        public async Task<DbPetModel> Feed(Guid id, string userlogin, IOptions<LogicOptions> innogotchiOptions)
        {
            DbPetModel pet = await _dbService.GetPet(id, userlogin);

            UpdateModel(pet, innogotchiOptions);

            if (pet.Dead == false)
            {
                if (pet.Hunger < Hunger.Full)
                {
                    pet.Hunger++;
                }

                pet.LastEatTime = DateTime.Now;
            }

            await _dbService.UpdatePet(pet, userlogin);

            return pet;
        }
    }
}
