using InnoGotchiWebAPI.Database;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using InnoGotchiWebAPI.Exceptions;

namespace InnoGotchiWebAPI.Logic
{
    public class DBService : IDBService
    {
        private readonly InnoGotchiContext _context;
        public DBService(InnoGotchiContext context)
        {
            _context = context;
        }

        public async Task<bool> PetExists(Guid id)
        {
            return await _context.Pets.AnyAsync(p => p.Id == id);
        }
        public async Task<DbPetModel> DeletePet(Guid id, string userlogin)
        {
            DbPetModel? toDelete = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);

            if (toDelete == null)
            {
                throw new InnoGotchiNotFoundException();
            }
            else 
            {
                if (toDelete?.OwnerId != userlogin)
                {
                    throw new InnoGotchiNotYourPetException();
                }

                // Note that dbUserPetsModel deletes on cascade
                _context.Pets.Remove(toDelete);
                await _context.SaveChangesAsync();
            }

            return toDelete;
        }

        public async Task<DbPetModel> GetPet(Guid id, string userlogin)
        {
            DbPetModel? toReturn = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);

            if (toReturn?.OwnerId != userlogin)
            {
                throw new InnoGotchiNotYourPetException();
            }

            return toReturn ?? throw new InnoGotchiNotFoundException();
        }

        public async Task<IEnumerable<DbPetModel>> GetPets(string userlogin)
        {
            IEnumerable<DbPetModel> pets = await _context.Pets.Where(p => p.OwnerId == userlogin).AsNoTracking().ToListAsync();
            return pets;
        }

        public async Task AddPet(DbPetModel pet)
        {
            if(await PetExists(pet.Id))
            {
                throw new InnoGotchiDBException("Specified pet already exists. Use update", 409);
            }

            _context.Pets.Add(pet);

            DbUsersPetModel userPetModel = new()
            {
                PetId = pet.Id,
                UserLogin = pet.OwnerId
            };

            _context.UsersPets.Add(userPetModel);

            await _context.SaveChangesAsync();
        }

        public async Task<DbPetModel> UpdatePet(DbPetModel pet, string userlogin)
        {
            DbPetModel? toEdit = await _context.Pets.FirstOrDefaultAsync(p => p.Id == pet.Id)
                ?? throw new InnoGotchiNotFoundException();

            if (toEdit?.OwnerId != userlogin)
            {
                throw new InnoGotchiNotYourPetException();
            }

            _context.Entry(toEdit).CurrentValues.SetValues(pet);

            await _context.SaveChangesAsync();

            return toEdit;
        }


        public async Task<bool> UserExists(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }
        public async Task<DbUserModel> DeleteUser(string login)
        {
            DbUserModel? toDelete = await _context.Users.FirstOrDefaultAsync(p => p.Login == login);

            if (toDelete == null)
            {
                throw new InnoGotchiNotFoundException();
            }
            else
            {
                _context.Users.Remove(toDelete);
                await _context.SaveChangesAsync();
            }

            return toDelete;
        }

        public async Task<DbUserModel> GetUser(string login)
        {
            DbUserModel? toReturn = await _context.Users.FirstOrDefaultAsync(p => p.Login == login);

            return toReturn ?? throw new InnoGotchiNotFoundException();
        }

        public async Task<IEnumerable<DbUserModel>> GetUsers()
        {
            IEnumerable<DbUserModel> users = await _context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        public async Task AddUser(DbUserModel user)
        {
            if (await UserExists(user.Login))
            {
                throw new InnoGotchiDBException("Specified user already exists. Use update", 409);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<DbUserModel> UpdateUser(DbUserModel user)
        {
            DbUserModel? toEdit = await _context.Users.FirstOrDefaultAsync(p => p.Login == user.Login)
                ?? throw new InnoGotchiNotFoundException();

            _context.Entry(toEdit).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();

            return toEdit;
        }
    }
}
