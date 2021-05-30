using LibrARRRy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.ViewModel
{
    public class BooksViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<SelectListItem> AllAuthors { get; set; }
        public IEnumerable<SelectListItem> AllTags { get; set; }
        public List<int> SelectedAuthors { set; get; }
        public List<int> SelectedTags { set; get; }

        public BooksViewModel()
        {
            AllAuthors = new List<SelectListItem>();
            AllTags = new List<SelectListItem>();
            SelectedAuthors = new List<int>();
            SelectedTags = new List<int>();
        }
    }
}