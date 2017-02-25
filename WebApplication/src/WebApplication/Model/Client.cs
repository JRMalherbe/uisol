using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIS
{
    public class Client
    {
        [Key, StringLength(255)]
        public string Email { get; set; }
        [StringLength(255)]
        public string ContactName { get; set; }
        [StringLength(255)]
        public string CompanyName { get; set; }
        public int ClientId { get; set; }
    }
}
