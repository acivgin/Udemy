using System.Threading.Tasks;
using Udemy.API.DTOs;
using Udemy.API.Models;

namespace Udemy.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<UserForListDto> Login(string username, string password);
        Task<bool> UserExists(string username);

    }
}