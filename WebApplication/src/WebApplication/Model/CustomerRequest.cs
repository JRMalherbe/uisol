using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UIS
{
    public class CustomerRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LabNo { get; set; }
        public virtual ICollection<CustomerFile> Reports { get; set; }
    }
}
