using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibrARRRy.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }
        [Required]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        [Required]
        public string ReaderId { get; set; }
        public virtual ApplicationUser Reader { get; set; }

        [Required]
        public DateTime LoanedDate { get; set; }
        [Required]
        public DateTime LoanExpireDate { get; set; }    // When reader should return book
        public DateTime? ReturnedDate { get; set; }  // When reader actually returns book
    }
}