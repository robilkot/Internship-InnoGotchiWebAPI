using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Interfaces
{
    public interface IDBService
    {
        public Task<bool> PetExists(Guid id);
        public Task<IEnumerable<DbPetModel>> GetPets(string userlogin);
        public Task<DbPetModel> GetPet(Guid id, string userlogin);
        public Task<DbPetModel> DeletePet(Guid id, string userlogin);
        public Task AddPet(DbPetModel pet);
        public Task<DbPetModel> UpdatePet(DbPetModel pet, string userlogin);

        public Task<bool> UserExists(string login);
        public Task<IEnumerable<DbUserModel>> GetUsers();
        public Task<DbUserModel> GetUser(string login);
        public Task<DbUserModel> DeleteUser(string login);
        public Task AddUser(DbUserModel user);
        public Task<DbUserModel> UpdateUser(DbUserModel user);
    }
}
