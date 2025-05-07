using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedioUnicoDePago.Models
{
    public class MedioPagoViewModel
    {
        public string PeEmision { get; set; }
        public string PeLiquidado { get; set; }
        public string NombreBanco { get; set; }
        public string NombreAgencia { get; set; }
        public string CBU { get; set; }
        public string CVU { get; set; }
        public string MPago { get; set; }
        public string TipoMedioPago { get; set; }
        public string CodigoBanco { get; set; }
        public string CodigoAgencia { get; set; }

        // Propiedades de conveniencia
        public bool TieneCBU => !string.IsNullOrEmpty(CBU);
        public bool TieneCVU => !string.IsNullOrEmpty(CVU);
        public bool EsBanco => TipoMedioPago == "Banco";

        public string DisplayValue
        {
            get
            {
                if (TieneCBU)
                    return $"CBU: {CBU}";
                if (TieneCVU)
                    return $"CVU: {CVU}";
                if (EsBanco)
                    return $"Banco: {NombreBanco} - Agencia: {NombreAgencia}";
                return MPago;
            }
        }
    }


}