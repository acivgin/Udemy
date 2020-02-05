using System.Linq;
using Newtonsoft.Json;
using Udemy.API.Models;
using System.Collections.Generic;

namespace Udemy.API.Data
{
    public static class Seed
    {
        private const string data = "Data/UserSeedData.json";

        public static void SeedUsers(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText(data);
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.UserName = user.UserName.ToLower();
                    context.Users.Add(user);
                }
                context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}