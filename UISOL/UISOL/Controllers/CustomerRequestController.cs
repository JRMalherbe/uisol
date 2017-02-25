using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UISOL.Controllers
{
    public class CustomerRequestController : ApiController
    {
        // GET: api/CustomerRequest
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CustomerRequest/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CustomerRequest
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/CustomerRequest/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CustomerRequest/5
        public void Delete(int id)
        {
        }
    }
}
