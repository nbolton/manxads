﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.237.
// 
#pragma warning disable 1591

namespace Rensoft.ErrorReporting.ErrorReportingWebService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ReportingServiceSoap", Namespace="http://schemas.rensoft.net/ErrorReporting")]
    public partial class ReportingService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ReportOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ReportingService() {
            this.Url = global::Rensoft.ErrorReporting.Properties.Settings.Default.Rensoft_ErrorReporting_ErrorReportingWebService_ReportingService;
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
        public event ReportCompletedEventHandler ReportCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.rensoft.net/ErrorReporting/Report", RequestNamespace="http://schemas.rensoft.net/ErrorReporting", ResponseNamespace="http://schemas.rensoft.net/ErrorReporting", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Report(string clientCode, string userComments, string exceptionType, string exceptionDetails, string userName, string machineName) {
            this.Invoke("Report", new object[] {
                        clientCode,
                        userComments,
                        exceptionType,
                        exceptionDetails,
                        userName,
                        machineName});
        }
        
        /// <remarks/>
        public void ReportAsync(string clientCode, string userComments, string exceptionType, string exceptionDetails, string userName, string machineName) {
            this.ReportAsync(clientCode, userComments, exceptionType, exceptionDetails, userName, machineName, null);
        }
        
        /// <remarks/>
        public void ReportAsync(string clientCode, string userComments, string exceptionType, string exceptionDetails, string userName, string machineName, object userState) {
            if ((this.ReportOperationCompleted == null)) {
                this.ReportOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReportOperationCompleted);
            }
            this.InvokeAsync("Report", new object[] {
                        clientCode,
                        userComments,
                        exceptionType,
                        exceptionDetails,
                        userName,
                        machineName}, this.ReportOperationCompleted, userState);
        }
        
        private void OnReportOperationCompleted(object arg) {
            if ((this.ReportCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReportCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ReportCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591