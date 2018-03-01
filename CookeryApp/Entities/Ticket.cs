using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CookeryApp.Entities
{
    public class Ticket : BaseID
    {
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [DataType(DataType.Text)]
        public string FName { get; set; }

        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [DataType(DataType.Text)]
        public string LName { get; set; }

        [MaxLength(200, ErrorMessage = "Máximo 200 caracteres.")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        public string PaypalReference { get; set; }

        //properties navigation
        [ForeignKey("Event")]
        public int Event_Id { get; set; }
        public Event Event { get; set; }

    }
}