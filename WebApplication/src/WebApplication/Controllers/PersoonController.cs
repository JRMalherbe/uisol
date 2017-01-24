using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication3.Controllers
{

    [Route("api/[controller]")]
    public class PersoonController : Controller
    {
        private readonly static List<Persoon> persone = new[] { new Persoon { Naam = "Hans", Van = "Malherbe" }, new Persoon { Naam = "Tanya", Van = "Kotze" } }.ToList();
        // GET: api/values
        [HttpGet]
        public IEnumerable<Persoon> Get()
        {
            return persone;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Persoon Get(int id)
        {
            return persone.Single(x => x.Id == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Persoon value)
        {
            persone.Add(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Persoon value)
        {
            var persoon = persone.Single(x => x.Id == id);
            persoon.Naam = value.Naam;
            persoon.Van = value.Van;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            persone.RemoveAll(x => x.Id == id);
        }
    }
}
