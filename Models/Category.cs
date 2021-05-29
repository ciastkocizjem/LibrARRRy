using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibrARRRy.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        [ForeignKey("ParentCategory")]
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
    }
}