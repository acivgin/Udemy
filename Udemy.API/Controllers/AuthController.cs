using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Udemy.API.Data;
using Udemy.API.DTOs;
using Udemy.API.Models;

namespace Udemy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        public AuthController(IAuthRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegister)
        {
            //validate request

            if (await repository.UserExists(userForRegister.UserName.ToLower()))
                return BadRequest("Username already exists!");

            var userToCreate = new User
            {
                UserName = userForRegister.UserName.ToLower()
            };

            var createduser = await repository.Register(userToCreate, userForRegister.Password);

            return StatusCode(201);
        }

    }
}