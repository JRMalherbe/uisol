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
    }
}