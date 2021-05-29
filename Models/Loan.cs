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
        [Display(Name = "Borrowed Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanedDate { get; set; }    // when reader loaned on website
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Collection Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CollectionDate { get; set; }    // when reader collected book
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Borrow Expire Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanExpireDate { get; set; }    // When reader should return book
        [DataType(DataType.Date)]
        [Display(Name = "Returned Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnedDate { get; set; }  // When reader actually returns book
    }
}