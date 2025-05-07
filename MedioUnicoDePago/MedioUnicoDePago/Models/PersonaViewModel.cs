using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedioUnicoDePago.Models
{
    public class PersonaViewModel
    {
        public string Cuil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string PaisResidencia { get; set; }
        public DomicilioViewModel Domicilio { get; set; }
        public bool PuedeIngresarMiAnses { get; set; }
        public string MedioPagoActual { get; set; }
        public bool TieneError { get; set; }
        public string MensajeError { get; set; }
    }
}