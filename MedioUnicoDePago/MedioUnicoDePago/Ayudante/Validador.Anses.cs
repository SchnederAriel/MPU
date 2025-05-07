using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedioUnicoDePago.Ayudante
{
    public static class Validador
    {
        public static bool CuilFormato(string cuilIngresado)
        {
            var valido = false;

            if (cuilIngresado.Length != 11)
                return false;

            valido = !string.IsNullOrWhiteSpace(cuilIngresado) && cuilIngresado.All(c => c >= '0' && c <= '9');

            if (!valido)
                return false;

            var cuil = (cuilIngresado).ToCharArray();
            double count = ((cuil[0] - '0') * 5) + ((cuil[1] - '0') * 4) + ((cuil[2] - '0') * 3) + ((cuil[3] - '0') * 2) +
                           ((cuil[4] - '0') * 7) + ((cuil[5] - '0') * 6) + ((cuil[6] - '0') * 5) + ((cuil[7] - '0') * 4) + ((cuil[8] - '0') * 3) + ((cuil[9] - '0') * 2) + ((cuil[10] - '0') * 1);
            var division = count / 11;

            return division == Math.Floor(division);
        }

        /*
         * Recibe un periodo MMAAAA
         * Se devuelve el periodo AAAAMM
         */
        public static string MesAnio2aaaaMM(string periodo)
        {
            return string.Format("{0}{1}", periodo.Substring(2, 4), periodo.Substring(0, 2));
        }

        /*
         * Recibe un periodo AAAAMM
         * Se devuelve el periodo MM/AAAA
         */
        public static string AnioMes2MMaaaa(string periodo) 
        {
            return string.IsNullOrEmpty(periodo)?null:periodo.Substring(4, 2) + "/" + periodo.Substring(0, 4);
        }

        /*
         * Recibe un periodo MM/AAAA
         * Se devuelve el periodo AAAAMM
         */
        public static string MesSlashAnioMMaaaa(string periodo)
        {
            return string.IsNullOrEmpty(periodo) ? null:periodo.Substring(periodo.IndexOf("/") + 1) + periodo.Substring(0, periodo.IndexOf("/"));
        }
    }
}