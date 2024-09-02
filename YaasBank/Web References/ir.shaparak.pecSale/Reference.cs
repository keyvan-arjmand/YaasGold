﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace YaasBank.ir.shaparak.pecSale {
    using System.Diagnostics;
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SaleServiceSoap", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClientPaymentRequestDataBase))]
    public partial class SaleService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SalePaymentRequestOperationCompleted;
        
        private System.Threading.SendOrPostCallback SalePaymentWithIdOperationCompleted;
        
        private System.Threading.SendOrPostCallback UDSalePaymentRequestOperationCompleted;
        
        private System.Threading.SendOrPostCallback SalePaymentWithDiscountOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SaleService() {
            this.Url = global::YaasBank.Properties.Settings.Default.YaasBank_ir_shaparak_pecSale_SaleService;
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
        public event SalePaymentRequestCompletedEventHandler SalePaymentRequestCompleted;
        
        /// <remarks/>
        public event SalePaymentWithIdCompletedEventHandler SalePaymentWithIdCompleted;
        
        /// <remarks/>
        public event UDSalePaymentRequestCompletedEventHandler UDSalePaymentRequestCompleted;
        
        /// <remarks/>
        public event SalePaymentWithDiscountCompletedEventHandler SalePaymentWithDiscountCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService/SalePaymentRequest", RequestNamespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", ResponseNamespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ClientSaleResponseData SalePaymentRequest(ClientSaleRequestData requestData) {
            object[] results = this.Invoke("SalePaymentRequest", new object[] {
                        requestData});
            return ((ClientSaleResponseData)(results[0]));
        }
        
        /// <remarks/>
        public void SalePaymentRequestAsync(ClientSaleRequestData requestData) {
            this.SalePaymentRequestAsync(requestData, null);
        }
        
        /// <remarks/>
        public void SalePaymentRequestAsync(ClientSaleRequestData requestData, object userState) {
            if ((this.SalePaymentRequestOperationCompleted == null)) {
                this.SalePaymentRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSalePaymentRequestOperationCompleted);
            }
            this.InvokeAsync("SalePaymentRequest", new object[] {
                        requestData}, this.SalePaymentRequestOperationCompleted, userState);
        }
        
        private void OnSalePaymentRequestOperationCompleted(object arg) {
            if ((this.SalePaymentRequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SalePaymentRequestCompleted(this, new SalePaymentRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService/SalePaymentWithId", RequestNamespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", ResponseNamespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ClientSaleResponseData SalePaymentWithId(ClientSaleRequestData requestData) {
            object[] results = this.Invoke("SalePaymentWithId", new object[] {
                        requestData});
            return ((ClientSaleResponseData)(results[0]));
        }
        
        /// <remarks/>
        public void SalePaymentWithIdAsync(ClientSaleRequestData requestData) {
            this.SalePaymentWithIdAsync(requestData, null);
        }
        
        /// <remarks/>
        public void SalePaymentWithIdAsync(ClientSaleRequestData requestData, object userState) {
            if ((this.SalePaymentWithIdOperationCompleted == null)) {
                this.SalePaymentWithIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSalePaymentWithIdOperationCompleted);
            }
            this.InvokeAsync("SalePaymentWithId", new object[] {
                        requestData}, this.SalePaymentWithIdOperationCompleted, userState);
        }
        
        private void OnSalePaymentWithIdOperationCompleted(object arg) {
            if ((this.SalePaymentWithIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SalePaymentWithIdCompleted(this, new SalePaymentWithIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService/UDSalePaymentRequest", RequestNamespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", ResponseNamespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ClientPaymentResponseDataBase UDSalePaymentRequest(ClientSaleRequestData requestData) {
            object[] results = this.Invoke("UDSalePaymentRequest", new object[] {
                        requestData});
            return ((ClientPaymentResponseDataBase)(results[0]));
        }
        
        /// <remarks/>
        public void UDSalePaymentRequestAsync(ClientSaleRequestData requestData) {
            this.UDSalePaymentRequestAsync(requestData, null);
        }
        
        /// <remarks/>
        public void UDSalePaymentRequestAsync(ClientSaleRequestData requestData, object userState) {
            if ((this.UDSalePaymentRequestOperationCompleted == null)) {
                this.UDSalePaymentRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUDSalePaymentRequestOperationCompleted);
            }
            this.InvokeAsync("UDSalePaymentRequest", new object[] {
                        requestData}, this.UDSalePaymentRequestOperationCompleted, userState);
        }
        
        private void OnUDSalePaymentRequestOperationCompleted(object arg) {
            if ((this.UDSalePaymentRequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UDSalePaymentRequestCompleted(this, new UDSalePaymentRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService/SalePaymentWithDiscount", RequestNamespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", ResponseNamespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ClientSaleResponseData SalePaymentWithDiscount(ClientSaleDiscountRequestData requestData) {
            object[] results = this.Invoke("SalePaymentWithDiscount", new object[] {
                        requestData});
            return ((ClientSaleResponseData)(results[0]));
        }
        
        /// <remarks/>
        public void SalePaymentWithDiscountAsync(ClientSaleDiscountRequestData requestData) {
            this.SalePaymentWithDiscountAsync(requestData, null);
        }
        
        /// <remarks/>
        public void SalePaymentWithDiscountAsync(ClientSaleDiscountRequestData requestData, object userState) {
            if ((this.SalePaymentWithDiscountOperationCompleted == null)) {
                this.SalePaymentWithDiscountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSalePaymentWithDiscountOperationCompleted);
            }
            this.InvokeAsync("SalePaymentWithDiscount", new object[] {
                        requestData}, this.SalePaymentWithDiscountOperationCompleted, userState);
        }
        
        private void OnSalePaymentWithDiscountOperationCompleted(object arg) {
            if ((this.SalePaymentWithDiscountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SalePaymentWithDiscountCompleted(this, new SalePaymentWithDiscountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClientSaleDiscountRequestData))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class ClientSaleRequestData : ClientPaymentRequestDataBase {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClientSaleRequestData))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClientSaleDiscountRequestData))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class ClientPaymentRequestDataBase {
        
        private string loginAccountField;
        
        private long amountField;
        
        private long orderIdField;
        
        private string callBackUrlField;
        
        private string additionalDataField;
        
        private string originatorField;
        
        /// <remarks/>
        public string LoginAccount {
            get {
                return this.loginAccountField;
            }
            set {
                this.loginAccountField = value;
            }
        }
        
        /// <remarks/>
        public long Amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        public long OrderId {
            get {
                return this.orderIdField;
            }
            set {
                this.orderIdField = value;
            }
        }
        
        /// <remarks/>
        public string CallBackUrl {
            get {
                return this.callBackUrlField;
            }
            set {
                this.callBackUrlField = value;
            }
        }
        
        /// <remarks/>
        public string AdditionalData {
            get {
                return this.additionalDataField;
            }
            set {
                this.additionalDataField = value;
            }
        }
        
        /// <remarks/>
        public string Originator {
            get {
                return this.originatorField;
            }
            set {
                this.originatorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClientSaleResponseData))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class ClientPaymentResponseDataBase {
        
        private long tokenField;
        
        private string messageField;
        
        private short statusField;
        
        /// <remarks/>
        public long Token {
            get {
                return this.tokenField;
            }
            set {
                this.tokenField = value;
            }
        }
        
        /// <remarks/>
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        public short Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class ClientSaleResponseData : ClientPaymentResponseDataBase {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class Product {
        
        private int pGroupIdField;
        
        private long amountField;
        
        /// <remarks/>
        public int PGroupId {
            get {
                return this.pGroupIdField;
            }
            set {
                this.pGroupIdField = value;
            }
        }
        
        /// <remarks/>
        public long Amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class ClientSaleDiscountRequestData : ClientSaleRequestData {
        
        private Product[] discountProductField;
        
        /// <remarks/>
        public Product[] DiscountProduct {
            get {
                return this.discountProductField;
            }
            set {
                this.discountProductField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void SalePaymentRequestCompletedEventHandler(object sender, SalePaymentRequestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SalePaymentRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SalePaymentRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ClientSaleResponseData Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ClientSaleResponseData)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void SalePaymentWithIdCompletedEventHandler(object sender, SalePaymentWithIdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SalePaymentWithIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SalePaymentWithIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ClientSaleResponseData Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ClientSaleResponseData)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void UDSalePaymentRequestCompletedEventHandler(object sender, UDSalePaymentRequestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UDSalePaymentRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UDSalePaymentRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ClientPaymentResponseDataBase Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ClientPaymentResponseDataBase)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void SalePaymentWithDiscountCompletedEventHandler(object sender, SalePaymentWithDiscountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SalePaymentWithDiscountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SalePaymentWithDiscountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ClientSaleResponseData Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ClientSaleResponseData)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591