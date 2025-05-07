using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using log4net;
using System.Reflection;

namespace MedioUnicoDePago.Ayudante
{
    public class Validador2
    {
        public static ValidationResult ValidateEndTimeRange(DateTime fecha)
        {

            if (fecha > DateTime.Now && fecha != DateTime.MinValue)
            {
                return new ValidationResult("La fecha es mayor a la actual");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateStartTimeRange(DateTime fecha)
        {

            if (DateTime.Now >= fecha.AddYears(2) && fecha != DateTime.MinValue)
            {
                return new ValidationResult("La fecha tiene más de dos años de antiguedad");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult FechaValida(string date)
        {
            DateTime tempDate;
            if (!DateTime.TryParse(date, out tempDate))
            {
                return new ValidationResult("Fecha invalida");
            }

            return ValidationResult.Success;

        }

        public static ValidationResult ValidarCUIL(string cuilIngresado)
        {
            if (cuilIngresado == null) {
                return ValidationResult.Success;
            }
            var cuil = (cuilIngresado).ToCharArray();
            double count = ((cuil[0] - '0') * 5) + ((cuil[1] - '0') * 4) + ((cuil[2] - '0') * 3) + ((cuil[3] - '0') * 2) +
                           ((cuil[4] - '0') * 7) + ((cuil[5] - '0') * 6) + ((cuil[6] - '0') * 5) + ((cuil[7] - '0') * 4) + ((cuil[8] - '0') * 3) + ((cuil[9] - '0') * 2) + ((cuil[10] - '0') * 1);
            var division = count / 11;

            if (!(division == Math.Floor(division)))
            {
                return new ValidationResult("CUIL no válido");
            }

            return ValidationResult.Success;

        }

        public static ValidationResult Validarcbu(string cbu)
        {
            //validacion de cbu
            var array = cbu.Split('-');
            string cbu1 = array[0];
            string cbu2 = array[1];

            long Sumacbu1 = new long();
            long Sumacbu2 = new long();
            long diferencial1 = 10;
            long diferencial2 = 10;

            bool valido = false;

            //BLOQUE 1
            //separo los 8 digitos del primer rango del cbu
            var Cbu1Digitos = cbu1.ToString().ToCharArray();

            //calculo la suma de los codigos
            Sumacbu1 = long.Parse(Cbu1Digitos[0].ToString()) * 7 + long.Parse(Cbu1Digitos[1].ToString()) * 1 + long.Parse(Cbu1Digitos[2].ToString()) * 3 + long.Parse(Cbu1Digitos[3].ToString()) * 9
                        + long.Parse(Cbu1Digitos[4].ToString()) * 7 + long.Parse(Cbu1Digitos[5].ToString()) * 1 + long.Parse(Cbu1Digitos[6].ToString()) * 3;

            //separo los digitos de la suma para obtener el ultimo
            var arraySuma1 = Sumacbu1.ToString().ToCharArray();
            //le resto el ultimo digito de la suma al diferencial
            diferencial1 = diferencial1 - long.Parse(arraySuma1[arraySuma1.Length - 1].ToString());

            //si el diferencial es igual al digito validador o el digito el digito es 0 y el diferencial es 10
            if ((long.Parse(Cbu1Digitos[7].ToString()) != 0 && long.Parse(Cbu1Digitos[7].ToString()) == diferencial1) || (long.Parse(Cbu1Digitos[7].ToString()) == 0 && diferencial1 == 10))
            {
                //BLOQUE 2
                var Cbu2Digitos = cbu2.ToString().ToCharArray();
                //calculo la suma de los codigos
                Sumacbu2 = long.Parse(Cbu2Digitos[0].ToString()) * 3 + long.Parse(Cbu2Digitos[1].ToString()) * 9 + long.Parse(Cbu2Digitos[2].ToString()) * 7 + long.Parse(Cbu2Digitos[3].ToString()) * 1
                            + long.Parse(Cbu2Digitos[4].ToString()) * 3 + long.Parse(Cbu2Digitos[5].ToString()) * 9 + long.Parse(Cbu2Digitos[6].ToString()) * 7 + long.Parse(Cbu2Digitos[7].ToString()) * 1
                            + long.Parse(Cbu2Digitos[8].ToString()) * 3 + long.Parse(Cbu2Digitos[9].ToString()) * 9 + long.Parse(Cbu2Digitos[10].ToString()) * 7 + long.Parse(Cbu2Digitos[11].ToString()) * 1
                            + long.Parse(Cbu2Digitos[12].ToString()) * 3;

                //separo los digitos de la suma para obtener el ultimo
                var arraySuma2 = Sumacbu2.ToString().ToCharArray();
                //le resto el ultimo digito de la suma al diferencial
                diferencial2 = diferencial2 - long.Parse(arraySuma2[arraySuma2.Length - 1].ToString());

                //si el diferencial es igual al digito validador o el digito el digito es 0 y el diferencial es 10
                if ((long.Parse(Cbu2Digitos[13].ToString()) != 0 && long.Parse(Cbu2Digitos[13].ToString()) == diferencial2) || (long.Parse(Cbu2Digitos[13].ToString()) == 0 && diferencial2 == 10))
                {
                    //es valido
                    valido = true;
                }
                //else
                //{
                //    //es invalido
                //    return false;
                //}
            }
            //else
            //{
            //    //es invalido
            //    return false;
            //}
            if (!valido)
            {
                return new ValidationResult("cbu inválido");
            }
            else
            {
                return ValidationResult.Success;
            }

        }

        /*
         * Recibe un periodo AAAAMM
         * Se devuelve el periodo MM/AAAA
         */
        public static string AnioMes2MMaaaa(string periodo)
        {
            try
            {
                return (string.IsNullOrEmpty(periodo) || (periodo.Length != 6)) ? periodo : periodo.Substring(4, 2) + "/" + periodo.Substring(0, 4);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}