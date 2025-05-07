using System;
using System.Web;
using Newtonsoft.Json;
using log4net;

namespace MedioUnicoDePago.Ayudante
{
    public class Cookie
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Cookie));

        /// <summary>
        /// Dado el nombre de una Cookie la función devuelve el valor
        /// </summary>
        /// <param name="pNombreCookie"></param>
        /// <returns>String con el valor de la Cookie</returns>
        public static object ObtenerCookie(HttpRequestBase pRequest, string pNombreCookie)
        {
            return pRequest.Cookies[pNombreCookie].Value.ToString();
        }

        /// <summary>
        /// Dado el nombre, el valor de una Cookie, y su DateTime de Expire, la función almacena el valor en la Cookie
        /// </summary>
        /// <param name="pResponse"></param>
        /// <param name="pRequest"></param>
        /// <param name="pNombreCookie"></param>
        /// <param name="pValorCookie"></param>
        /// <param name="pTiempoExpire"></param>
        public static void GuardarCookie(HttpResponseBase pResponse, string pRawUrl, string pNombreCookie, object pValorCookie, DateTime pTiempoExpire)
        {
            HttpCookie httpCookie = new HttpCookie(pNombreCookie, pRawUrl);
            httpCookie.Value = JsonConvert.SerializeObject(pValorCookie);
            pResponse.Cookies.Add(httpCookie);
            pResponse.Cookies[pNombreCookie].Expires = pTiempoExpire;
        }

        /// <summary>
        /// Dado el nombre de la Cookie se expira la misma
        /// </summary>
        /// <param name="pResponse"></param>
        /// <param name="pNombreCookie"></param>
        public static void ExpirarCookie(HttpResponseBase pResponse, string pNombreCookie)
        {
            pResponse.Cookies[pNombreCookie].Expires = DateTime.Now.AddDays(-10);
        }
    }
}