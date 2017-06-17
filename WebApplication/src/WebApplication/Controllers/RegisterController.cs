using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UIS.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private static HttpClient _client = new HttpClient();
        private readonly UISContext _db;
        private readonly UISConfig _config;

        public RegisterController(UISContext db, IOptions<UISConfig> config)
        {
            _db = db;
            _config = config.Value;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{username}")]
        public string Get(string username)
        {
            Login login = _db.Login.Find(username);
            if (login == null)
            {
                NotFound();
                return null;
            }
            NoContent();
            return null;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Login item)
        {
            if (item == null)
                return BadRequest();

            Login login = _db.Login.Find(item.Email);
            if (login == null)
            {
                var sha2 = System.Security.Cryptography.SHA256.Create();
                var hash = sha2.ComputeHash(Encoding.UTF8.GetBytes(item.Password));
                var pswhash = BitConverter.ToString(hash).Replace("-", "");
                item.Password = pswhash;
                _db.Login.Add(item);
                _db.SaveChanges();
                return NoContent();
            }  
            return BadRequest();
            //return CreatedAtRoute("Register", new { id = item.Email }, item);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
