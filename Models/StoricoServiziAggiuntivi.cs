using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicotel.Models
{
    public class StoricoServiziAggiuntivi
    {
        public int IdStorico { get; set; }

        public int IdPrenotazione { get; set; }

        public int IdServizio { get; set; }

        public string DataServizio { get; set; }

        public decimal PrezzoTotale { get; set; }
    }
}