using InnoGotchiWebAPI.Database;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiWebAPI.Logic
{
    public class InnoGotchiDBService : IInnoGotchiDBService
    {
        private readonly InnoGotchiWebContext _context;
        public InnoGotchiDBService(InnoGotchiWebContext context)
        {
            _context = context;
        }
        public async Task<DbPetModel> DeletePet(Guid id)
        {
            DbPetModel? toDelete = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);

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

        public async Task<DbPetModel> GetPet(Guid id)
        {
            DbPetModel? toReturn = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);

            return toReturn ?? throw new InnoGotchiException("Can't find specified pet", 404);
        }

        public async Task<IEnumerable<DbPetModel>> GetPets()
        {
            IEnumerable<DbPetModel> pets = await _context.Pets.AsNoTracking().ToListAsync();
            return pets;
        }

        public async Task PostPet(DbPetModel pet)
        {
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();
        }

        public async Task<DbPetModel> PutPet(DbPetModel pet)
        {
            DbPetModel? toEdit = await _context.Pets.FirstOrDefaultAsync(p => p.Id == pet.Id)
                ?? throw new InnoGotchiException("Can't find specified pet", 404);

            _context.Entry(toEdit).CurrentValues.SetValues(pet);

            await _context.SaveChangesAsync();

            return toEdit;
        }
    }
}
