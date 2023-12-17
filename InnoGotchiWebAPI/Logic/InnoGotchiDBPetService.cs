using InnoGotchiWebAPI.Database;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using InnoGotchiWebAPI.Exceptions;

namespace InnoGotchiWebAPI.Logic
{
    public class InnoGotchiDBPetService : IInnoGotchiDBPetService
    {
        private readonly InnoGotchiContext _context;
        public InnoGotchiDBPetService(InnoGotchiContext context)
        {
            _context = context;
        }

        public async Task<bool> PetExists(Guid id)
        {
            return await _context.Pets.AnyAsync(p => p.Id == id);
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

                // Todo: check if this deletes automatically on cascade
                //
                //var usersPetModel = _context.UsersPets.First(u => u.PetId == id);
                //_context.UsersPets.Remove(usersPetModel);

                await _context.SaveChangesAsync();
            }

            return toDelete;
        }

        public async Task<DbPetModel> GetPet(Guid id)
        {
            DbPetModel? toReturn = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);

            return toReturn ?? throw new InnoGotchiException("Can't find specified pet", 404);
        }

        public async Task<IEnumerable<DbPetModel>> GetPets(string userlogin)
        {
            IEnumerable<DbPetModel> pets = await _context.Pets.Where(p => p.OwnerId == userlogin).AsNoTracking().ToListAsync();
            return pets;
        }

        public async Task PostPet(DbPetModel pet, string userlogin)
        {
            _context.Pets.Add(pet);

            DbUsersPetModel userPetModel = new()
            {
                PetId = pet.Id,
                UserLogin = userlogin
            };

            _context.UsersPets.Add(userPetModel);

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
