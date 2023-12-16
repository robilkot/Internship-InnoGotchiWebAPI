using AutoMapper;

namespace InnoGotchiWebAPI.Models
{
    public static class Mappers
    {
        // todo: make config not mappers. use DI then
        public static readonly MapperConfiguration PetClientToDbConfig = new(cfg => cfg.CreateMap<ClientPetModel, DbPetModel>());
        public static readonly MapperConfiguration PetDbToClientConfig = new(cfg => cfg.CreateMap<DbPetModel, ClientPetModel>());
        public static readonly Mapper PetClientToDbMapper = new (PetClientToDbConfig);
        public static readonly Mapper PetDbToClientMapper = new (PetDbToClientConfig);

        public static readonly MapperConfiguration UserClientToDbConfig = new(cfg => cfg.CreateMap<ClientUserModel, DbUserModel>());
        public static readonly MapperConfiguration UserDbToClientConfig = new(cfg => cfg.CreateMap<DbUserModel, ClientUserModel>());
        public static readonly Mapper UserClientToDbMapper = new(UserClientToDbConfig);
        public static readonly Mapper UserDbToClientMapper = new(UserDbToClientConfig);
    }
}
