﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrARRRy.Models
{
    public class Search
    {
        [Key]
        public int SearchId { get; set; }
        public string Content { get; set; }
        [ForeignKey("Reader")]
        public int ReaderId { get; set; }
        public virtual Reader Reader { get; set; }
    }
}