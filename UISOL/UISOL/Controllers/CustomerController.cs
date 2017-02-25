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

        [Route("api/Customer/{id}/Reports")]
        public IEnumerable<CustomerRequest> GetReports(string id)
        {
            string email = Encoding.ASCII.GetString(Convert.FromBase64String(id));
            Customer customer = new Customer();
            List<CustomerRequest> reports = new List<CustomerRequest>();
            using (OleDbConnection connection = new OleDbConnection(ConfigurationManager.AppSettings["UISContext"]))
            {
                connection.Open();

                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ID, [CONTACT NAME], [COMPANY NAME], [E-mail] from Customers where [E-mail] = '" + email + "'", connection);
                //OleDbCommand command = new OleDbCommand("SELECT ID, [CONTACT NAME], [COMPANY NAME], [E-mail] from Customers where id = " + key.ToString(), connection);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    customer = new Customer()
                    {
                        Id = Int32.Parse(reader[0].ToString()),
                        ContactName = reader[1].ToString(),
                        CompanyName = reader[2].ToString(),
                        Email = reader[3].ToString()
                    };

                    reader = null;
                    command = new OleDbCommand("SELECT [LAB_NO], [REQUEST COORDINATOR ID], [DETAIL], [DATUM], [REQUIRED DATE], [AFGEHANDEL], [INVOICED] from Requests where [CUST ID] = " + customer.Id.ToString(), connection);

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new CustomerRequest()
                        {
                            LabNo = Int32.Parse(reader[0].ToString()),
                            CustomerId = customer.Id,
                            Coordinator = reader[1].ToString(),
                            Detail = reader[2].ToString(),
                            Received = DateTime.Parse(reader[3].ToString()),
                            Required = DateTime.Parse(reader[4].ToString()),
                            Completed = Boolean.Parse(reader[5].ToString()),
                            Invoiced = Boolean.Parse(reader[6].ToString())
                        });
                    }
                }
            }

            return reports;
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
