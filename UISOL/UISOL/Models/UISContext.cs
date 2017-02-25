using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Configuration;

namespace UISOL.Models
{
    public class UISContext: DbContext
    {
        public UISContext() : base(ConfigurationManager.AppSettings["UISContext"])
        {

        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerFile> CustomerFile { get; set; }
        public virtual DbSet<CustomerRequest> CustomerRequest { get; set; }
    }
}