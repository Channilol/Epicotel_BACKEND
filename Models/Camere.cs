using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epicotel.Models
{
    public class Camere
    {
        public int IdCamera { get; set; }   

        public bool Tipologia {  get; set; }

        public string Descrizione { get; set; }

        public decimal Caparra { get; set; }
    }
}