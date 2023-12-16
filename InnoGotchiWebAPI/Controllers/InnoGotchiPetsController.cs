using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Logic;
using InnoGotchiWebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace InnoGotchiWebAPI.Controllers
{
    [ApiController]
    [Route("innogotchi/pets")]
    public class InnoGotchiPetsController : ControllerBase
    {
        private readonly IInnoGotchiDBPetService _dbService;
        private readonly InnoGotchiPetUpdateService _petUpdateService;

        public InnoGotchiPetsController(IInnoGotchiDBPetService dbService, InnoGotchiPetUpdateService petUpdateService)
        {
            _dbService = dbService;
            _petUpdateService = petUpdateService;
        }

        /// <summary>
        /// Get all pets from db
        /// </summary>
        /// <returns>Pets from db</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET /innogotchi/pets/
        ///
        /// </remarks>
        /// <response code="200">Pets succesfully retrieved</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<ClientPetModel>>> GetPets()
        {
            var dbPets = await _dbService.GetPets();

            var clientPets = Mappers.PetDbToClientMapper.Map<IEnumerable<ClientPetModel>>(dbPets);

            return new(clientPets);
        }



        /// <summary>
        /// Get pet with given id from db
        /// </summary>
        /// <param name="id">Pet identifier</param>
        /// <returns>Pet from db</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET /innogotchi/pets/{id}
        ///
        /// </remarks>
        /// <response code="200">Pet succesfully retrieved</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> GetPet(Guid id)
        {
            var dbPet = await _dbService.GetPet(id);

            var clientPet = Mappers.PetDbToClientMapper.Map<ClientPetModel>(dbPet);

            return new(clientPet);
        }

        /// <summary>
        /// Delete pet with given id from db
        /// </summary>
        /// <param name="id">Pet identifier</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /innogotchi/pets/{id}
        ///
        /// </remarks>
        /// <response code="200">Pet succesfully deleted</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> DeletePet(Guid id)
        {
            var dbPet = await _dbService.DeletePet(id);

            var clientPet = Mappers.PetDbToClientMapper.Map<ClientPetModel>(dbPet);

            return new(clientPet);
        }

        /// <summary>
        /// Creates pet in db from given model
        /// </summary>
        /// <param name="pet">Pet clientmodel</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /innogotchi/pets/
        ///
        /// </remarks>
        /// <response code="200">Pet succesfully created</response>
        /// <response code="409">Pet with given id already exists. Use PUT instead</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> PostPet(ClientPetModel pet)
        {
            var dbPet = Mappers.PetClientToDbMapper.Map<DbPetModel>(pet);

            await _dbService.PostPet(dbPet);

            return Ok();
        }

        /// <summary>
        /// Updated properties in db of given pet
        /// </summary>
        /// <param name="pet">Pet clientmodel</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /innogotchi/pets/
        ///
        /// </remarks>
        /// <response code="200">Pet succesfully updated</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> PutPet(ClientPetModel pet)
        {
            var dbPet = Mappers.PetClientToDbMapper.Map<DbPetModel>(pet);

            await _dbService.PutPet(dbPet);

            return Ok();
        }

        /// <summary>
        /// Feed pet with given id in db
        /// </summary>
        /// <param name="id">Pet identifier</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /innogotchi/pets/{id}/feed
        ///
        /// </remarks>
        /// <response code="200">Pet succesfully updated</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpGet("{id:Guid}/feed")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> Feed(Guid id)
        {
            var pet = await _petUpdateService.Feed(id);

            var clientPet = Mappers.PetClientToDbMapper.Map<ClientPetModel>(pet);

            return new(clientPet);
        }

        /// <summary>
        /// Give drink to pet with given id in db
        /// </summary>
        /// <param name="id">Pet identifier</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /innogotchi/pets/{id}/givedrink
        ///
        /// </remarks>
        /// <response code="200">Pet succesfully updated</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpGet("{id:Guid}/givedrink")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> GiveDrink(Guid id)
        {
            var pet = await _petUpdateService.GiveDrink(id);

            var clientPet = Mappers.PetClientToDbMapper.Map<ClientPetModel>(pet);

            return new(clientPet);
        }

        /// <summary>
        /// Update pet with given id in db
        /// </summary>
        /// <param name="id">Pet identifier</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /innogotchi/pets/{id}/update
        ///
        /// </remarks>
        /// <response code="200">Pet succesfully updated</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpGet("{id:Guid}/update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DbPetModel>> Update(Guid id)
        {
            var pet = await _petUpdateService.Update(id);

            // map

            return new(pet);
        }

        /// <summary>
        /// Update all pets in db
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /innogotchi/pets/update
        ///
        /// </remarks>
        /// <response code="200">Pets succesfully updated</response>
        [HttpGet("update")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> UpdateAll()
        {
            await _petUpdateService.UpdateAll();

            return Ok();
        }
    }
}