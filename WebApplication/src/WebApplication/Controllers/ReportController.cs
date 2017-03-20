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
    public class ReportController : Controller
    {
        private static HttpClient _client = new HttpClient();
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();
            string url = "http://localhost:50209/api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "/Reports";
            HttpResponseMessage result = _client.GetAsync(url).Result;
            string body = "";
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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
