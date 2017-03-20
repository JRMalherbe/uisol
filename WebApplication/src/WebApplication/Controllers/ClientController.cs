using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Net;
using Microsoft.Net.Http.Headers;
using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UIS.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private static HttpClient _client = new HttpClient();
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();
            string url = "http://localhost:50209/api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName));
            if (userRole == "Admin")
                url = "http://localhost:50209/api/Customer";
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
        [HttpGet("{email}")]
        public string Get(string email)
        {
            //string requestEmail = Encoding.ASCII.GetString(Convert.FromBase64String(email));
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();

            //string email = "claris.dreyer@kumbaresources.com";
            //byte[] toBytes = Encoding.ASCII.GetBytes(somestring);
            //string something = Encoding.ASCII.GetString(toBytes);
            //WebUtility.UrlEncode();
            //WebUtility.UrlDecode();
            string url = "http://localhost:50209/api/Customer('" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "')";
            if (userRole == "Admin")
                url = "http://localhost:50209/api/Customer('" + email + "')";
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
        [HttpGet("{email}/Request")]
        public string GetRequest(string email)
        {
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();
            string url = "http://localhost:50209/api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "/Reports";
            if (userRole == "Admin")
                url = "http://localhost:50209/api/Customer/" + email + "/Reports";
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

        // GET api/values
        [HttpGet("{email}/Request/{labno}")]
        public string GetRequest(string email, int labno)
        {
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();
            string url = "";
            string body = "";
            HttpResponseMessage result = null;

            url = "http://localhost:50209/api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "/Reports/" + labno.ToString();
            if (userRole == "Admin")
                url = "http://localhost:50209/api/Customer/" + email + "/Reports/" + labno.ToString();
            result = _client.GetAsync(url).Result;
            body = "";
            if (result.IsSuccessStatusCode)
            {
                body = result.Content.ReadAsStringAsync().Result;
                Response.Headers.Add("Content-Type", "application/json");
                return body;
            }
            NotFound();
            return null;
        }

        [HttpGet("{email}/Request/{labno}/File/{name}")]
        public async Task<IActionResult> GetFile(string email, int labno, string name)
        {
            string filename = @"C:\p\reports\" + Encoding.ASCII.GetString(Convert.FromBase64String(name));
            byte[] result;

            FileStream SourceStream = System.IO.File.Open(filename, FileMode.Open);
            result = new byte[SourceStream.Length];
            await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);

            //var stream = await { { __get_stream_here__} }
            var response = File(SourceStream, "application/octet-stream"); // FileStreamResult
            return response;
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
