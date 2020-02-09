using System.Threading.Tasks;
using ASPNetCore_Angular.API.DTOs;
using ASPNetCore_Angular.API.Models;

namespace ASPNetCore_Angular.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<UserForListDto> Login(string username, string password);
        Task<bool> UserExists(string username);

    }
}