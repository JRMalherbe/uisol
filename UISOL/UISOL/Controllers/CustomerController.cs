using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using UISOL.Models;

namespace UISOL.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using UISOL.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Customer>("Customer");
    builder.EntitySet<CustomerFile>("ClientFile"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CustomerController : ODataController
    {
        private UISContext db = new UISContext();

        // GET: odata/Customer
        [EnableQuery]
        public IQueryable<Customer> GetCustomer()
        {
            return db.Customer;
        }

        // GET: odata/Customer(5)
        //[EnableQuery]
        public IHttpActionResult GetCustomer([FromODataUri] string key)
        {
            string email = Encoding.ASCII.GetString(Convert.FromBase64String(key));
            Customer customer = new Customer();
            using (OleDbConnection connection = new OleDbConnection(ConfigurationManager.AppSettings["UISContext"]))
            {
                connection.Open();
                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ID, [CONTACT NAME], [COMPANY NAME], [E-mail] from Customers where [E-mail] = '" + email + "'", connection);
                //OleDbCommand command = new OleDbCommand("SELECT ID, [CONTACT NAME], [COMPANY NAME], [E-mail] from Customers where id = " + key.ToString(), connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customer = new Customer()
                    {
                        Id = Int32.Parse(reader[0].ToString()),
                        ContactName = reader[1].ToString(),
                        CompanyName = reader[2].ToString(),
                        Email = reader[3].ToString()
                    };
                }
            }

            return Ok<Customer>(customer);
        }

        // PUT: odata/Customer(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<Customer> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = db.Customer.Find(key);
            if (customer == null)
            {
                return NotFound();
            }

            patch.Put(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customer);
        }

        // POST: odata/Customer
        public IHttpActionResult Post(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customer.Add(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(customer);
        }

        // PATCH: odata/Customer(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<Customer> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = db.Customer.Find(key);
            if (customer == null)
            {
                return NotFound();
            }

            patch.Patch(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customer);
        }

        // DELETE: odata/Customer(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            Customer customer = db.Customer.Find(key);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customer.Remove(customer);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Customer(5)/Reports
        [EnableQuery]
        public IQueryable<CustomerFile> GetReports([FromODataUri] string key)
        {
            return db.Customer.Where(m => m.Email == key).SelectMany(m => m.Reports);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(string key)
        {
            return db.Customer.Count(e => e.Email == key) > 0;
        }
    }
}
