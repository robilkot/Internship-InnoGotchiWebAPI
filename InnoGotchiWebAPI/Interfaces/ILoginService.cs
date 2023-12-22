using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Interfaces
{
    public interface ILoginService
    {
        public Task<DbUserModel> Login(string username, string password);
        public Task Register(string username, string password, string? nickname);
    }
}
