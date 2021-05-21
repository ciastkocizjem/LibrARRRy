using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibrARRRy.ViewModel
{
    public class ConfirmReadersViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
    }
}