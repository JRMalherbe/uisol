using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Net;
using Microsoft.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UIS.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private static HttpClient _client = new HttpClient();
        // GET: api/values
        [HttpGet]
        public IEnumerable<Client> Get()
        {
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();

            string url = "http://localhost:50209/api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName));
            if (userRole == "Admin")
                url = "http://localhost:50209/api/Customer";

            List<Client> clients = new List<Client>();

            HttpResponseMessage result = _client.GetAsync(url).Result;
            string body = "";
            if (result.IsSuccessStatusCode)
            {
                body = result.Content.ReadAsStringAsync().Result;
            }

            return clients;
        }

        // GET api/values/5
        [HttpGet("{email}")]
        public string Get(string email)
        {
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();
            //string email = "claris.dreyer@kumbaresources.com";
            //byte[] toBytes = Encoding.ASCII.GetBytes(somestring);
            //string something = Encoding.ASCII.GetString(toBytes);
            //WebUtility.UrlEncode();
            //WebUtility.UrlDecode();
            string url = "http://localhost:50209/api/Customer('" + email + "')";
            HttpResponseMessage result = _client.GetAsync(url).Result;
            string body = "";
            //return result;
            
            if (result.IsSuccessStatusCode)
            {
                body = result.Content.ReadAsStringAsync().Result;
                Response.Headers.Add("Content-Type", "application/json");
                return body;
            }
            NotFound();
            return null;
        }

        // GET api/values/5
        [HttpGet("{email}/Request")]
        public string GetRequest(string email)
        {
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();
            //string email = "claris.dreyer@kumbaresources.com";
            //byte[] toBytes = Encoding.ASCII.GetBytes(somestring);
            //string something = Encoding.ASCII.GetString(toBytes);
            //WebUtility.UrlEncode();
            //WebUtility.UrlDecode();
            string url = "http://localhost:50209/api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "/Reports";
            HttpResponseMessage result = _client.GetAsync(url).Result;
            string body = "";
            //return result;

            if (result.IsSuccessStatusCode)
            {
                body = result.Content.ReadAsStringAsync().Result;
                Response.Headers.Add("Content-Type", "application/json");
                return body;
            }
            NotFound();
            return null;
        }

        // GET api/values
        [HttpGet("{email}/Request/{labno}")]
        public string GetRequest(string email, int labno)
        {
            NotFound();
            return null;
        }

        // GET api/values
        [HttpGet("{email}/Request/{labno}/Reports")]
        public string GetReport(string email, int labno)
        {
            NotFound();
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
