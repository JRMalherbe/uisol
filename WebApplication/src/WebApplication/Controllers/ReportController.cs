using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.IO;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace UIS.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private static HttpClient _client = new HttpClient();
        private readonly UISContext _db;
        private readonly UISConfig _config;

        public ReportController(UISContext db, IOptions<UISConfig> config)
        {
            _db = db;
            _config = config.Value;
        }

        [HttpGet, Route("{email}/Request/{labno}/File/{name}", Name = "GetReportFile")]
        public async Task<FileContentResult> GetReportFile(string email, int labno, string name)
        {
            //string linkName = Encoding.ASCII.GetString(Convert.FromBase64String(name));

            CustomerFile customerFile = _db.CustomerFile.Where(x => x.CustomerRequestLabNo == labno && x.LinkName == name).SingleOrDefault();
            string fileName = "Not Found.pdf";
            if (customerFile != null)
                fileName = customerFile.FileName;
            string filePath = _config.ReportPath + fileName;

            byte[] byteResult;
            FileStream SourceStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read);
            byteResult = new byte[SourceStream.Length];
            await SourceStream.ReadAsync(byteResult, 0, (int)SourceStream.Length);
            return File(byteResult, "application/octet-stream", fileName); // FileStreamResult
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
    }
}
