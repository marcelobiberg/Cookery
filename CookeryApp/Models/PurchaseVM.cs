using CookeryApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CookeryApp.Models
{
    public class PurchaseVM 
    {
        public Event events { get; set; }

        [Required]
        public Ticket ticket { get; set; }
    }
}