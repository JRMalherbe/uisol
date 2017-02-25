using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UISOL.Models
{
    public class Customer
    {
        [Key, StringLength(255)]
        public string Email { get; set; }
        [StringLength(255)]
        public string ContactName { get; set; }
        [StringLength(255)]
        public string CompanyName { get; set; }
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public virtual ICollection<CustomerFile> Reports { get; set; }
    }
}