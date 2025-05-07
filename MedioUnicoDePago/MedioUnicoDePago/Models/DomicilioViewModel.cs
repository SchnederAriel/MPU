using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedioUnicoDePago.Models
{
    public class DomicilioViewModel
    {
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string TipoZona { get; set; }
    }
}