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

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientFile> ClientFile { get; set; }
    }
}