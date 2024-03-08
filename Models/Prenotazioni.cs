using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Epicotel.Models
{
    public class Prenotazioni
    {
        [Key]
        public int IdPrenotazione { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        [Range(1, 2147483647, ErrorMessage = "Devi scegliere un cliente")]
        public int IdCliente { get; set; }

        [Required]
        [Display(Name = "Camera")]
        [Range(1, 2147483647, ErrorMessage = "Devi scegliere una camera")]
        public int IdCamera { get; set; }

        [Required]
        [Display(Name = "Pensione")]
        [Range(1, 2147483647, ErrorMessage = "Devi scegliere un tipo di pensione")]
        public int IdPensione { get; set; }

        [Required]
        [Display(Name = "Data Prenotazione")]
        public string DataPrenotazione {  get; set; }

        [Required]
        [Display(Name = "Soggiorno Da")]
        public string SoggiornoDa {  get; set; }

        [Required]
        [Display(Name = "Soggiorno A")]
        public string SoggiornoA {  get; set; }

        [Required]
        [Display(Name = "Tariffa")]
        public decimal Tariffa { get; set; }

    }

}