using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Interfaces
{
    public interface IInnoGotchiService
    {
        public Task<IEnumerable<Pet>> GetPets();
        public Task<Pet> GetPet(Guid id);
        public Task<Pet> DeletePet(Guid id);
        public Task<Pet> PostPet(Pet pet);
        public Task<Pet> PutPet(Pet pet);
    }
}
