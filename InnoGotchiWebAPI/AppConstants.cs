using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InnoGotchiWebAPI
{
    public static class AppConstants
    {
        public static readonly string DefaultPetName = "Unnamed Pet";
        public static readonly int PetEatInterval = 3600;
        public static readonly int PetDrinkInterval = 3600;

        public static readonly string TokenSecretKey = "InnogotchiSecretKey";
        public static readonly string TokenIssuer = "innogotchi";
        public static readonly TimeSpan TokenLifeTime = TimeSpan.FromSeconds(20);
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(TokenSecretKey));
    }
}
