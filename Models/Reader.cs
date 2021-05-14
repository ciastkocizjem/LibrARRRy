using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibrARRRy.Models
{
    public class Reader
    {
        [Key]
        public int ReaderId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Book> BooksRead { get; set; }
        public virtual ICollection<Search> SearchHistory { get; set; }
        public virtual ICollection<Loan> Loaned { get; set; }
    }
}