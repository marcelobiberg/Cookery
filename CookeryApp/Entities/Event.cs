using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CookeryApp.Entities
{
    public class Event : BaseID
    {
        [MaxLength(50,ErrorMessage ="Máximo 50 caracteres.")]
        [MinLength(4,ErrorMessage = "Mínimo 4 caracteres.")]
        [DataType(DataType.Text)]
        public string Title { get; set; }
        
        [MaxLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        public string ShortDescription { get; set; }

        [MaxLength(1200, ErrorMessage = "Máximo 800 caracteres.")]
        public string LongDescription { get; set; }

        [MaxLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [DataType(DataType.Text)]
        public string ImageURL { get; set; }

        [DataType(DataType.Currency)]
        public int Price { get; set; }

        //property navigation
        public List<Ticket> Tickets { get; set; }






    }
}