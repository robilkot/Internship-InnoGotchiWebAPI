using AutoMapper;
using InnoGotchiWebAPI.Exceptions;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InnoGotchiWebAPI.Logic
{
    public class InnoGotchiLoginService : IInnoGotchiLoginService
    {
        private readonly IInnoGotchiDBUserService _dbUserService;
        private readonly IMapper _mapper;
        public InnoGotchiLoginService(IInnoGotchiDBUserService dbUserService, IMapper mapper)
        {
            _dbUserService = dbUserService;
            _mapper = mapper;
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
                new Claim(ClaimTypes.NameIdentifier, login),
                new Claim(ClaimTypes.Role, user.Role!)
            };

            var jwt = new JwtSecurityToken(
                    claims: claims,
                    issuer: AppConstants.TokenIssuer,
                    expires: DateTime.Now.Add(AppConstants.TokenLifeTime),
                    signingCredentials: new SigningCredentials(AppConstants.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            var clientUser = _mapper.Map<ClientUserModel>(user);
            clientUser.Token = token;

            return clientUser;
        }


        public async Task Register(string login, string password, string? nickname = default)
        {
            if (await _dbUserService.UserExists(login))
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
