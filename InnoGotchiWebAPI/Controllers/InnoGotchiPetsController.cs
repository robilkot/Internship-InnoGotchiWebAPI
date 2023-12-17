using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Logic;
using InnoGotchiWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<ClientPetModel>>> GetPets()
        {
            var userlogin = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var dbPets = await _dbService.GetPets(userlogin!);

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
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Pet belongs to other user</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpGet("{id:Guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> GetPet(Guid id)
        {
            var dbPet = await _dbService.GetPet(id);

            if (CheckOwnership(dbPet) == false)
            {
                return Forbid();
            }

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
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Pet belongs to other user</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpDelete("{id:Guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> DeletePet(Guid id)
        {
            // todo: Is this optimal?
            var dbPet = await _dbService.GetPet(id);

            if (CheckOwnership(dbPet) == false)
            {
                return Forbid();
            }

            dbPet = await _dbService.DeletePet(id);

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
        /// <response code="401">Unauthorized</response>
        /// <response code="409">Pet with given id already exists. Use PUT instead</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> PostPet(ClientPetModel pet)
        {
            var dbPet = Mappers.PetClientToDbMapper.Map<DbPetModel>(pet);

            var userlogin = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Todo: is it ok to set ownership here?
            dbPet.OwnerId = userlogin!;

            await _dbService.PostPet(dbPet, userlogin!);

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
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Pet belongs to other user</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> PutPet(ClientPetModel pet)
        {
            bool petExists = await _dbService.PetExists(pet.Id);

            if (petExists)
            {
                var ownerCheckingPet = await _dbService.GetPet(pet.Id);

                if (CheckOwnership(ownerCheckingPet) == false)
                {
                    return Forbid();
                }
            }

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
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Pet belongs to other user</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpGet("{id:Guid}/feed")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> Feed(Guid id)
        {
            var dbPet = await _dbService.GetPet(id);

            if (CheckOwnership(dbPet) == false)
            {
                return Forbid();
            }

            var pet = await _petUpdateService.Feed(id);

            var clientPet = Mappers.PetDbToClientMapper.Map<ClientPetModel>(pet);

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
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Pet belongs to other user</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpGet("{id:Guid}/givedrink")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> GiveDrink(Guid id)
        {
            var dbPet = await _dbService.GetPet(id);

            if (CheckOwnership(dbPet) == false)
            {
                return Forbid();
            }

            var pet = await _petUpdateService.GiveDrink(id);

            var clientPet = Mappers.PetDbToClientMapper.Map<ClientPetModel>(pet);

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
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Pet belongs to other user</response>
        /// <response code="404">Pet with given id not found</response>
        [HttpGet("{id:Guid}/update")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientPetModel>> Update(Guid id)
        {
            var dbPet = await _dbService.GetPet(id);

            if (CheckOwnership(dbPet) == false)
            {
                return Forbid();
            }

            var pet = await _petUpdateService.Update(id);

            var clientPet = Mappers.PetDbToClientMapper.Map<ClientPetModel>(pet);

            return new(clientPet);
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
        /// <response code="401">Unauthorized</response>
        [HttpGet("update")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> UpdateAll()
        {
            var userlogin = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _petUpdateService.UpdateAll(userlogin!);

            return Ok();
        }

        // todo: Bad practice? How to implement check properly?
        [NonAction]
        private bool CheckOwnership(DbPetModel pet)
        {
            var userlogin = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (pet.OwnerId != userlogin)
            {
                return false;
            }

            return true;
        }
    }
}