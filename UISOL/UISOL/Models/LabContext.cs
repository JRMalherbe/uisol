using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Configuration;

namespace UISOL.Models
{
    public class LabContext: DbContext
    {
        public LabContext() : base(ConfigurationManager.AppSettings["Context"])
        {

        }

        public virtual DbSet<User> User { get; set; }
    }
}