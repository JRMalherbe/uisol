﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UISOL.Models
{
    public class CustomerFile
    {
        [Key, StringLength(255)]
        public string FileName { get; set; }
        [StringLength(255)]
        public string LinkName { get; set; }
    }
}