using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using UISOL.Models;
using System.IO;
using System.Net.Http.Headers;
using System.Data.OleDb;
using System.Web.Http.OData.Query;

namespace UISOL
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using UISOL.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Client>("Client");
    builder.EntitySet<ClientFile>("ClientFile"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ClientController : ODataController
    {
        private UISContext db = new UISContext();

        // GET: odata/Client
        //[EnableQuery]
        public IHttpActionResult GetClient(ODataQueryOptions<Client> queryOptions)
        {
            List<Client> clients = new List<Client>();

            using (OleDbConnection connection = new OleDbConnection(ConfigurationManager.AppSettings["UISContext"]))
            {
                connection.Open();
                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ID, [CONTACT NAME], [COMPANY NAME], [E-mail] from Customers", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new Client()
                    {
                        Id = Int32.Parse(reader[0].ToString()),
                        ContactName = reader[1].ToString(),
                        CompanyName = reader[2].ToString(),
                        Email = reader[3].ToString()
                    });
                    //Console.WriteLine(reader[0].ToString() + "," + reader[1].ToString());
                }
            }

            return Ok<IEnumerable<Client>>(clients);
        }

        // GET: odata/Client(5)
        //[EnableQuery]
        public IHttpActionResult GetClient([FromODataUri] int key)
        {
            Client client = new Client();

            using (OleDbConnection connection = new OleDbConnection(ConfigurationManager.AppSettings["UISContext"]))
            {
                connection.Open();
                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ID, [CONTACT NAME], [COMPANY NAME], [E-mail] from Customers where id = " + key.ToString(), connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    client = new Client()
                    {
                        Id = Int32.Parse(reader[0].ToString()),
                        ContactName = reader[1].ToString(),
                        CompanyName = reader[2].ToString(),
                        Email = reader[3].ToString()
                    };
                }
            }

            return Ok<Client>(client);

            //return SingleResult.Create(client);
        }

        // PUT: odata/Client(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Client> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Client client = db.Client.Find(key);
            if (client == null)
            {
                return NotFound();
            }

            patch.Put(client);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(client);
        }

        // POST: odata/Client
        public IHttpActionResult Post(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Client.Add(client);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClientExists(client.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(client);
        }

        // PATCH: odata/Client(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Client> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Client client = db.Client.Find(key);
            if (client == null)
            {
                return NotFound();
            }

            patch.Patch(client);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(client);
        }

        // DELETE: odata/Client(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Client client = db.Client.Find(key);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Client(5)/File
        //[EnableQuery]
        //public SingleResult<ClientFile> GetFile([FromODataUri] int key)
        //public IHttpActionResult GetFile([FromODataUri] int key)
        //{
            //return SingleResult.Create(db.Client.Where(m => m.Id == key).Select(m => m.File));
            //return StatusCode(HttpStatusCode.NoContent);
        //}
        public HttpResponseMessage GetFile([FromODataUri] int key)
        {
            //if (String.IsNullOrEmpty(key))
            //    return StatusCode(HttpStatusCode.NoContent);

            string fileName = "Toets.pdf";
            string localFilePath;
            long fileSize;

            localFilePath = ConfigurationManager.AppSettings["PdfLocation"].ToString() + "Toets.pdf";
            FileInfo fileInfo = new FileInfo(localFilePath);
            fileSize = fileInfo.Length;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int key)
        {
            return db.Client.Count(e => e.Id == key) > 0;
        }
    }
}
