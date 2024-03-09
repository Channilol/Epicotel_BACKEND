using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicotel.Models
{
    public class Checkout
    {
        public int IdPrenotazione { get; set; }

        public int IdCamera { get; set; }

        public string Periodo { get; set; }

        public decimal Tariffa { get; set; }

        public string TipoPensione { get; set; }

        public decimal PrezzoPensione { get; set; }

        public decimal PrezzoCheckout { get; set; }

        public string Caparra { get; set; }

        public int IdServizio { get; set; }

        public string TipoServizio { get; set; }

        public decimal PrezzoServizio { get; set; }

        public string DataServizio { get; set; }

    }
}