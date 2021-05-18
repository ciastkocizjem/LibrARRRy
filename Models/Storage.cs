﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibrARRRy.Models
{
    public class Storage
    {
        [Key]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        [Required]
        public int Amount { get; set; } // All copies of book
        [Required]
        public int CurrentAmount { get; set; }  // Currently in storage
    }
}