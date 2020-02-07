using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Udemy.API.Data;
using Udemy.API.DTOs;
using Udemy.API.Models;

namespace Udemy.API.Controllers {
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IDatingRepository repo;
        private readonly IMapper mapper;
        public UsersController (IDatingRepository repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers () {
            var users = await repo.GetUsers ();

            var result = mapper.Map<IEnumerable<UserForListDto>> (users);

            return Ok (result);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetUser (int id) {
            var user = await repo.GetUser (id);

            if (user == null) return NotFound ();

            var result = mapper.Map<UserForDetailsDto> (user);

            return Ok (result);
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateUser (int id, UserForUpdateDto model) {

            //Check the id of logged in user
            if (id != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();

            var user = await repo.GetUser (id);

            if (user == null) return NotFound ();
            user.LastActive = DateTime.Now;

            mapper.Map (model, user);

            if (await repo.SaveAll ())
                return NoContent ();

            throw new Exception ($"Updating user {id} failed on save");
        }

    }
}