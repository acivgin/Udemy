using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.API;
using Udemy.API.Models;

namespace UdemyWebAPI_Angular8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {       
        private ApplicationDbContext _context; 
        public ValuesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<List<Value>> Get()
        {
            return _context.Values.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Value> Get(int id)
        {
            return _context.Values.FirstOrDefault(v=>v.Id == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
