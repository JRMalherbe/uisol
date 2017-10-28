using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UISWeb.Models
{
    public class User
    {
        [Key, StringLength(255)]
        public string Email { get; set; }

        public virtual ICollection<ClientUser> Clients { get; set; }
    }
}
