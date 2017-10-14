using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
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
                        ClientId = Int32.Parse(reader[1].ToString()),
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
                        ClientId = Int32.Parse(reader[0].ToString()),
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
                        ClientId = Int32.Parse(reader[0].ToString()),
                        ContactName = reader[1].ToString(),
                        CompanyName = reader[2].ToString(),
                        Email = reader[3].ToString()
                    };

                    string query = @"SELECT Requests.[LAB_NO], Requests.[REQUEST COORDINATOR ID], Requests.[DETAIL], Requests.[DATUM], Requests.[REQUIRED DATE], Requests.[AFGEHANDEL], Requests.[INVOICED], Requests.[CUST ID],
C.[CONTACT NAME],
P.[TotalPreps], P.[1stPrep], P.[2ndPrep], P.[Completed], p.[MS], p.[FID]
FROM(Requests
LEFT JOIN Customers AS C ON C.ID = Requests.[CUST ID])
LEFT JOIN(SELECT[Assigned hours].LAB_NO,
COUNT(*) AS [TotalPreps],
SUM(iif([Assigned hours].[1st_Prep_Completed] = True,1,0)) AS[1stPrep], 
SUM(iif([Assigned hours].[2nd_Prep_Completed] = True, 1, 0)) AS[2ndPrep], 
SUM(iif([Assigned hours].[Processed_Completed] = True, 1, 0)) AS[Completed], 
MAX([Assigned hours].Loaded_MS) AS[MS], 
MAX([Assigned hours].Loaded_FID) AS[FID]
FROM[Assigned hours]
WHERE[Assigned hours].LAB_NO IS NOT NULL
GROUP BY[Assigned hours].LAB_NO) AS P ON P.LAB_NO = Requests.LAB_NO
WHERE Requests.[CUST ID] IN({{=CustomerIds}})
ORDER BY Requests.DATUM DESC";

                    reader = null;
                    CustomerRequest request = null;
                    //command = new OleDbCommand("SELECT [LAB_NO], [REQUEST COORDINATOR ID], [DETAIL], [DATUM], [REQUIRED DATE], [AFGEHANDEL], [INVOICED], [CUST ID] from Requests where [CUST ID] = " + customer.ClientId.ToString(), connection);
                    command = new OleDbCommand(query.Replace("{{=CustomerIds}}", customer.ClientId.ToString()), connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        /*
                        t1 = reader[9].ToString();//total
                        t1 = reader[10].ToString();//1st
                        t1 = reader[11].ToString();//2nd
                        t1 = reader[12].ToString();//Completed
                        */
                        int tempProgress = 0;
                        if (!String.IsNullOrEmpty(reader[9].ToString())) {
                            decimal tempTotal = Int32.Parse(reader[9].ToString());
                            decimal tempValue = Int32.Parse(reader[12].ToString());
                            if (tempTotal > 0)
                                tempProgress = (Int32)Math.Round(((tempValue * 100) / tempTotal),0);
                        }

                        request = new CustomerRequest()
                        {
                            LabNo = Int32.Parse(reader[0].ToString()),
                            Coordinator = reader[1].ToString(),
                            Detail = reader[2].ToString(),
                            Received = DateTime.Parse(reader[3].ToString()),
                            Required = DateTime.Parse(reader[4].ToString()),
                            Completed = Boolean.Parse(reader[5].ToString()),
                            Invoiced = Boolean.Parse(reader[6].ToString()),
                            CustomerId = customer.ClientId,
                            CustomerName = reader[8].ToString(),
                            LoadedMS = reader[13].ToString(),
                            LoadedFID = reader[14].ToString(),
                            Progress = tempProgress,
                            Reports = new List<CustomerFile>()
                        };
                        reports.Add(request);
                    }
                }
                connection.Close();
            }
            return reports;
        }

        [Route("api/Customer/{id}/Reports/{labno}")]
        public CustomerRequest GetReport(string id, int labno)
        {
            string email = Encoding.ASCII.GetString(Convert.FromBase64String(id));
            Customer customer = new Customer();
            CustomerRequest request = null;
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
                        ClientId = Int32.Parse(reader[0].ToString()),
                        ContactName = reader[1].ToString(),
                        CompanyName = reader[2].ToString(),
                        Email = reader[3].ToString()
                    };

                    string query = @"SELECT Requests.[LAB_NO], Requests.[REQUEST COORDINATOR ID], Requests.[DETAIL], Requests.[DATUM], Requests.[REQUIRED DATE], Requests.[AFGEHANDEL], Requests.[INVOICED], Requests.[CUST ID], 
C.[CONTACT NAME], 
P.[TotalPreps], P.[1stPrep], P.[2ndPrep], P.[Completed], p.[MS], p.[FID] 
FROM(Requests 
LEFT JOIN Customers AS C ON C.ID = Requests.[CUST ID]) 
LEFT JOIN(SELECT[Assigned hours].LAB_NO, 
COUNT(*) AS [TotalPreps], 
SUM(iif([Assigned hours].[1st_Prep_Completed] = True,1,0)) AS [1stPrep], 
SUM(iif([Assigned hours].[2nd_Prep_Completed] = True, 1, 0)) AS [2ndPrep], 
SUM(iif([Assigned hours].[Processed_Completed] = True, 1, 0)) AS [Completed], 
MAX([Assigned hours].Loaded_MS) AS [MS], 
MAX([Assigned hours].Loaded_FID) AS [FID] 
FROM[Assigned hours] 
WHERE[Assigned hours].LAB_NO IS NOT NULL 
GROUP BY[Assigned hours].LAB_NO) AS P ON P.LAB_NO = Requests.LAB_NO 
WHERE Requests.[CUST ID] IN ({{=CustomerIds}}) AND Requests.[LAB_NO] = {{=LabNo}} 
ORDER BY Requests.DATUM DESC";

                    reader = null;
                    //command = new OleDbCommand("SELECT [LAB_NO], [REQUEST COORDINATOR ID], [DETAIL], [DATUM], [REQUIRED DATE], [AFGEHANDEL], [INVOICED], [CUST ID] from Requests where [CUST ID] = " + customer.ClientId.ToString() + " and [LAB_NO] = " + labno.ToString(), connection);
                    command = new OleDbCommand(query.Replace("{{=CustomerIds}}", customer.ClientId.ToString()).Replace("{{=LabNo}}", labno.ToString()), connection);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        /*
                        t1 = reader[9].ToString();//total
                        t1 = reader[10].ToString();//1st
                        t1 = reader[11].ToString();//2nd
                        t1 = reader[12].ToString();//Completed
                        */
                        int tempProgress = 0;
                        if (!String.IsNullOrEmpty(reader[9].ToString()))
                        {
                            decimal tempTotal = Int32.Parse(reader[9].ToString());
                            decimal tempValue = Int32.Parse(reader[12].ToString());
                            if (tempTotal > 0)
                                tempProgress = (Int32)Math.Round(((tempValue * 100) / tempTotal), 0);
                        }

                        request = new CustomerRequest()
                        {
                            LabNo = Int32.Parse(reader[0].ToString()),
                            Coordinator = reader[1].ToString(),
                            Detail = reader[2].ToString(),
                            Received = DateTime.Parse(reader[3].ToString()),
                            Required = DateTime.Parse(reader[4].ToString()),
                            Completed = Boolean.Parse(reader[5].ToString()),
                            Invoiced = Boolean.Parse(reader[6].ToString()),
                            CustomerId = customer.ClientId,
                            CustomerName = reader[8].ToString(),
                            LoadedMS = reader[13].ToString(),
                            LoadedFID = reader[14].ToString(),
                            Progress = tempProgress,
                            Reports = new List<CustomerFile>()
                        };
                        string reportPath = ConfigurationManager.AppSettings["ReportPath"];
                        DirectoryInfo folder = new DirectoryInfo(reportPath);
                        if (folder.Exists) // else: Invalid folder!
                        {
                            FileInfo[] files = folder.GetFiles(request.LabNo.ToString() + "*.pdf");
                            foreach (FileInfo file in files)
                            {
                                request.Reports.Add(new CustomerFile()
                                {
                                    FileName = file.Name,
                                    LinkName = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString()))
                                    //LinkName = Convert.ToBase64String(Encoding.ASCII.GetBytes(file.Name))
                                });
                            }
                        }
                    }
                }
                connection.Close();
            }
            return request;
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
