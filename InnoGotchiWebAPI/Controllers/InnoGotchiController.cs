using AutoMapper;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Logic;
using InnoGotchiWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/pets")]
    public class InnoGotchiController : ControllerBase
    {
        private readonly IInnoGotchiDBService _dbService;
        private readonly InnoGotchiPetUpdateService _petUpdateService;

        public InnoGotchiController(IInnoGotchiDBService dbService, InnoGotchiPetUpdateService petUpdateService)
        {
            _dbService = dbService;
            _petUpdateService = petUpdateService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Prod]
        public async Task<ActionResult<IEnumerable<ClientPetModel>>> GetPets()
        {
            var dbPets = await _dbService.GetPets();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<DbPetModel, ClientPetModel>());
            var mapper = new Mapper(config);
            var clientPets = mapper.Map<IEnumerable<ClientPetModel>>(dbPets);
            
            return new(clientPets);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> GetPet(Guid id)
        {
            var dbPet = await _dbService.GetPet(id);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<DbPetModel, ClientPetModel>());
            var mapper = new Mapper(config);
            var clientPet = mapper.Map<ClientPetModel>(dbPet);

            return new(clientPet);
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> DeletePet(Guid id)
        {
            var dbPet = await _dbService.DeletePet(id);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<DbPetModel, ClientPetModel>());
            var mapper = new Mapper(config);
            var clientPet = mapper.Map<ClientPetModel>(dbPet);

            return new(clientPet);
        }

        [HttpPost]
        public async Task<ActionResult> PostPet(ClientPetModel pet)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ClientPetModel, DbPetModel> ());
            var mapper = new Mapper(config);
            var dbPet = mapper.Map<DbPetModel>(pet);
            
            await _dbService.PostPet(dbPet);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Pets(ClientPetModel pet)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ClientPetModel, DbPetModel>());
            var mapper = new Mapper(config);
            var dbPet = mapper.Map<DbPetModel>(pet);

            await _dbService.PutPet(dbPet);

            return Ok();
        }


        [HttpGet("{id:Guid}/feed")]
        public async Task<ActionResult> Feed(Guid id)
        {
            var pet = await _dbService.GetPet(id);
            _petUpdateService.Feed(pet);
            await _dbService.PutPet(pet);

            return Ok();
        }

        [HttpGet("{id:Guid}/givedrink")]
        public async Task<ActionResult> GiveDrink(Guid id)
        {
            var pet = await _dbService.GetPet(id);
            _petUpdateService.GiveDrink(pet);
            await _dbService.PutPet(pet);

            return Ok();
        }

        [HttpGet("{id:Guid}/update")]
        public async Task<ActionResult> Update(Guid id)
        {
            var pet = await _dbService.GetPet(id);
            _petUpdateService.Update(pet);
            await _dbService.PutPet(pet);

            return Ok();
        }

        [HttpGet("update")]
        public async Task<ActionResult> UpdateAll(Guid id)
        {
            await _petUpdateService.UpdateAll();
            
            return Ok();
        }
    }
}