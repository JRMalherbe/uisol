using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using UISOL.Models;

namespace UISOL.Controllers
{
    public class CustomerController : ApiController
    {
        // GET: api/Customer
        public IEnumerable<Customer> Get()
        {
            List<Customer> customers = new List<Customer>();

            using (OleDbConnection connection = new OleDbConnection(ConfigurationManager.AppSettings["UISContext"]))
            {
                connection.Open();
                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT [E-mail], [ID], [CONTACT NAME], [COMPANY NAME] from Customers", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer()
                    {
                        Email = reader[0].ToString(),
                        Id = Int32.Parse(reader[1].ToString()),
                        ContactName = reader[2].ToString(),
                        CompanyName = reader[3].ToString()
                    });
                    //Console.WriteLine(reader[0].ToString() + "," + reader[1].ToString());
                }
            }

            return customers;
        }

        // GET: api/Customer/5
        public Customer Get(string id)
        {
            string email = Encoding.ASCII.GetString(Convert.FromBase64String(id));
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

            return customer;
        }

        // POST: api/Customer
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Customer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Customer/5
        public void Delete(int id)
        {
        }
    }
}
