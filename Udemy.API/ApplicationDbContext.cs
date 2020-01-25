using Microsoft.EntityFrameworkCore;
using Udemy.API.Models;

namespace Udemy.API
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Value> Values { get; set; }
    }
}