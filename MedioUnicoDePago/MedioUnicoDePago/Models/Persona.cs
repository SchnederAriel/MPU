using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedioUnicoDePago.Models
{
    public class Persona
    {
        public Persona() { }

        public string apellidoYNombre { get; set; }

        public string tipoDocumento { get; set; }

        public string numeroDocumento { get; set; }

        public string calle { get; set; }

        public string numero { get; set; }

        public string piso { get; set; }

        public string departamento { get; set; }

        public string localidad { get; set; }

        public string codigoPostal { get; set; }

        public string provincia { get; set; }

        public string sexo { get; set; }

        public int cod_falleci { get; set; }

        public DateTime? f_fallecimiento { get; set; }

        public DateTime f_nacimiento { get; set; }

        public string cuil { get; set; }

        public string email { get; set; }

        public DateTime fechaDePresentacion { get; set; }

        public DateTime fechaDeNacimiento { get; set; }

        public bool TieneErrores { get; set; }

        public string mensaje { get; set; }

        public bool esMenor16 { get; set; }
        public bool esMenor18 { get; set; }


        public bool depositoVigente { get; set; }

        public bool renunciaVigente { get; set; }

        public bool incapacitado { get; set; }

        public bool apoderadoVigente { get; set; }

        public int codEstado { get; set; }

        public bool mdpVigente { get; set; }

        public string descEstadoGrpCtrl { get; set; }

        public int edad { get; set; }

        public Persona(string apeynom, string tipoDoc, string numDoc, string codPostal, string provincia, string sexo, int codFalle, DateTime? fFalle, DateTime fNac, string cuil, DateTime fechaPresentacion, bool error, string mensajes, bool menor16, bool menor18, string email, bool incap, bool apodVig, int codEst, bool mdp, string descEst, int edad)
        {
            this.apellidoYNombre = apeynom;
            this.tipoDocumento = tipoDoc;
            this.numeroDocumento = numDoc;
            this.codigoPostal = codPostal;
            this.provincia = provincia;
            this.sexo = sexo;
            this.cod_falleci = codFalle;
            this.f_fallecimiento = fFalle;
            this.f_nacimiento = fNac;
            this.cuil = cuil;
            this.fechaDePresentacion = fechaPresentacion;
            this.TieneErrores = error;
            this.mensaje = mensajes;
            this.esMenor16 = menor16;
            this.esMenor18 = menor18;

            this.email = email;
            this.incapacitado = incap;
            this.apoderadoVigente = apodVig;
            this.codEstado = codEst;
            this.mdpVigente = mdp;
            this.descEstadoGrpCtrl = descEst;
            this.edad = edad;
        }
    }
}
