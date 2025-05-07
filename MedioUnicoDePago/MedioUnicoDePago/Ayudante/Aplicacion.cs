using System;
using System.Configuration;

namespace MedioUnicoDePago
{
    public class Aplicacion
    {
        static string _version;
        static string _ambienteApp;
        static string _nombreAplicacion;
        static bool _loguearActividad = true;
        static bool _validarCoelsa = true;
        static bool _verPrenatal = false;
        static bool _verMaternidad = false;
        static bool _validarCredencialPrevia = false;
        static string _operador = "";


        public enum ModulosLUPA
        {
            Apoderado = 21,
            Prenatal = 10,            
            Maternidad = 11,
            Decreto614 = 35,
            DepositoJudicial = 22,
            MediosDePago = 24,
            RenunciaAlCobro = 27,
            DerechoHabiente = 37,
            Reclamos = 25,
            Decreto840 = 39
        }

        public enum SistemasAAFF
        {
            TODOS = 1,                        
            UVHI = 60,
            SUAF = 14,
            CUNA = 69
        }

        public Aplicacion()
        {
            _version = ConfigurationManager.AppSettings["version"];
            _ambienteApp = ConfigurationManager.AppSettings["ambiente"];
            //_nombreAplicacion = ConfigurationManager.AppSettings["nombreaplicacion"];
            _loguearActividad = bool.Parse(String.IsNullOrEmpty(ConfigurationManager.AppSettings["LogActividad"])?"false": ConfigurationManager.AppSettings["LogActividad"].ToString());
            _validarCoelsa = bool.Parse(String.IsNullOrEmpty(ConfigurationManager.AppSettings["COELSA"])?"false": ConfigurationManager.AppSettings["COELSA"].ToString());
            _operador = ConfigurationManager.AppSettings["operador_tramitedecreto"];
        }

        public static string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }

        public static string operador
        {
            get
            {
                return _operador;
            }
            set
            {
                _operador = value;
            }
        }
        
        public static string Ambiente
        {
            get
            {
                return _ambienteApp.Trim().ToUpper();
            }
            set
            {
                _ambienteApp = value.Trim().ToUpper();
            }
        }

        public static string Nombre
        {
            get
            {
                return _nombreAplicacion;
            }
            set
            {
                _nombreAplicacion = value;
            }
        }

        public static bool LogApp
        {
            get
            {
                return _loguearActividad;
            }
            set
            {
                _loguearActividad = value;
            }
        }

        public static bool ValidarCoelsa
        {
            get
            {
                return _validarCoelsa;
            }
            set
            {
                _validarCoelsa = value;
            }
        }

        public static bool verPrenatal
        {
            get
            {
                return _verPrenatal;
            }
            set
            {
                _verPrenatal = value;
            }
        }

        public static bool verMaternidad
        {
            get
            {
                return _verMaternidad;
            }
            set
            {
                _verMaternidad = value;
            }
        }

        public static bool validarCredencialPrevia
        {
            get
            {
                return _validarCredencialPrevia;
            }
            set
            {
                _validarCredencialPrevia = value;
            }
        }
    }
}