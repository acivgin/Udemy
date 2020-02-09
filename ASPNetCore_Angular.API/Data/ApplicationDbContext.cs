using Microsoft.EntityFrameworkCore;
using ASPNetCore_Angular.API.Models;

namespace ASPNetCore_Angular.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }

    }
}