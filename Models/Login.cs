using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Epicotel.Models
{
    public class Login
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "L'Username deve avere min. 3, max 30 caratteri/numeri")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "La Password deve avere min. 4, max 12 caratteri/numeri")]
        public string Password { get; set; }
    }
}