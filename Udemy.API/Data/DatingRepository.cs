using System.Threading.Tasks;
using Udemy.API.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Udemy.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly ApplicationDbContext context;
        public DatingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            return await context.Users.Include(u => u.Photos).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await context.Users.Include(u => u.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            var result = await context.SaveChangesAsync();

            return result == 1;
        }

    }
}