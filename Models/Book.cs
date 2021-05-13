using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibrARRRy.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime AdditionDate { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Reader> Readers { get; set; }
    }
}