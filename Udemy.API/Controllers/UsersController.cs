using System.IO.Compression;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Udemy.API.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Udemy.API.DTOs;
using System.Collections.Generic;
using AutoMapper;

namespace Udemy.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository repo;
        private readonly IMapper mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await repo.GetUsers();

            var result = mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await repo.GetUser(id);

            if(user == null) return NotFound();

            var result = mapper.Map<UserForDetailsDto>(user);

            return Ok(result);
        }

    }
}