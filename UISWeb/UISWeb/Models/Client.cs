using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UISWeb.Models
{
    public class Client
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClientId { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(255)]
        public string ContactName { get; set; }
        [StringLength(255)]
        public string CompanyName { get; set; }

        public virtual ICollection<ClientUser> Users { get; set; }
    }
}
