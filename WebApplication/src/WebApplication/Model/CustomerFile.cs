using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UIS
{
    public class CustomerFile
    {
        [Key, StringLength(255)]
        public string FileName { get; set; }
        [StringLength(255)]
        public string LinkName { get; set; }

        [ForeignKey("Request"), Required]
        public int CustomerRequestLabNo { get; set; }
        public CustomerRequest Request { get; set; }
    }
}
