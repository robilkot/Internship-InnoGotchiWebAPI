using InnoGotchiWebAPI.Database;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using InnoGotchiWebAPI.Exceptions;

namespace InnoGotchiWebAPI.Logic
{
    public class InnoGotchiDBUserService : IInnoGotchiDBUserService
    {
        private readonly InnoGotchiContext _context;
        public InnoGotchiDBUserService(InnoGotchiContext context)
        {
            _context = context;
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
                throw new InnoGotchiException("Can't find specified user", 404);
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

            return toReturn ?? throw new InnoGotchiException("Can't find specified user", 404);
        }

        public async Task<IEnumerable<DbUserModel>> GetUsers()
        {
            IEnumerable<DbUserModel> users = await _context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        public async Task PostUser(DbUserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<DbUserModel> PutUser(DbUserModel user)
        {
            DbUserModel? toEdit = await _context.Users.FirstOrDefaultAsync(p => p.Login == user.Login)
                ?? throw new InnoGotchiException("Can't find specified user", 404);

            _context.Entry(toEdit).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();

            return toEdit;
        }
    }
}
