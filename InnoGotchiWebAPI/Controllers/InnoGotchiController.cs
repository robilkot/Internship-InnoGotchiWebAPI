using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/pets")]
    public class InnoGotchiController : ControllerBase
    {
        private readonly IInnoGotchiService _service;

        public InnoGotchiController(IInnoGotchiService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {
            return new(await _service.GetPets());
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Pet>> GetPet(Guid id)
        {
            return new(await _service.GetPet(id));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<Pet>> DeletePet(Guid id)
        {
            return new(await _service.DeletePet(id));
        }

        [HttpPost]
        public async Task<ActionResult<Pet>> PostPet(Pet pet)
        {
            return new(await _service.PostPet(pet));
        }

        [HttpPut]
        public async Task<ActionResult<Pet>> Pets(Pet pet)
        {
            return new(await _service.PutPet(pet));
        }
    }
}