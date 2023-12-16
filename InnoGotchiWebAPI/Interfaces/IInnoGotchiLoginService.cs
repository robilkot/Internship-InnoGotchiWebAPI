using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Interfaces
{
    public interface IInnoGotchiLoginService
    {
        public Task<ClientUserModel> Login(string username, string password);
        public Task Register(string username, string password, string? nickname);
    }
}
