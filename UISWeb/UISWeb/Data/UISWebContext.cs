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
        public DbSet<ClientUser> ClientUser { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerRequest> CustomerRequest { get; set; }
        public DbSet<CustomerFile> CustomerFile { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CustomerFile>()
                    .HasOne(f => f.Request)
                    .WithMany(r => r.Reports)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ClientUser>()
                    .HasKey(bc => new { bc.ClientId, bc.UserEmail });

            builder.Entity<ClientUser>()
                .HasOne(bc => bc.Client)
                .WithMany(b => b.Users)
                .HasForeignKey(bc => bc.ClientId);

            builder.Entity<ClientUser>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.Clients)
                .HasForeignKey(bc => bc.UserEmail);

        }
    }
}
