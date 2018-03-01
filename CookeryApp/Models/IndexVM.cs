using CookeryApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CookeryApp.Models
{
    public class IndexVM
    {
        //injeção de dependencia no ctor
        public IndexVM()
        {
            Events = new List<Event>();
        }

        public List<Event> Events { get; set; }
    }
}