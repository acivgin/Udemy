using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPNetCore_Angular.API.Models;

namespace ASPNetCore_Angular.API.Data {
    public class DatingRepository : IDatingRepository {
        private readonly ApplicationDbContext context;
        public DatingRepository (ApplicationDbContext context) {
            this.context = context;
        }

        public void Add<T> (T entity) where T : class {
            context.Add (entity);
        }

        public void Delete<T> (T entity) where T : class {
            context.Remove (entity);
        }

        public async Task<User> GetUser (int id) {
            return await context.Users.Include (u => u.Photos).FirstOrDefaultAsync (p => p.Id == id);
        }
        public async Task<IEnumerable<User>> GetUsers () {
            var users = await context.Users.Include (u => u.Photos).ToListAsync ();
            return users;
        }

        public async Task<bool> SaveAll () {
            return await context.SaveChangesAsync () != 0;
        }

        public async Task<Photo> GetPhoto (int id) {
            return await context.Photos.FirstOrDefaultAsync (p => p.Id == id);
        }

        public async Task<Photo> GetMainPhoto (int userId) {
            return await context.Photos.Where (u => u.UserId == userId).FirstOrDefaultAsync (p => p.IsMain);
        }
    }
}