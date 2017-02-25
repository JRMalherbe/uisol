using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UIS.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private static HttpClient _client = new HttpClient();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();
            //string email = "claris.dreyer@kumbaresources.com";
            //byte[] toBytes = Encoding.ASCII.GetBytes(somestring);
            //string something = Encoding.ASCII.GetString(toBytes);
            //WebUtility.UrlEncode();
            //WebUtility.UrlDecode();
            string url = "http://localhost:50209/odata/Customer('" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "')";
            //string url = "http://localhost:50209/odata/Customer('" + id + "')";
            HttpResponseMessage result = _client.GetAsync(url).Result;
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
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}
