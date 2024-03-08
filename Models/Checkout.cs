using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicotel.Models
{
    public class Checkout
    {
        public int IdPrenotazione { get; set; }

        public decimal PrezzoCheckout { get; set; }
    }
}