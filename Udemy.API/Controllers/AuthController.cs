using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repository, IConfiguration config)
        {
            this.repository = repository;
            this._config = config;
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


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForRegisterDTO model)
        {
            var user = await repository.Login(model.UserName, model.Password);

            if (user == null) return Unauthorized();


            //Create Jwt Token
            var token = Helpers.AuthHelper.CreateJwtToken(user.Id.ToString(), user.UserName, _config.GetSection("AppSettings:Token").Value);

            return Ok(new { token = token, user });
        }
    }
}