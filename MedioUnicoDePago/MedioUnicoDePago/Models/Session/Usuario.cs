using System;

namespace MedioUnicoDePago.Models.Session
{
    public class Usuario
    {
        public Usuario()
        {
            HasToken = false;
        }

        public bool HasToken { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CUIL { get; set; }
        public string Oficina { get; set; }
        public string OficinaDetalle { get; set; }
        public string IP { get; set; }
        public bool ControlIP { get; set; }
        public DateTime ExpiraToken { get; set; }
        public string Perfil { get; set; }
        public string Sistema { get; set; }
        public string[] Grupos { get; set; }
    }
}