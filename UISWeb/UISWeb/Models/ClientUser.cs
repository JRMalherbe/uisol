using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UISWeb.Models
{
    public class ClientUser
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [StringLength(255)]
        public string UserEmail { get; set; }
        public User User { get; set; }
    }
}
        