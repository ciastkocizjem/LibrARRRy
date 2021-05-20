using LibrARRRy.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.ViewModel
{
    public class UsersViewModels
    {
        public ApplicationUser User { get; set; }
        public IdentityRole Role { get; set; }
        public IEnumerable<SelectListItem> AllRoles { get; set; }
        public IdentityRole SelectedRole { get; set; }

        public UsersViewModels()
        {
            AllRoles = new List<SelectListItem>();
        } 
    }
}