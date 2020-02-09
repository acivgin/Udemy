using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Udemy.API.DTOs;
using Udemy.API.Models;

namespace Udemy.API.Data {
    public class AuthRepository : IAuthRepository {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public AuthRepository (ApplicationDbContext dbContext, IMapper mapper) {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserForListDto> Login (string username, string password) {

            var user = await _dbContext.Users.Include (p => p.Photos).FirstOrDefaultAsync (x => x.UserName == username);
            if (user == null)
                return null;

            if (!Helpers.AuthHelper.VerifyPasswordHash (password, user.PasswordHash, user.PasswordSalt))
                return null;

            return _mapper.Map<UserForListDto> (user);
        }

        public async Task<User> Register (User user, string password) {

            byte[] passwordHash, passwordSalt;
            Helpers.AuthHelper.CreatePasswordHash (password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dbContext.Users.AddAsync (user);
            await _dbContext.SaveChangesAsync ();

            return user;
        }

        public async Task<bool> UserExists (string username) {

            if (await _dbContext.Users.AnyAsync (x => x.UserName == username))
                return true;

            return false;
        }

    }
}