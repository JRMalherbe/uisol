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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UIS.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private static HttpClient _client = new HttpClient();
        private readonly UISContext _db;
        private readonly UISConfig _config;

        public ClientController(UISContext db, IOptions<UISConfig> config)
        {
            _db = db;
            _config = config.Value;
        }
        
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            string userName = HttpContext.Request.Headers["UserName"].ToString();
            string userRole = HttpContext.Request.Headers["UserRole"].ToString();
            
            string url = _config.ServiceRoot + "api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName));
            if (userRole == "Admin")
                url = _config.ServiceRoot + "api/Customer";
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
            string url = _config.ServiceRoot + "api/Customer('" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "')";
            if (userRole == "Admin")
                url = _config.ServiceRoot + "api/Customer('" + email + "')";
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
            string url = _config.ServiceRoot + "api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "/Reports";
            if (userRole == "Admin")
                url = _config.ServiceRoot + "api/Customer/" + email + "/Reports";
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
            url = _config.ServiceRoot + "api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "/Reports/" + labno.ToString();
            if (userRole == "Admin")
                url = _config.ServiceRoot + "api/Customer/" + email + "/Reports/" + labno.ToString();
            result = _client.GetAsync(url).Result;
            body = "";
            if (result.IsSuccessStatusCode)
            {
                body = result.Content.ReadAsStringAsync().Result;

                CustomerRequest req = _db.CustomerRequest.Find(labno);
                if (req != null)
                {
                    List<CustomerFile> files = _db.CustomerFile.Where(x => x.CustomerRequestLabNo == labno).ToList();
                    _db.CustomerFile.RemoveRange(files);
                    _db.SaveChanges();

                    _db.CustomerRequest.Remove(req);
                    _db.SaveChanges();
                }

                CustomerRequest nreq = JsonConvert.DeserializeObject<CustomerRequest>(body);
                _db.CustomerRequest.Add(nreq);
                _db.SaveChanges();

                Response.Headers.Add("Content-Type", "application/json");
                return body;
            }
            NotFound();
            return null;
        }

        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }

        /*
        [HttpGet, Route("{email}/Request/{labno}/File/{name}", Name = "GetFile")]
        public FileStreamResult GetFile(string email, int labno, string name)
        {
            string fileName = Encoding.ASCII.GetString(Convert.FromBase64String(name));
            string filePath = @"C:\p\reports\" + fileName;
            var stream = new FileStream(filePath, FileMode.Open);
            string contentType = "application/pdf";
            //HttpContext.Response.ContentType = contentType;
            //HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + fileName });
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return new FileStreamResult(stream, contentType);
        }

        [HttpGet, Route("{email}/Request/{labno}/File/{name}", Name = "GetFile5")]
        public IActionResult GetFile5(string email, int labno, string name)
        {
            //string userName = HttpContext.Request.Headers["UserName"].ToString();
            //string userRole = HttpContext.Request.Headers["UserRole"].ToString();

            //string url = "";
            //HttpResponseMessage result = null;
            //url = "http://localhost:50209/api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(userName)) + "/Reports/" + labno.ToString();
            //if (userRole == "Admin")
            //    url = "http://localhost:50209/api/Customer/" + email + "/Reports/" + labno.ToString();
            //authresult = _client.GetAsync(url).Result;
            //if (!authresult.IsSuccessStatusCode)
            //    return NotFound();

            string fileName = Encoding.ASCII.GetString(Convert.FromBase64String(name));
            string filePath = @"C:\p\reports\" + fileName;
            string contentType = "application/pdf";
            HttpContext.Response.ContentType = contentType;
            var result = new FileContentResult(System.IO.File.ReadAllBytes(filePath), contentType)
            {
                FileDownloadName = fileName
            };

            return result;
        }

        //[HttpGet("{email}/Request/{labno}/File/{name}")]
        [HttpGet, Route("{email}/Request/{labno}/File2/{name}", Name = "GetFile2")]
        public FileResult GetFile2(string email, int labno, string name)
        {
            string fileName = Encoding.ASCII.GetString(Convert.FromBase64String(name));
            string filePath = @"C:\p\reports\" + fileName;

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + fileName });
            return File(fileBytes, "application/pdf", fileName);
        }

        [HttpGet, Route("{email}/Request/{labno}/File3/{name}", Name = "GetFile3")]
        //[HttpGet("{email}/Request/{labno}/File/{name}")]
        public async Task<IActionResult> GetFile3(string email, int labno, string name)
        {
            string fileName = Encoding.ASCII.GetString(Convert.FromBase64String(name));
            string filePath = @"C:\p\reports\" + fileName;

            var data = await KryLeerData(filePath);
            using (var leerStroom = new FileStream(filePath, FileMode.Create))
            {
                await leerStroom.WriteAsync(data, 0, data.Length);
            }

            if (data == null)
                return NotFound();

            //Response.Headers.Add("Content-Type", "application/octet-stream");
            Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + fileName });
            return File(data, "application/octet-stream");
        }

        [HttpGet, Route("{email}/Request/{labno}/File4/{name}", Name = "GetFile4")]
        //[HttpGet("{email}/Request/{labno}/File/{name}")]
        public async Task<FileContentResult> GetFile4(string email, int labno, string name)
        {
            string fileName = Encoding.ASCII.GetString(Convert.FromBase64String(name));
            string filePath = @"C:\p\reports\" + fileName;

            byte[] byteResult;
            FileStream SourceStream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read);
            byteResult = new byte[SourceStream.Length];
            await SourceStream.ReadAsync(byteResult, 0, (int)SourceStream.Length);
            return File(byteResult, "application/octet-stream", fileName ); // FileStreamResult
        }

        private async Task<byte[]> KryLeerData(string pad)
        {
            if (!System.IO.File.Exists(pad))
                return null;

            using (var leerStroom = new FileStream(pad, FileMode.Open))
            {
                var data = new byte[leerStroom.Length];
                var gelees = await leerStroom.ReadAsync(data, 0, (int)leerStroom.Length);
                return data;
            }
        }
        */

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
