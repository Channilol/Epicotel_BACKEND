using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Epicotel.Models
{
    public class Clienti
    {
        [Key]
        public int IdCliente { get; set; }

        [Required]
        [Display(Name ="Cognome")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Il cognome deve avere min. 3, max 50 caratteri")]
        public string Cognome { get; set; }

        [Required]
        [Display(Name = "Nome")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Il nome deve avere min. 3, max 50 caratteri")]
        public string Nome {  get; set; }

        [Required]
        [Display(Name = "Città")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "La città deve avere min. 3, max 50 caratteri")]
        public string Citta { get; set; }

        [Required]
        [Display(Name = "Provincia")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "La provincia deve essere composta da 2 caratteri")]
        public string Provincia { get; set; }

        [Required]
        [Display(Name = "Email")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "L'email deve avere min. 3, max 200 caratteri")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public int Telefono { get; set; }

        [Required]
        [Display(Name = "Cellulare")]
        public int Cellulare { get; set; }

        [Required]
        [Display(Name = "CodiceFiscale")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve avere 16 caratteri/numeri")]
        public string CodiceFiscale {  get; set; }

    }
}