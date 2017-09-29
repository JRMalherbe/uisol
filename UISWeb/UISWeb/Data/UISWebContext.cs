using Microsoft.EntityFrameworkCore;
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

        public DbSet<Client> Client { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerRequest> CustomerRequest { get; set; }
        public DbSet<CustomerFile> CustomerFile { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CustomerFile>()
                    .HasOne(f => f.Request)
                    .WithMany(r => r.Reports)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
