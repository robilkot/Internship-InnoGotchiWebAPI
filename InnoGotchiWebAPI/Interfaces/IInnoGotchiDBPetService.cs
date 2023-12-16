using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Interfaces
{
    public interface IInnoGotchiDBPetService
    {
        public Task<IEnumerable<DbPetModel>> GetPets();
        public Task<DbPetModel> GetPet(Guid id);
        public Task<DbPetModel> DeletePet(Guid id);
        public Task PostPet(DbPetModel pet);
        public Task<DbPetModel> PutPet(DbPetModel pet);
    }
}
