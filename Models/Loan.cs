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
        [DataType(DataType.Date)]
        public DateTime LoanedDate { get; set; }    // when reader loaned on website
        [Required]
        [DataType(DataType.Date)]
        public DateTime CollectionDate { get; set; }    // when reader collected book
        [Required]
        [DataType(DataType.Date)]
        public DateTime LoanExpireDate { get; set; }    // When reader should return book
        [DataType(DataType.Date)]
        public DateTime? ReturnedDate { get; set; }  // When reader actually returns book
    }
}