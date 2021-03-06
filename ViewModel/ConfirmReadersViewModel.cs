using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LibrARRRy.ViewModel
{
    public class ConfirmReadersViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Role { get; set; }
        [DisplayName("Confirmed")]
        public bool EmailConfirmed { get; set; }
        [DisplayName("Cash penalty")]
        public bool CashPenalty { get; set; }
    }
}