using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIS
{
    public class Login
    {
        [Key, StringLength(100)]
        public string Email { get; set; }
        [StringLength(80)]
        public string Password { get; set; }
    }
}
