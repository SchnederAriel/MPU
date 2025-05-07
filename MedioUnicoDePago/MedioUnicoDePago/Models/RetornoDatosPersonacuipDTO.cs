using System;

namespace MedioUnicoDePago.Models
{
    public class RetornoDatosPersonaCuipDTO
    {
        public ErrorDTO Error { get; set; }
        public PersonaCuipDTO PersonaCuip { get; set; }
    }

    public class ErrorDTO
    {
        public int CodRetorno { get; set; }
        public string DescMensaje { get; set; }
        public string CodError { get; set; }
        public string CodGravedad { get; set; }
    }

    public class PersonaCuipDTO
    {
        public DateTime FCambio { get; set; }
        public int DocCTipo { get; set; }
        public int DocOrigen { get; set; }
        public DateTime FNaci { get; set; }
        public DateTime CpFDesde { get; set; }
        public DateTime CpFHasta { get; set; }
        public int CpDocResp { get; set; }
        public DateTime FIngPais { get; set; }
        public int CodCompIngPais { get; set; }
        public int CodNacion { get; set; }
        public int CodEstcivil { get; set; }
        public int CodIncap { get; set; }
        public DateTime FFalle { get; set; }
        public int CodFalleci { get; set; }
        public DateTime FActuFalle { get; set; }
        public int FPeriodoMm { get; set; }
        public int FPeriodoAa { get; set; }
        public int CodTipoDom { get; set; }
        public int CodCompDomi { get; set; }
        public DateTime FActuDomi { get; set; }
        public DateTime FResidencia { get; set; }
        public int DomiCodPostal { get; set; }
        public int DomiCodSubpostal { get; set; }
        public int DomiCodPcia { get; set; }
        public int DomiCodDatAdic { get; set; }
        public int DomiCodCompPaisExtr { get; set; }
        public int DomiCodPaisExtr { get; set; }
        public int TelediscadoPais { get; set; }
        public int Telediscado { get; set; }
        public int Telefono { get; set; }
        public DateTime FActuTel { get; set; }
        public int CodEstGrcon { get; set; }
        public int CodEstEnte { get; set; }
        public int CodStmaOrigen { get; set; }
        public DateTime FAlta { get; set; }
        public DateTime FUltModi { get; set; }
        public DateTime HUltModi { get; set; }
        public int CodAltaStmaOrigen { get; set; }
        public int CodAltaEnteInf { get; set; }
        public DateTime AltaFAlta { get; set; }
        public int ModCSistOrig { get; set; }
        public int ModCEnteInfo { get; set; }
        public int PPeriodoMm { get; set; }
        public int PPeriodoAa { get; set; }
        public int CCompaniaTelefono { get; set; }
    }
}


