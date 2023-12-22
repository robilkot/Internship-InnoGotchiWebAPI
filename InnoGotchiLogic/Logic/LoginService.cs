using InnoGotchiWebAPI.Exceptions;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace InnoGotchiWebAPI.Logic
{
    public class LoginService : ILoginService
    {
        private readonly IDBService _dbService;
        public LoginService(IDBService dbUserService)
        {
            _dbService = dbUserService;
        }

        public async Task<DbUserModel> Login(string login, string password)
        {
            var user = await _dbService.GetUser(login);

            var hashedPassword = GetHashedPassword(password);

            if (user.Password!.SequenceEqual(hashedPassword) == false)
            {
                throw new InnoGotchiWrongPassowordException();
            }

            return user;
        }


        public async Task Register(string login, string password, string? nickname = default)
        {
            var hashedPassword = GetHashedPassword(password);

            DbUserModel userModel = new()
            {
                Login = login,
                Password = hashedPassword,
                Nickname = nickname
            };

            await _dbService.AddUser(userModel);
        }

        private static byte[] GetHashedPassword(string password)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(password);
            byte[] encodedKey = SHA1.HashData(keyArray);

            return encodedKey;
        }
    }
}
