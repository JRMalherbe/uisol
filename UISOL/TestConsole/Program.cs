using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Entity;
using System.Data.OleDb;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var db = new UISOL.Models.UISContext())
            //{
            //    ;
            //}

            using (OleDbConnection connection = new OleDbConnection(ConfigurationManager.AppSettings["UISContext"]))
            {
                connection.Open();
                OleDbDataReader reader = null;
                OleDbCommand command = new OleDbCommand("SELECT ID, [CONTACT NAME] from Customers where ID=3", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString() + "," + reader[1].ToString());
                }
            }
            Console.ReadLine();
        }
    }
}
