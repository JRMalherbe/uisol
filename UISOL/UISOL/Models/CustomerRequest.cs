using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UISOL.Models
{
    public class CustomerRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LabNo { get; set; }
        public int CustomerId { get; set; }
        [StringLength(100)]
        public string Coordinator { get; set; }
        [StringLength(100)]
        public string Detail { get; set; }
        public DateTime Received { get; set; }
        public DateTime Required { get; set; }
        public bool Completed { get; set; }
        public bool Invoiced { get; set; }
        public string CustomerName { get; set; }
        public string LoadedMS { get; set; }
        public string LoadedFID { get; set; }
        public int Progress { get; set; }


        public virtual ICollection<CustomerFile> Reports { get; set; }
    }
}