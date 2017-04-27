using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIS
{
    public class UISContext : DbContext
    {
        public UISContext(DbContextOptions<UISContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Login> Login { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<CustomerRequest> CustomerRequest { get; set; }
        public DbSet<CustomerFile> CustomerFile { get; set; }
    }
}