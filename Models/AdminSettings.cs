using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibrARRRy.Models
{
    public class AdminSettings
    {
        [Key]
        public int SettingId { get; set; }
        [Required]
        public int BorrowedBooksLimit { get; set; }
        [Required]
        public int DetentionLimit { get; set; }
        [Required]
        [StringLength(200)]
        public string AdminInfo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime InfoAdditionDate { get; set; }

    }
}