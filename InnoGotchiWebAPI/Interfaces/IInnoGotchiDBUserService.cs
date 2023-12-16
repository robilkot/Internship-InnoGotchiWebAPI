using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Interfaces
{
    public interface IInnoGotchiDBUserService
    {
        public Task<bool> UserExists(string login);
        public Task<IEnumerable<DbUserModel>> GetUsers();
        public Task<DbUserModel> GetUser(string login);
        public Task<DbUserModel> DeleteUser(string login);
        public Task PostUser(DbUserModel user);
        public Task<DbUserModel> PutUser(DbUserModel user);
    }
}
