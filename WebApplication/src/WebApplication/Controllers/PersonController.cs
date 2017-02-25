using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography;
using System.Text;
using System.Net;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UIS.Controllers
{

    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private static HttpClient Client = new HttpClient();
        private readonly static List<Person> persons = new[] { new Person { Naam = "Hans", Van = "Malherbe" }, new Person { Naam = "Tanya", Van = "Kotze" } }.ToList();
        // GET: api/values
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return persons;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Person Get(string key)
        {
            string email = "claris.dreyer@kumbaresources.com";
            //byte[] toBytes = Encoding.ASCII.GetBytes(somestring);
            //string something = Encoding.ASCII.GetString(toBytes);
            //WebUtility.UrlEncode();
            //WebUtility.UrlDecode();
            string url = "http://localhost:50209/odata/Client('" + Convert.ToBase64String(Encoding.ASCII.GetBytes(email)) + "')";

            HttpResponseMessage result = Client.GetAsync(url).Result;
            string body = "";
            if (result.IsSuccessStatusCode)
            {
                body = result.Content.ReadAsStringAsync().Result;
            }

            //return persons.Single(x => x.Id == key);
            return null;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Person value)
        {
            persons.Add(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Person value)
        {
            var person = persons.Single(x => x.Id == id);
            person.Naam = value.Naam;
            person.Van = value.Van;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            persons.RemoveAll(x => x.Id == id);
        }
    }
}
