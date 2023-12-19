using AutoMapper;

namespace InnoGotchiWebAPI.Models.MapperProfiles
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap<ClientPetModel, DbPetModel>();
            CreateMap<DbPetModel, ClientPetModel>();

            CreateMap<ClientUserModel, DbUserModel>();
            CreateMap<DbUserModel, ClientUserModel>();
        }
    }
}
