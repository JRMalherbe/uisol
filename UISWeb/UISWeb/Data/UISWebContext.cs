﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UISWeb.Models;

namespace UISWeb.Data
{
    public class UISWebContext: DbContext
    {
        public UISWebContext(DbContextOptions<UISWebContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerRequest> CustomerRequest { get; set; }
        public DbSet<CustomerFile> CustomerFile { get; set; }

    }
}
