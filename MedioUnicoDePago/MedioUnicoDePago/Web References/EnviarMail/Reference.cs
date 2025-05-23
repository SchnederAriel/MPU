﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace MedioUnicoDePago.EnviarMail {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="EnviarMailSoap", Namespace="http://anses.gov.ar/arquitectura/")]
    public partial class EnviarMail : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback EnvioPruebaMailOperationCompleted;
        
        private System.Threading.SendOrPostCallback EnvioUnicoOperationCompleted;
        
        private System.Threading.SendOrPostCallback EnvioConAdjuntoOperationCompleted;
        
        private System.Threading.SendOrPostCallback EnvioPruebaConAdjuntoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public EnviarMail() {
            this.Url = global::MedioUnicoDePago.Properties.Settings.Default.MedioUnicoDePago_EnviarMail_EnviarMail;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event EnvioPruebaMailCompletedEventHandler EnvioPruebaMailCompleted;
        
        /// <remarks/>
        public event EnvioUnicoCompletedEventHandler EnvioUnicoCompleted;
        
        /// <remarks/>
        public event EnvioConAdjuntoCompletedEventHandler EnvioConAdjuntoCompleted;
        
        /// <remarks/>
        public event EnvioPruebaConAdjuntoCompletedEventHandler EnvioPruebaConAdjuntoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://anses.gov.ar/arquitectura/EnvioPruebaMail", RequestNamespace="http://anses.gov.ar/arquitectura/", ResponseNamespace="http://anses.gov.ar/arquitectura/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool EnvioPruebaMail(string Asunto, string CorreoElectronico, string CuerpoComunicacion) {
            object[] results = this.Invoke("EnvioPruebaMail", new object[] {
                        Asunto,
                        CorreoElectronico,
                        CuerpoComunicacion});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void EnvioPruebaMailAsync(string Asunto, string CorreoElectronico, string CuerpoComunicacion) {
            this.EnvioPruebaMailAsync(Asunto, CorreoElectronico, CuerpoComunicacion, null);
        }
        
        /// <remarks/>
        public void EnvioPruebaMailAsync(string Asunto, string CorreoElectronico, string CuerpoComunicacion, object userState) {
            if ((this.EnvioPruebaMailOperationCompleted == null)) {
                this.EnvioPruebaMailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnvioPruebaMailOperationCompleted);
            }
            this.InvokeAsync("EnvioPruebaMail", new object[] {
                        Asunto,
                        CorreoElectronico,
                        CuerpoComunicacion}, this.EnvioPruebaMailOperationCompleted, userState);
        }
        
        private void OnEnvioPruebaMailOperationCompleted(object arg) {
            if ((this.EnvioPruebaMailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EnvioPruebaMailCompleted(this, new EnvioPruebaMailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://anses.gov.ar/arquitectura/EnvioUnico", RequestNamespace="http://anses.gov.ar/arquitectura/", ResponseNamespace="http://anses.gov.ar/arquitectura/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool EnvioUnico(string CorreoElectronico, string Asunto, string CuerpoHTML, string CuerpoTexto) {
            object[] results = this.Invoke("EnvioUnico", new object[] {
                        CorreoElectronico,
                        Asunto,
                        CuerpoHTML,
                        CuerpoTexto});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void EnvioUnicoAsync(string CorreoElectronico, string Asunto, string CuerpoHTML, string CuerpoTexto) {
            this.EnvioUnicoAsync(CorreoElectronico, Asunto, CuerpoHTML, CuerpoTexto, null);
        }
        
        /// <remarks/>
        public void EnvioUnicoAsync(string CorreoElectronico, string Asunto, string CuerpoHTML, string CuerpoTexto, object userState) {
            if ((this.EnvioUnicoOperationCompleted == null)) {
                this.EnvioUnicoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnvioUnicoOperationCompleted);
            }
            this.InvokeAsync("EnvioUnico", new object[] {
                        CorreoElectronico,
                        Asunto,
                        CuerpoHTML,
                        CuerpoTexto}, this.EnvioUnicoOperationCompleted, userState);
        }
        
        private void OnEnvioUnicoOperationCompleted(object arg) {
            if ((this.EnvioUnicoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EnvioUnicoCompleted(this, new EnvioUnicoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://anses.gov.ar/arquitectura/EnvioConAdjunto", RequestNamespace="http://anses.gov.ar/arquitectura/", ResponseNamespace="http://anses.gov.ar/arquitectura/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool EnvioConAdjunto(string CorreoElectronico, string Asunto, string CuerpoHTML, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] DatosBinarios, string NombreArchivo) {
            object[] results = this.Invoke("EnvioConAdjunto", new object[] {
                        CorreoElectronico,
                        Asunto,
                        CuerpoHTML,
                        DatosBinarios,
                        NombreArchivo});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void EnvioConAdjuntoAsync(string CorreoElectronico, string Asunto, string CuerpoHTML, byte[] DatosBinarios, string NombreArchivo) {
            this.EnvioConAdjuntoAsync(CorreoElectronico, Asunto, CuerpoHTML, DatosBinarios, NombreArchivo, null);
        }
        
        /// <remarks/>
        public void EnvioConAdjuntoAsync(string CorreoElectronico, string Asunto, string CuerpoHTML, byte[] DatosBinarios, string NombreArchivo, object userState) {
            if ((this.EnvioConAdjuntoOperationCompleted == null)) {
                this.EnvioConAdjuntoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnvioConAdjuntoOperationCompleted);
            }
            this.InvokeAsync("EnvioConAdjunto", new object[] {
                        CorreoElectronico,
                        Asunto,
                        CuerpoHTML,
                        DatosBinarios,
                        NombreArchivo}, this.EnvioConAdjuntoOperationCompleted, userState);
        }
        
        private void OnEnvioConAdjuntoOperationCompleted(object arg) {
            if ((this.EnvioConAdjuntoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EnvioConAdjuntoCompleted(this, new EnvioConAdjuntoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://anses.gov.ar/arquitectura/EnvioPruebaConAdjunto", RequestNamespace="http://anses.gov.ar/arquitectura/", ResponseNamespace="http://anses.gov.ar/arquitectura/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool EnvioPruebaConAdjunto(string Asunto, string CorreoElectronico, string CuerpoComunicacion, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] DatosBinarios, string NombreArchivo) {
            object[] results = this.Invoke("EnvioPruebaConAdjunto", new object[] {
                        Asunto,
                        CorreoElectronico,
                        CuerpoComunicacion,
                        DatosBinarios,
                        NombreArchivo});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void EnvioPruebaConAdjuntoAsync(string Asunto, string CorreoElectronico, string CuerpoComunicacion, byte[] DatosBinarios, string NombreArchivo) {
            this.EnvioPruebaConAdjuntoAsync(Asunto, CorreoElectronico, CuerpoComunicacion, DatosBinarios, NombreArchivo, null);
        }
        
        /// <remarks/>
        public void EnvioPruebaConAdjuntoAsync(string Asunto, string CorreoElectronico, string CuerpoComunicacion, byte[] DatosBinarios, string NombreArchivo, object userState) {
            if ((this.EnvioPruebaConAdjuntoOperationCompleted == null)) {
                this.EnvioPruebaConAdjuntoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnvioPruebaConAdjuntoOperationCompleted);
            }
            this.InvokeAsync("EnvioPruebaConAdjunto", new object[] {
                        Asunto,
                        CorreoElectronico,
                        CuerpoComunicacion,
                        DatosBinarios,
                        NombreArchivo}, this.EnvioPruebaConAdjuntoOperationCompleted, userState);
        }
        
        private void OnEnvioPruebaConAdjuntoOperationCompleted(object arg) {
            if ((this.EnvioPruebaConAdjuntoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EnvioPruebaConAdjuntoCompleted(this, new EnvioPruebaConAdjuntoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void EnvioPruebaMailCompletedEventHandler(object sender, EnvioPruebaMailCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EnvioPruebaMailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EnvioPruebaMailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void EnvioUnicoCompletedEventHandler(object sender, EnvioUnicoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EnvioUnicoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EnvioUnicoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void EnvioConAdjuntoCompletedEventHandler(object sender, EnvioConAdjuntoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EnvioConAdjuntoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EnvioConAdjuntoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void EnvioPruebaConAdjuntoCompletedEventHandler(object sender, EnvioPruebaConAdjuntoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EnvioPruebaConAdjuntoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EnvioPruebaConAdjuntoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591