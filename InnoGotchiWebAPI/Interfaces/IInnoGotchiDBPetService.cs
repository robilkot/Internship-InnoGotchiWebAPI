using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Interfaces
{
    public interface IInnoGotchiDBPetService
    {
        public Task<bool> PetExists(Guid id);
        public Task<IEnumerable<DbPetModel>> GetPets(string userlogin);
        public Task<DbPetModel> GetPet(Guid id);
        public Task<DbPetModel> DeletePet(Guid id);
        public Task PostPet(DbPetModel pet, string userlogin);
        public Task<DbPetModel> PutPet(DbPetModel pet);
    }
}
