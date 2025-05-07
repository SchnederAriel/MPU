using Anses.Director.Session;
using MedioUnicoDePago.Ayudante;
using MedioUnicoDePago.Models;
using log4net;
using Newtonsoft.Json;
using System.Configuration;
using System;
using System.Reflection;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using MedioUnicoDePago.ADPWrapperWS;
using MedioUnicoDePago.VerificarCelu;
using MedioUnicoDePago.ArgentaCWS;
using MedioUnicoDePago.GestionarMPU;
using MedioUnicoDePago.DatosdePersonaporCuip;
using MedioUnicoDePago.validarCBU;
using MedioUnicoDePago.EnviarMail;
using System.Collections.Generic;
using MedioUnicoDePago.Filtros;
using System.IO;

namespace MedioUnicoDePago.Controllers
{
    public class HomeController : ControladorBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        private DtoToken _token;
        private string cuil = "";
        private const string TITULO = "Método de Pago Único";
        private bool UDAI = bool.Parse(ConfigurationManager.AppSettings["UDAI"]);
        private int DiasNoDejaCargarMDP = int.Parse(ConfigurationManager.AppSettings["DiasNoDejaCargarMDP"]);
        private bool ValidarPersona = bool.Parse(ConfigurationManager.AppSettings["ValidarPersona"]);
        DirectorMV director = new DirectorMV();
        VerificaMailCelular VC = new VerificaMailCelular();
        ArgentaCWS.ArgentaCWS ACWS = new ArgentaCWS.ArgentaCWS();
        DatosdePersonaporCuip.DatosdePersonaporCuip DPC = new DatosdePersonaporCuip.DatosdePersonaporCuip();
        GestionarMPU.GestionMPU GMPU = new GestionarMPU.GestionMPU();
        ValidarCBU VCBU = new ValidarCBU();
      
        

        private string GetProperty(string name, DtoToken token2)
        {
            FieldInfo[] fields = token2.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo fieldInfo = fields[i];
                if (fieldInfo.Name == name)
                {
                    return (string)fieldInfo.GetValue(token2);
                }
            }

            return null;
        }

        public async Task<ActionResult> GuardarMDP()
        {
            ViewBag.Error = "0";
            string cuil = Session["CUIL"] as string;
            if (string.IsNullOrEmpty(cuil))
            {
                // Tomar de la cookie si no viene en sesión
                cuil = Request.Cookies["CUIL"]?.Value;
            }
            //LupaServicio.LupaServicio lupa = new LupaServicio.LupaServicio();
            //string retornoMail = lupa.EnviarMail();
            ViewBag.UDAI = UDAI;
            log.Debug($"ViewBag UDAI: {UDAI}");
           
            ViewBag.Title = TITULO;
            ViewBag.cuil = cuil;

            // Obtener datos de la persona
            RetornoDatosPersonaCuip datosPersona = DPC.ObtenerPersonaxCUIP(cuil);
            log.Debug("Mayor 13 años: " + EsMayorDe13Anios(Convert.ToDateTime(datosPersona.PersonaCuip.f_naci)));
            log.Debug($"Fecha nacimiento: {datosPersona.PersonaCuip.f_naci}");

            // Verificamos si es menor de 13 y si no tiene apoderado, ya retornamos con error
            if (!EsMayorDe13Anios(Convert.ToDateTime(datosPersona.PersonaCuip.f_naci)))
            {
                decimal cuilApoderado = -1;
                DTOApoderado apod = new DTOApoderado();
                apod.cuil = cuil;
                apod.p_pago = Convert.ToString(DateTime.Now.ToString("yyyyMM"));
                cuilApoderado = GMPU.ObtenerApoderado(apod);
                log.Debug($"CUIL Apoderado: {cuilApoderado}");
                if (cuilApoderado == -1)
                {
                    log.Debug($"Menor de 13 años sin apoderado");
                    ViewBag.Error = "Error. Menor de 13 años sin apoderado.";
                    return View();
                }
            }

            // -----------------------
            // 1) Calcular EDAD y setear ViewBags
            // -----------------------
            int edad = CalcularEdad(Convert.ToDateTime(datosPersona.PersonaCuip.f_naci));
            ViewBag.EsMenor13 = (edad < 13);
            log.Debug($"Viewbag EsMenor13: {(edad < 13)}");
            ViewBag.EsEntre13y18 = (edad >= 13 && edad < 18);
            log.Debug($"Viewbag EsEntre13y18: {(edad >= 13 && edad < 18)}");
            ViewBag.EsMayorIgual18 = (edad >= 18);
            log.Debug($"Viewbag EsMayorIgual18: {(edad >= 18)}");
            log.Debug($"Edad del CUIL a evaluar: "+ edad.ToString());
            // -----------------------
            // 2) Verificar APODERADO (ya se hizo arriba, pero repetimos para setear ViewBag)
            // -----------------------
            DTOApoderado apod2 = new DTOApoderado();
            apod2.cuil = cuil;
            log.Debug($"CUIL usuario evaluado: {cuil}");
            apod2.p_pago = DateTime.Now.ToString("yyyyMM");
            decimal cuilApoderado2 = GMPU.ObtenerApoderado(apod2);
            bool tieneApoderado2 = (cuilApoderado2 != -1);
            ViewBag.TieneApoderado2 = tieneApoderado2;
            log.Debug($"Viewbag TieneApoderado2: {tieneApoderado2}");
            // -----------------------
            // 3) Verificar ZONA AUSTRAL
            // -----------------------
            string mensajeZA = ObtenerMensajeZonaAustral(cuil);
            log.Debug("Mensaje zona austral:" + mensajeZA);
            ViewBag.MensajeZonaAustral = mensajeZA;
            ADPWrapperWS.ADPWrapperWS ws = new ADPWrapperWS.ADPWrapperWS();

            var response = ws.ObtenerPertenenciaZonaAustralPorCuil(cuil);
            
            if (response != null
                && response.Retorno != null
                )
            {
                if (response.Retorno.CodigoRetorno == 0 && UDAI==false)
                {
                    ViewBag.MensajeGenericoZonaAustral = "Tené en cuenta que si seleccionas esta opción dejarás de cobrar el pago adicional por Zona Austral";
                }else
                {
                    ViewBag.MensajeGenericoZonaAustral = string.Empty;
                }


            }

            // Retornamos la vista
            return View();
        }

        private bool EsMayorDe13Anios(DateTime fechaNacimiento)
        {
            DateTime fechaActual = DateTime.Today;
            int edad = fechaActual.Year - fechaNacimiento.Year;
            if (fechaNacimiento > fechaActual.AddYears(-edad))
            {
                edad--;
            }
            return edad >= 13;
        }

        // Helper para calcular edad en un int
        private int CalcularEdad(DateTime fechaNac)
        {
            DateTime hoy = DateTime.Today;
            int edad = hoy.Year - fechaNac.Year;
            if (fechaNac.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }

        // Método que llama al WS para ver si NO está en zona austral (retorna msg), o SÍ está (retorna string.Empty)
        private string ObtenerMensajeZonaAustral(string cuil)
        {
            ADPWrapperWS.ADPWrapperWS ws = new ADPWrapperWS.ADPWrapperWS();
            var response = ws.ObtenerPertenenciaZonaAustralPorCuil(cuil);

            if (response != null && response.Retorno != null)
            {
                // Verificamos que DatosZonaAustral y domicilio no sean nulos
               

                    if (response.Retorno.CodigoRetorno == 99)
                    {
                        return "ATENCION: si vivís en una Zona Austral, " +
                               "antes de seleccionar un nuevo medio de cobro " +
                               "acreditá tu domicilio en una oficina de ANSES para " +
                               "mantener el pago adicional por zona";
                    }else if (response.Retorno.CodigoRetorno != 99 &&
                         response.DatosZonaAustral.domicilio.c_comprobante_domicilio_1 == 0 &&
                         response.DatosZonaAustral.domicilio.c_comprobante_domicilio_2 == 0)
                    {
                        if (response.DatosZonaAustral != null && response.DatosZonaAustral.domicilio != null)
                        {
                        log.Error("Codigo Postal: " + response.DatosZonaAustral.domicilio.codigo_postal);
                        log.Error("Codigo Provincia: " + response.DatosZonaAustral.domicilio.c_provincia);
                        bool EsZonaAustral = ValidarCPYProvincia(
                            response.DatosZonaAustral.domicilio.codigo_postal,
                            response.DatosZonaAustral.domicilio.c_provincia);
                        log.Error("Es zona austral: " + EsZonaAustral);
                        if(EsZonaAustral)
                        {
                            return "ATENCION: si vivís en una Zona Austral, " +
                               "antes de seleccionar un nuevo medio de cobro " +
                               "acreditá tu domicilio en una oficina de ANSES para " +
                               "mantener el pago adicional por zona";
                        }

                        }

                }
                
            }
            // Si no se cumple la condición o falta información, retorna string vacío.
            return string.Empty;
        }

        public bool ValidarCPYProvincia(int codigoPostal, int codigoProvincia)
        {
            int[] cpsValidos = { 8504, 8505, 8506, 8508, 8142, 8512, 8148 };
            int[] provinciasValidas = { 17, 19, 21, 22, 23, 24 };

            return cpsValidos.Contains(codigoPostal) || provinciasValidas.Contains(codigoProvincia);
        }

        [HttpPost]
        public JsonResult ValidarCBU(string CBU)
        {
            string cuil = Session["CUIL"] as string;
            validarCBU.Retorno ret = VCBU.Validar(cuil, CBU);
            if(ret.CodigoRetorno!=0)
            {
                return Json(new { isValid = true });
            }else
            {
                return Json(new { isValid = false });
            }
            
        }

        [HttpGet]
        public JsonResult TraerBancosFisicos(string codigoPostal)
        {
            try
            {
                var bancosFisicos = GMPU.TraerBancosFisicos(short.Parse(codigoPostal));
                return Json(bancosFisicos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Error al obtener los bancos físicos", ex);
                return Json(new { error = "Error al obtener los bancos físicos." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult TraerBancosCorreo(string codigoPostal)
        {
            try
            {
                var bancosCorreo = GMPU.ListarBancosCorreo(short.Parse(codigoPostal));
                return Json(bancosCorreo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Error al obtener los bancos correo", ex);
                return Json(new { error = "Error al obtener los bancos correo." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult TraerBancosVirtuales()
        {
            try
            {
                var bancosVirtuales = GMPU.ListarBancosVirtuales();
                return Json(bancosVirtuales, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Error al obtener los bancos virtuales", ex);
                return Json(new { error = "Error al obtener los bancos virtuales." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GuardarMedioDePago(string tipoMedioPago, string valor, string codigoBanco = null, string codigoAgencia = null)
        {
            try
            {
                string cuil = Session["CUIL"] as string;
                if (string.IsNullOrEmpty(cuil))
                {
                    return Json(new { success = false, message = "CUIL no encontrado en la sesión." });
                }
                decimal? codUdai = 999999995;
                if (UDAI)
                {
                    codUdai = Convert.ToDecimal(Session["CODUDAI"] as string);
                    log.Debug($"CodUdai: {codUdai}");
                    log.Debug($"CodUdaiSession: {Session["CODUDAI"]}");
                }
                var medioPago = new DTOMedioPago
                {
                    Cuil = decimal.Parse(cuil),
                    TipoMedioPago = tipoMedioPago,
                    PeInicioPago = decimal.Parse(DateTime.Now.ToString("yyyyMM")),
                    IpOrigen = Session["IP_ORIGEN"] as string,
                    Udai = codUdai,
                    OpeTramite = Session["OPE_TRAMITE"] as string
                };

                switch (tipoMedioPago)
                {
                    case "CBU":
                        if (valor.Length != 22)
                        {
                        
                            return Json(new { success = false, message = "El CBU debe tener 22 dígitos." });

                        }

                        medioPago.CbuInicio = decimal.Parse(valor.Substring(0, 8));
                        medioPago.CbuFinal = decimal.Parse(valor.Substring(8));
                        break;
                    case "BilleteraVirtual":
                        if (string.IsNullOrEmpty(codigoBanco) || string.IsNullOrEmpty(codigoAgencia) )
                        {
                            return Json(new { success = false, message = "Código de billetera y agencia son requeridos." });

                        }
                        medioPago.CBanco = short.Parse(codigoBanco);
                        medioPago.CAgencia = short.Parse(codigoAgencia);

                        break;
                    case "Banco":
                        if (string.IsNullOrEmpty(codigoBanco) || string.IsNullOrEmpty(codigoAgencia))
                        {
                            return Json(new { success = false, message = "Código de banco y agencia son requeridos." });
                        }
                        medioPago.CBanco = short.Parse(codigoBanco);
                        medioPago.CAgencia = short.Parse(codigoAgencia);
                        break;
                    case "BancoDigital":
                        if (string.IsNullOrEmpty(codigoBanco) || string.IsNullOrEmpty(codigoAgencia))
                        {
                            return Json(new { success = false, message = "Código de banco y agencia son requeridos." });
                        }
                        medioPago.CBanco = short.Parse(codigoBanco);
                        medioPago.CAgencia = short.Parse(codigoAgencia);
                        break;
                    default:
                        return Json(new { success = false, message = "Tipo de medio de pago no válido." });
                }

                log.Debug("Propiedades del objeto medioPago:");
                log.Debug($"Cuil: {medioPago.Cuil}");
                log.Debug($"TipoMedioPago: {medioPago.TipoMedioPago}");
                log.Debug($"PeInicioPago: {medioPago.PeInicioPago}");
                log.Debug($"IpOrigen: {medioPago.IpOrigen}");
                log.Debug($"Udai: {medioPago.Udai}");
                log.Debug($"OpeTramite: {medioPago.OpeTramite}");
                log.Debug($"CbuInicio: {medioPago.CbuInicio}");
                log.Debug($"CbuFinal: {medioPago.CbuFinal}");
                log.Debug($"Cvu1: {medioPago.Cvu1}");
                log.Debug($"Cvu2: {medioPago.Cvu2}");
                log.Debug($"CBanco: {medioPago.CBanco}");
                log.Debug($"CAgencia: {medioPago.CAgencia}");

                int resultado = GMPU.GuardarMedioPagoUnico(medioPago);
                //int resultado = 1;
                log.Debug($"Resultado: {resultado}");

                if (resultado > 0)
                {
                    RetornoDatosPersonaCuip datosPersona = DPC.ObtenerPersonaxCUIP(cuil);
                    string nombreTitular = datosPersona.PersonaCuip.nombre;
                    string apellidoTitular = datosPersona.PersonaCuip.apellido;
                    string titularCompleto = (nombreTitular + " " + apellidoTitular).Trim();

                    // ********** BLOQUE ORIGINAL (comentado) **********
                    // string fechaPresentacion = DateTime.Now.ToString("dd/MM/yyyy");
                    // string estadoTramite = "Autorizado"; // Ajusta si tu lógica dice otra cosa
                    // string tipoTramite = "Alta";       // "Alta" o "Cambio"
                    // string bco = (tipoMedioPago == "BilleteraVirtual") ? "BILLETERA VIRTUAL"
                    //         : (tipoMedioPago == "BancoDigital") ? "BANCO DIGITAL"
                    //         : (tipoMedioPago == "Banco") ? "SUCURSAL BANCARIA"
                    //         : "CBU"; // fallback
                    // string age = (codigoAgencia != null) ? codigoAgencia : "N/A";
                    // string cbu = (tipoMedioPago == "CBU") ? valor : "N/A";
                    // string vigenciaDesde = DateTime.Now.ToString("MM/yyyy"); // Ejemplo
                    // ********** FIN BLOQUE ORIGINAL **********

                    // ********** MODIFICACION: Bloque nuevo para armar la info del comprobante y el mail **********
                    string fechaPresentacion = DateTime.Now.ToString("dd/MM/yyyy"); // Formato de fecha para el comprobante
                    string estadoTramite = "Autorizado"; // Estado fijo "Autorizado" (se puede ajustar según la lógica)
                    string tipoTramite = "Alta";         // Tipo de trámite definido ("Alta" o "Cambio")
                    string bco = string.Empty;           // Variable para el nombre del banco
                    string age = string.Empty;           // Variable para la información de la agencia
                    string cbu = (tipoMedioPago == "CBU") ? valor : "N/A"; // Si es CBU, se asigna el valor completo; de lo contrario "N/A"
                    string vigenciaDesde = DateTime.Now.ToString("MM/yyyy"); // Vigencia del medio de pago

                    if (tipoMedioPago == "Banco")
                    {
                        // Se obtiene el nombre real del banco y la información de la agencia (nombre, calle y número)
                        bco = GMPU.ObtenerNombreBanco(medioPago.CBanco.Value); // Uso de método de negocio para obtener nombre de banco
                        var agenciaInfo = GMPU.ObtenerNombreAgencia(medioPago.CBanco.Value, medioPago.CAgencia.Value); // Obtención de información de la agencia
                        age = agenciaInfo != null ? $"{agenciaInfo.NombreAgencia} - {agenciaInfo.Calle} {agenciaInfo.NumeroCalle}" : codigoAgencia; // Formateo de agencia y dirección
                    }
                    else if (tipoMedioPago == "CBU")
                    {
                        // MODIFICACION: Se extraen los primeros 3 dígitos y se usa el método de GMPU para obtener el nombre del banco
                        string bankCode = valor.Substring(0, 3); // Extraer los primeros 3 dígitos del CBU
                        bco = GMPU.ObtenerNombreBanco(short.Parse(bankCode)); // Uso del método de negocio en GMPU para obtener el nombre del banco
                        age = "CBU/CVU"; // Se establece "CBU/CVU" en agencia
                    }
                    else if (tipoMedioPago == "BilleteraVirtual")
                    {
                        bco = "BILLETERA VIRTUAL"; // Valor fijo para billetera virtual
                        age = (codigoAgencia != null) ? codigoAgencia : "N/A"; // Usa el código de agencia si existe
                    }
                    else if (tipoMedioPago == "BancoDigital")
                    {
                        bco = "BANCO DIGITAL"; // Valor fijo para banco digital
                        age = (codigoAgencia != null) ? codigoAgencia : "N/A"; // Usa el código de agencia si existe
                    }
                    else
                    {
                        bco = "N/A";
                        age = "N/A";
                    }
                    // ********** LOGEO DE TODA LA INFO NUEVA **********
                    log.Debug("---- Información para el comprobante y mail ----");
                    log.Debug("Fecha de Presentación: " + fechaPresentacion);
                    log.Debug("Estado del Trámite: " + estadoTramite);
                    log.Debug("Tipo de Trámite: " + tipoTramite);
                    log.Debug("Nombre del Banco (bco): " + bco);
                    log.Debug("Información de Agencia (age): " + age);
                    log.Debug("CBU: " + cbu);
                    log.Debug("Vigencia Desde: " + vigenciaDesde);
                    log.Debug("---- Fin de la información nueva ----");
                    // ********** FIN LOGEO **********
                    // ********** FIN BLOQUE MODIFICADO **********

                    Session["Titular"] = titularCompleto;
                    Session["CuilTitular"] = cuil;
                    Session["FechaPresentacion"] = fechaPresentacion;
                    Session["EstadoTramite"] = estadoTramite;
                    Session["TipoTramite"] = tipoTramite;
                    Session["Banco"] = bco;
                    Session["Agencia"] = age;
                    Session["CBU"] = cbu;
                    Session["VigenciaDesde"] = vigenciaDesde;

                    EnviarMail.EnviarMail wsMail = new EnviarMail.EnviarMail();
                    try
                    {
                        string mailHtml = GenerarHtmlComprobante(
                            titularCompleto,
                            cuil,
                            fechaPresentacion,
                            estadoTramite,
                            tipoTramite,
                            bco,
                            age,
                            cbu,
                            vigenciaDesde);
                        log.Debug("Email HTML: " + mailHtml);

                        string emailDestino = datosPersona.PersonaCuip.email;
                        log.Debug("Email DESTINO: " + emailDestino);

                        bool enviadoOk = wsMail.EnvioUnico(
                            emailDestino,
                            "Comprobante de Medio de Pago",
                            mailHtml,
                            ""
                        );

                        if (!enviadoOk)
                        {
                            log.Warn("No se pudo enviar el email de comprobante. El WS devolvió false.");
                        }
                        else
                        {
                            log.Debug("Email de comprobante enviado con éxito.");
                        }
                    }
                    catch (Exception exMail)
                    {
                        log.Error("Error al intentar enviar el email de comprobante", exMail);
                    }

                    return Json(new { success = true, message = "Medio de pago guardado exitosamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo guardar el medio de pago." });
                }
            }
            catch (Exception ex)
            {
                log.Error("Error al guardar el medio de pago", ex);
                return Json(new { success = false, message = "Ocurrió un error al guardar el medio de pago. Por favor, intente nuevamente." });
            }
        }

        /// <summary>
        /// Genera el HTML para la constancia de trámite, usando datos dinámicos.
        /// </summary>
        /// <summary>
        /// Genera el HTML con estilo ANSES, usando los mismos 9 parámetros del método original.
        /// </summary>
        private string GenerarHtmlComprobante(
            string titularCompleto,
            string cuil,
            string fechaPresentacion,
            string estadoTramite,
            string tipoTramite,
            string banco,
            string agencia,
            string cbu,
            string vigenciaDesde)
        {
            return $@"
<!DOCTYPE html>
<html lang='es'>
<head>
  <meta charset='UTF-8'>
  <title>CUNA - Constancia de trámite</title>
  <style>
    body {{
      margin: 0;
      padding: 0;
      font-family: Arial, sans-serif;
      color: #333;
      background-color: #f7f7f7;
    }}
    .header {{
      background-color: #00519a;
      color: #fff;
      padding: 15px 20px;
      text-align: center;
      font-size: 18px;
      font-weight: bold;
    }}
    .container {{
      background-color: #fff;
      margin: 20px auto;
      padding: 20px;
      max-width: 700px;
      box-shadow: 0 0 5px rgba(0,0,0,0.1);
    }}
    .subheader {{
      margin-top: 10px;
      font-size: 16px;
      font-weight: bold;
      text-transform: uppercase;
      text-align: center;
      color: #333;
    }}
    .datos-tramite {{
      margin-top: 20px;
    }}
    .datos-tramite table {{
      width: 100%;
      border-collapse: collapse;
      margin-bottom: 20px;
    }}
    .datos-tramite table td {{
      border: 1px solid #ccc;
      padding: 8px;
      vertical-align: middle;
      font-size: 14px;
    }}
    .section-title {{
      font-weight: bold;
      margin: 15px 0 5px 0;
      font-size: 15px;
    }}
    .footer {{
      margin-top: 40px;
      text-align: center;
      font-size: 12px;
      color: #777;
    }}
    hr {{
      margin: 30px 0;
      border: none;
      border-top: 1px solid #ccc;
    }}
  </style>
</head>
<body>

  <!-- Cabecera principal -->
  <div class='header'>
    Constancia de trámite
  </div>

  <!-- Contenedor principal -->
  <div class='container'>
    <!-- Subheader con nombre y CUIL -->
    <div class='subheader'>
      {titularCompleto} | {cuil}
    </div>

    <!-- Sección de datos del trámite -->
    <div class='datos-tramite'>
      <table>
        <tr>
          <td><strong>Fecha presentación:</strong></td>
          <td>{fechaPresentacion}</td>
        </tr>
        <tr>
          <td><strong>Estado del trámite:</strong></td>
          <td>{estadoTramite}</td>
        </tr>
        <tr>
          <td><strong>Tipo de trámite:</strong></td>
          <td>{tipoTramite}</td>
        </tr>
        <tr>
          <td><strong>Banco / Billetera:</strong></td>
          <td>{banco}</td>
        </tr>
        <tr>
          <td><strong>Agencia:</strong></td>
          <td>{agencia}</td>
        </tr>
        <tr>
          <td><strong>CBU:</strong></td>
          <td>{cbu}</td>
        </tr>
        <tr>
          <td><strong>Vigencia Desde:</strong></td>
          <td>{vigenciaDesde}</td>
        </tr>
      </table>

      <p>
        El solicitante <strong>{titularCompleto}</strong>, CUIL: <strong>{cuil}</strong>
        realizó un <strong>{tipoTramite}</strong> de MEDIO DE PAGO con vigencia desde <strong>{vigenciaDesde}</strong>.<br>
        Banco / Billetera: <strong>{banco}</strong> | Agencia: <strong>{agencia}</strong> | CBU: <strong>{cbu}</strong>
      </p>
    </div>

    <hr>

    

  </div>

</body>
</html>";
        }

        public ActionResult MensajeFinal()
        {
            // Recuperamos lo que guardamos en Session
            string titular = Session["Titular"] as string;
            string cuil = Session["CuilTitular"] as string;
            string fechaPres = Session["FechaPresentacion"] as string;
            string estado = Session["EstadoTramite"] as string;
            string tipo = Session["TipoTramite"] as string;
            string banco = Session["Banco"] as string;
            string agencia = Session["Agencia"] as string;
            string cbu = Session["CBU"] as string;
            string vigencia = Session["VigenciaDesde"] as string;

            // Cargamos en ViewBag o en un ViewModel
            ViewBag.Titular = titular;
            ViewBag.Cuil = cuil;
            ViewBag.Fecha = fechaPres;
            ViewBag.Estado = estado;
            ViewBag.Tramite = tipo;
            ViewBag.Banco = banco;
            ViewBag.Agencia = agencia;
            ViewBag.CBU = cbu;
            ViewBag.Vigencia = vigencia;

            return View();
        }


        [HttpGet]
        public JsonResult TraerBilleterasVirtuales()
        {
            try
            {
                var billeterasVirtuales = GMPU.ListarBilleterasVirtuales();
                return Json(billeterasVirtuales, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Error al obtener las billeteras virtuales", ex);
                return Json(new { error = "Error al obtener las billeteras virtuales." }, JsonRequestBehavior.AllowGet);
            }
        }




        public async Task<ActionResult> Index(string cuil)
       //public async Task<ActionResult> Index()
        {
            //string cuil = "20150003521";
            //Session["CUIL"] = cuil;
            ViewBag.UDAI = UDAI;
            try
            {
                //este bloque es solo para cuando se recibe el cuil por param
                if (UDAI)
                {
                    if (string.IsNullOrEmpty(cuil))
                    {
                        // Intentar recuperar de la cookie
                        cuil = Request.Cookies["CUIL"]?.Value;

                        if (string.IsNullOrEmpty(cuil))
                        {
                            log.Error("No se proporcionó CUIL y no se pudo recuperar de la cookie.");
                            return View("Error", new ErrorViewModel { Message = "CUIL no proporcionado." });
                        }
                        else
                        {
                            log.Info($"CUIL recuperado de la cookie: {cuil}");
                            Session["CUIL"] = cuil;
                        }
                    }
                    else
                    {
                        // Almacenar el CUIL en una cookie para futuras solicitudes
                        Response.Cookies["CUIL"].Value = cuil;
                        Response.Cookies["CUIL"].Expires = DateTime.Now.AddMinutes(30);
                    }
                }
                    
                //string cuil = "";
                if (2==2)
                {
                    this._token = Credencial.ObtenerCredencial();

                    log.Info("OBTUVE TOKEN");

                    //Se crea una variable de tipo DirectorMV para guardar la cookie "token"
                    director = new DirectorMV
                    {
                        token = GetProperty("strtoken", this._token),
                        sign = GetProperty("strsign", this._token),
                        userId = this._token.SSOToken.Operation.Login.UId
                    };
                    
                    log.Info("USUARIO --> " + director.userId);
                    if(!UDAI)
                    {
                        Cookie.GuardarCookie(Response, Request.RawUrl, "directorToken", director, DateTime.Now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["datosUsuarioExpireTimeMintutes"])));
                        cuil = director.userId;
                        log.Error($"cuil recuperado de mianses " + cuil);
                        Session["CUIL"] = cuil;
                    }
                    if (this._token == null || !this._token.credencialok)
                    {
                        
                        /*Response.Cookies["CODUDAI"].Value = Helper.getTokenCodDependencia(); 
                        Response.Cookies["CODUDAI"].Expires = DateTime.Now.AddMinutes(30);*/
                        
                        log.Debug("INDEX - Acceso denegado");

                        throw new Exception("Acceso denegado");

                    }
                    else
                    {

                        /*ViewBag.OperadorNombre = Helper.getTokenNombreUsuario();

                        ViewBag.OperadorLegajo = Helper.getUsrLegajo();

                        ViewBag.OperadorUdai = 

                        ViewBag.Udai = 

                        ViewBag.OperadorRol = Helper.getTokenGroups();*/
                        Response.Cookies["CODUDAI"].Value = Helper.getTokenCodDependencia();
                        Response.Cookies["CODUDAI"].Expires = DateTime.Now.AddMinutes(30);
                        Session["CODUDAI"] = Helper.getTokenCodDependencia();
                        Session["IP_ORIGEN"] = Helper.getTokenIp();
                        if(UDAI)
                        {
                            Session["OPE_TRAMITE"] = Helper.getUsrLegajo();
                        }
                        else
                        {
                            Session["OPE_TRAMITE"] = "MIANSES";
                        }
                        

                        log.Debug("UDAI: "+ Session["CODUDAI"]);
                        log.Debug("IP_ORIGEN: "+ Session["IP_ORIGEN"]);
                        log.Debug("OPE_TRAMITE: "+ Session["OPE_TRAMITE"]);
                    }


                }
               
                try
                {
                    ViewBag.ValidarPersona = ValidarPersona;
                    if (!string.IsNullOrEmpty(cuil))
                    {
                        var medioPagoVigente = GMPU.ObtenerMedioPagoVigente(cuil.ToString());
                        
                        if (medioPagoVigente != null)
                        {
                           /* log.Debug($"DiasNoDejaCargarMDP: {DiasNoDejaCargarMDP}");
                            DateTime fechaAlta = medioPagoVigente.FAlta;
                            // Calculamos la diferencia de meses con respecto a hoy
                            DateTime hoy = DateTime.Now;
                            int totalMeses = (hoy.Year - fechaAlta.Year) * 12 + (hoy.Month - fechaAlta.Month);

                            //if (5>= 4)
                          if (totalMeses >= 4)*/
                            log.Debug($"DiasNoDejaCargarMDP: {DiasNoDejaCargarMDP}");
                            DateTime fechaAlta = medioPagoVigente.FAlta;
                            log.Debug($"fechaAlta MDP: {fechaAlta}");
                            DateTime hoy = DateTime.Now;
                            int totalDias = (hoy - fechaAlta).Days;
                            log.Debug($"Total dias ddiferencia entre fecha alta y parametrizacion: {totalDias}");
                            if (totalDias >= DiasNoDejaCargarMDP)
                            {
                                ViewBag.NoModificar = false;
                            }else
                            {
                                ViewBag.NoModificar = true;
                            }
                            ViewBag.MedioPagoActual = ObtenerDescripcionMedioPago(medioPagoVigente);
                           if(UDAI)
                            {
                                //return RedirectToAction("GuardarMDP");
                            }
                        }
                        else
                        {
                            if(UDAI)
                            {
                                return RedirectToAction("GuardarMDP");
                                //return RedirectToAction("ListadoMDP");

                            }
                            else
                            {
                                if(ValidarPersona)
                                {
                                    return RedirectToAction("VerificarCelular");
                                }else
                                {
                                    return RedirectToAction("ListadoMDP");
                                }
                                
                            }

                        }
                    }
                     
                }
                catch (Exception ex)
                {
                    log.Error($"Error al obtener el medio de pago vigente para el CUIL {cuil}", ex);
                    ViewBag.MedioPagoActual = "Error al obtener el medio de pago vigente";
                }

            }
            catch(Exception ex)
            {
                Session["CUIL"] = cuil;
                log.Error($"2 Error al obtener las credenciales ", ex);
                log.Error($"cuil con excepcion primera "+cuil);
            }


            ViewBag.DiasNoDejaCargarMDP = DiasNoDejaCargarMDP;

            ViewBag.Title = TITULO;
            return View();
        }

        private string ObtenerDescripcionMedioPago(DTOMedioPagoVigente medioPago)
        {
            if (medioPago == null)
            {
                return "No se ha registrado un medio de pago";
            }

            if (!string.IsNullOrEmpty(medioPago.Cbu1) && !string.IsNullOrEmpty(medioPago.Cbu2))
            {
                return $"CBU: {medioPago.Cbu1}{medioPago.Cbu2}";
            }
            else if (!string.IsNullOrEmpty(medioPago.Cvu1) && !string.IsNullOrEmpty(medioPago.Cvu2))
            {
                return $"CVU: {medioPago.Cvu1}{medioPago.Cvu2}";
            }
            else if (!string.IsNullOrEmpty(medioPago.DBco) && !string.IsNullOrEmpty(medioPago.DAge))
            {
                return $"Banco/Agencia: {medioPago.DBco} - {medioPago.DAge}";
            }
            else if (!string.IsNullOrEmpty(medioPago.Alias))
            {
                return $"Alias: {medioPago.Alias}";
            }
            else
            {
                return "Medio de pago no especificado";
            }
        }




        
        public ActionResult VerificarCelular()
        {
            
            string cuil = Session["CUIL"] as string;

            if (string.IsNullOrEmpty(cuil))
            {
                //return RedirectToAction("Index");
                cuil = Request.Cookies["CUIL"]?.Value;
                log.Info($"CUIL recuperado de la cookie: {cuil}");
            }

            try
            {
                if(1==1)
                {
                    if (UDAI)
                    {
                        var MPUActual = Session["MedoDePagoActual"] as string;
                        if (MPUActual != null)
                        {
                            //return RedirectToAction("GuardarMDP");
                        }
                        else
                        {
                            //return RedirectToAction("ListadoMDP");
                        }

                    }
                    log.Info($"CUIL antes de enviar al RETORNARPERSONA: {cuil}");
                    RetornoDatosPersonaCuip datosPersona = DPC.ObtenerPersonaxCUIP(cuil);

                    if (TieneError(datosPersona))
                    {
                        ViewBag.Error = $"Error al obtener datos de la persona: {datosPersona.error.desc_mensaje}";
                        return View();
                    }

                    
                  

                        if (!TieneTelefono(datosPersona))
                    {
                        ViewBag.Error = "Actualice sus datos de contacto";
                        return View();
                    }
                        string tel="+"+ datosPersona.PersonaCuip.telediscado_pais.ToString() + datosPersona.PersonaCuip.telediscado.ToString() + datosPersona.PersonaCuip.telefono.ToString();

                    ViewBag.Telefono = tel;
                    Session["Sexo"] = datosPersona.PersonaCuip.sexo;
                    Session["Telefono"] = tel;
                    Session["DNI"] = datosPersona.PersonaCuip.doc_nro;
                    if(datosPersona.PersonaCuip.marca_cel.ToString()!="S")
                    {
                        log.Error($"No es celular, tiene marca celular = "+ datosPersona.PersonaCuip.marca_cel.ToString());
                        ViewBag.Error = "El usuario no tiene celular";
                    }
                }
                else
                {
                    //ViewBag.Telefono = 1141684887;
                    //Session["Telefono"] = "1141684887";
                    //Session["Sexo"] = "M";
                    //Session["DNI"] = "40640653";
                }

                ViewBag.Cuil = cuil;

                return View();
            }
            catch (Exception ex)
            {
                log.Error($"Error al obtener datos de la persona con CUIL {cuil}", ex);
                ViewBag.Error = "Ocurrió un error al obtener los datos. Por favor, intente nuevamente más tarde.";
                return View();
            }
        }

        private bool TieneError(RetornoDatosPersonaCuip datosPersona)
        {
            return datosPersona.error != null && datosPersona.error.cod_retorno != 0;
        }

        private bool TieneTelefono(RetornoDatosPersonaCuip datosPersona)
        {
            return datosPersona.PersonaCuip.telefono != 0;
        }



        [HttpPost]
        public ActionResult EnviarCodigo()
        {
            try
            {
                string cuil = Session["CUIL"] as string;
                //string telefono = Session["Telefono"] as string;
                /*if (string.IsNullOrEmpty(telefono))
                {
                    //RetornoDatosPersonaCuip datosPersona = DPC.ObtenerPersonaxCUIP(cuil);
                    //telefono = "+" + datosPersona.PersonaCuip.telediscado_pais.ToString() + datosPersona.PersonaCuip.telediscado.ToString() + datosPersona.PersonaCuip.telefono.ToString();
                }*/
                RetornoDatosPersonaCuip datosPersona = DPC.ObtenerPersonaxCUIP(cuil);
                string telefono = datosPersona.PersonaCuip.telediscado.ToString() + datosPersona.PersonaCuip.telefono.ToString();
                log.Debug("Verificar celular");
                log.Debug("Cuil:"+ cuil);
                log.Debug("Celular:"+ telefono);
                int idRegistroInt = VC.VerificarCelular(cuil,telefono,0);

                Session["IdRegistro"] = idRegistroInt.ToString();

                if (idRegistroInt > 0)
                {
                    return Json(new { success = true, idRegistro = idRegistroInt });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo enviar el código" });
                }
            }
            catch (Exception ex)
            {
                // Loguear el error
                return Json(new { success = false, message = "Error al procesar la solicitud" });
            }
        }

        public ActionResult IngresarCodigo()
        {
            string IdRegistro=Session["IdRegistro"] as string;
            ViewBag.IdRegistro = IdRegistro;
            return View();
        }

        [HttpPost]
        public ActionResult ValidarCodigoSMS(string IdRegistro, string codigoIngresado)
        {
            try
            {
                int Response = VC.ConfirmarCelularxId(Convert.ToInt32(IdRegistro),codigoIngresado);
                

                if (Response>0)
                {
                   

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Código inválido" });
                }
            }
            catch (Exception ex)
            {
                // Loguear el error
                return Json(new { success = false, message = "Error al procesar la solicitud" });
            }
        }
        public ActionResult VerificarDNI()
        {
            string dni = Session["DNI"] as string;
            //00478917569
            ViewBag.DNI = dni;
            return View();
        }

        [HttpPost]
        public ActionResult ValidarIDTramite(string DNI, long IdTramite)
        {
            try
            {
                
                string sexo=Session["Sexo"] as string;
                string dni = Session["DNI"] as string;

                bool esValido = ACWS.ValidarDNI(DNI, sexo, IdTramite); ;

                if (esValido)
                {
                    string MDPA=Session["MedoDePagoActual"] as string;
                    ViewBag.MedioPagoActual = MDPA;
                    
                    // Aquí podrías guardar el estado de la validación en la sesión o en la base de datos
                    Session["DNIValidado"] = true;
                    string Val = Session["DNIValidado"].ToString();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "El número de trámite no es válido para el DNI proporcionado." });
                }
            }
            catch (Exception ex)
            {
                // Loguear el error
                return Json(new { success = false, message = "Error al procesar la solicitud" });
            }
        }




        private async Task<PersonaViewModel> ObtenerYValidarPersona(string cuil)
        {
            // Datos hardcodeados para pruebas
            var persona = new PersonaViewModel
            {
                Cuil = cuil,
                Nombre = "Juan",
                Apellido = "Pérez",
                FechaNacimiento = new DateTime(1990, 1, 1),
                PaisResidencia = "Argentina",
                Domicilio = new DomicilioViewModel
                {
                    Calle = "Av. Corrientes",
                    Numero = "1234",
                    CodigoPostal = "1043",
                    Localidad = "Ciudad Autónoma de Buenos Aires",
                    Provincia = "Buenos Aires",
                    TipoZona = "Urbana"
                }
            };

            // Validaciones
            bool esMenorDe13 = (DateTime.Now.Year - persona.FechaNacimiento.Year) < 13;
            bool esResidenteExterior = persona.PaisResidencia != "Argentina";
            bool esDomicilioZonaAustralRural = persona.Domicilio.TipoZona == "Rural" ||
                                               persona.Domicilio.Provincia == "Tierra del Fuego" ||
                                               persona.Domicilio.Provincia == "Santa Cruz";

            // Determinar si puede ingresar a la versión MiAnses
            persona.PuedeIngresarMiAnses = !esMenorDe13 && !esResidenteExterior && !esDomicilioZonaAustralRural;

            return persona;
        }
        public ActionResult ListadoMDP()
        {
            string cuil = Session["CUIL"] as string;
            if (string.IsNullOrEmpty(cuil))
            {
                //return RedirectToAction("Index");
                cuil = Request.Cookies["CUIL"]?.Value;
            }

            try
            {
                var mediosPagoDisponibles = GMPU.ListarMPDisponibles(cuil);
                //log.Info($"Medios de pago disponibles obtenidos: {mediosPagoDisponibles.Count}");

                var mediosPagoViewModel = new List<MedioPagoViewModel>();

                foreach (var mp in mediosPagoDisponibles)
                {
                    log.Info($"Procesando medio de pago: ID={mp?.CBanco}-{mp?.CAgencia}");

                    var viewModel = new MedioPagoViewModel();

                    // PeEmision
                    viewModel.PeEmision = mp?.PeEmision.ToString() ?? "N/A";
                    log.Info($"PeEmision: {viewModel.PeEmision}");

                    // PeLiquidado
                    viewModel.PeLiquidado = mp?.PeLiquidado.ToString() ?? "N/A";
                    log.Info($"PeLiquidado: {viewModel.PeLiquidado}");

                    // NombreBanco
                    viewModel.NombreBanco = mp?.NombreBanco ?? "N/A";
                    log.Info($"NombreBanco: {viewModel.NombreBanco}");

                    // NombreAgencia
                    viewModel.NombreAgencia = mp?.NombreAgencia ?? "N/A";
                    log.Info($"NombreAgencia: {viewModel.NombreAgencia}");

                    // CBU
                    if (mp?.Cbu1.HasValue == true && mp?.Cbu2.HasValue == true)
                    {
                        viewModel.CBU = $"{mp.Cbu1.Value:D8}{mp.Cbu2.Value:D14}";
                    }
                    else
                    {
                        viewModel.CBU = "N/A";
                    }
                    log.Info($"CBU: {viewModel.CBU}");

                    // MPago
                    viewModel.MPago = mp?.MPago ?? "N/A";
                    log.Info($"MPago: {viewModel.MPago}");

                    // TipoMedioPago
                    viewModel.TipoMedioPago = DeterminarTipoMedioPago(mp);
                    log.Info($"TipoMedioPago: {viewModel.TipoMedioPago}");

                    // CodigoBanco
                    viewModel.CodigoBanco = mp?.CBanco.ToString() ?? "N/A";
                    log.Info($"CodigoBanco: {viewModel.CodigoBanco}");

                    // CodigoAgencia
                    viewModel.CodigoAgencia = mp?.CAgencia.ToString() ?? "N/A";
                    log.Info($"CodigoAgencia: {viewModel.CodigoAgencia}");

                    mediosPagoViewModel.Add(viewModel);
                    log.Info("ViewModel agregado a la lista");
                }

                //log.Info($"Total de MediosPagoViewModel creados: {mediosPagoViewModel.Count}");

                return View(mediosPagoViewModel);
            }
            catch (Exception ex)
            {
                log.Error($"Error al obtener los medios de pago disponibles para el CUIL {cuil}", ex);
                ViewBag.Error = "Ocurrió un error al obtener los medios de pago disponibles. Por favor, intente nuevamente más tarde.";
                return View(new List<MedioPagoViewModel>());
            }
        }

        private string DeterminarTipoMedioPago(DTOMedioPagoDisponible mp)
        {
            if (mp == null)
            {
                log.Warn("DTOMedioPagoDisponible es null");
                return "Desconocido";
            }

            if (mp.Cbu1.HasValue && mp.Cbu2.HasValue)
            {
                log.Info("Tipo de medio de pago determinado como CBU");
                return "CBU";
            }
            if (mp.CBanco != 0 && mp.CAgencia != 0)
            {
                log.Info("Tipo de medio de pago determinado como Banco");
                return "Banco";
            }
            if (!string.IsNullOrEmpty(mp.MPago))
            {
                log.Info($"Tipo de medio de pago determinado como {mp.MPago}");
                return mp.MPago;
            }

            log.Warn("No se pudo determinar el tipo de medio de pago");
            return "Otro";
        }









        private string ObtenerDescripcionMedioPago()
        {
            /*if (medioPago == null)
            {
                return "No se ha registrado un medio de pago";
            }

            switch (medioPago.CModoPago)
            {
                case 1: // Asumiendo que 1 es CBU
                    return $"CBU: {medioPago.Cbu1}{medioPago.Cbu2}";
                case 2: // Asumiendo que 2 es CVU
                    return $"CVU: {medioPago.Cvu1}{medioPago.Cvu2}";
                case 3: // Asumiendo que 3 es Banco/Agencia
                    return $"Banco/Agencia: {medioPago.DBco} - {medioPago.DAge}";
                case 4: // Asumiendo que 4 es Alias
                    return $"Alias: {medioPago.Alias}";
                default:
                    return $"Medio de pago: {medioPago.CModoPago}";
            }*/
            return "No se ha registrado un medio de pago";
        }

        public short? convertVariable(string variable)
        {
            if (String.IsNullOrEmpty(variable) || variable == "-1")
            {
                return null;
            }
            else
            {
                return Convert.ToInt16(variable);
            }
        }

        public decimal? convertVariableDecimal(string variable)
        {
            if (String.IsNullOrEmpty(variable))
            {
                return null;
            }
            else
            {
                return decimal.Parse(variable);
            }
        }

        public DateTime? convertVariableDateTime(string variable)
        {
            if (String.IsNullOrEmpty(variable))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(variable);
            }
        }

    }

    public class ErrorViewModel
    {
        public string Message { get; set; }
    }
}


