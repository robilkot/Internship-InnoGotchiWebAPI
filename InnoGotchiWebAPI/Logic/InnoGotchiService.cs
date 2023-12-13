using InnoGotchiWebAPI.Database;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiWebAPI.Logic
{
    public class InnoGotchiService : IInnoGotchiService
    {
        private readonly InnoGotchiContext _context;
        public InnoGotchiService(InnoGotchiContext context)
        {
            _context = context;
        }
        public async Task<Pet> DeletePet(Guid id)
        {
            Pet? toDelete = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);

            if (toDelete == null)
            {
                throw new InnoGotchiException("Can't find specified pet", 404);
            }
            else 
            {
                _context.Pets.Remove(toDelete);
                await _context.SaveChangesAsync();
            }

            return toDelete;
        }

        public async Task<Pet> GetPet(Guid id)
        {
            Pet? toReturn = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);

            return toReturn ?? throw new InnoGotchiException("Can't find specified pet", 404);
        }

        public async Task<IEnumerable<Pet>> GetPets()
        {
            IEnumerable<Pet> pets = await _context.Pets.AsNoTracking().ToListAsync();
            return pets;
        }

        public async Task<Pet> PostPet(Pet pet)
        {
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            return pet;
        }

        public async Task<Pet> PutPet(Pet pet)
        {
            Pet? toEdit = await _context.Pets.FirstOrDefaultAsync(p => p.Id == pet.Id)
                ?? throw new InnoGotchiException("Can't find specified pet", 404);

            _context.Entry(toEdit).CurrentValues.SetValues(pet);

            await _context.SaveChangesAsync();

            return toEdit;
        }
    }
}
