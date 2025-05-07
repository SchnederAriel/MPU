using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedioUnicoDePago.Models
{
    public class DTOMedioPago2
    {
        public decimal Cuil { get; set; }
        public string TipoMedioPago { get; set; }
        public decimal? CbuInicio { get; set; }
        public decimal? CbuFinal { get; set; }
        public short? CBanco { get; set; }
        public short? CAgencia { get; set; }
        public decimal PeInicioPago { get; set; }
        public decimal? Cvu1 { get; set; }
        public decimal? Cvu2 { get; set; }
        public string Alias { get; set; }
        public string IpOrigen { get; set; }
        public decimal? Udai { get; set; }
        public string OpeTramite { get; set; }

        // Este método ayuda a obtener el valor correcto basado en el TipoMedioPago
        public string ObtenerValor()
        {
            switch (TipoMedioPago)
            {
                case "CBU":
                    return $"{CbuInicio}{CbuFinal}";
                case "CVU":
                    return $"{Cvu1}{Cvu2}";
                case "Banco/Agencia":
                    return $"{CBanco}/{CAgencia}";
                case "Alias":
                    return Alias;
                default:
                    return null;
            }
        }


    }
}