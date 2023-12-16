using InnoGotchiWebAPI.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InnoGotchiWebAPI.Exceptions;
using InnoGotchiWebAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace InnoGotchiWebAPI.Logic
{
    public class InnoGotchiLoginService : IInnoGotchiLoginService
    {
        private readonly IInnoGotchiDBUserService _dbUserService;
        public InnoGotchiLoginService(IInnoGotchiDBUserService dbUserService)
        {
            _dbUserService = dbUserService;
        }

        public async Task<ClientUserModel> Login(string login, string password)
        {
            var user = await _dbUserService.GetUser(login);

            var hashedPassword = GetHashedPassword(password);

            if (user.Password!.SequenceEqual(hashedPassword) == false)
            {
                throw new InnoGotchiException("Wrong password", 401);
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, login)
            };

            var jwt = new JwtSecurityToken(
                    claims: claims,
                    issuer: AppConstants.TokenIssuer,
                    expires: DateTime.Now.Add(AppConstants.TokenLifeTime),
                    signingCredentials: new SigningCredentials(AppConstants.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            var clientUser = Mappers.UserDbToClientMapper.Map<ClientUserModel>(user);
            clientUser.Token = token;

            return clientUser;
        }


        public async Task Register(string login, string password, string? nickname = default)
        {
            if(await _dbUserService.UserExists(login))
            {
                throw new InnoGotchiException("User with this login already exists", 409);
            }

            var hashedPassword = GetHashedPassword(password);

            DbUserModel userModel = new()
            {
                Login = login,
                Password = hashedPassword,
                Nickname = nickname
            };

            await _dbUserService.PostUser(userModel);
        }

        private static byte[] GetHashedPassword(string password)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(password);
            byte[] encodedKey = SHA1.HashData(keyArray);

            return encodedKey;
        }
    }
}
