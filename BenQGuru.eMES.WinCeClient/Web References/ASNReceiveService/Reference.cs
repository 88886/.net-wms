﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.5485
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.CompactFramework.Design.Data 2.0.50727.5485 版自动生成。
// 
namespace BenQGuru.eMES.WinCeClient.ASNReceiveService {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    using System.Data;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ASNReceiveServiceSoap", Namespace="http://tempuri.org/")]
    public partial class ASNReceiveService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        /// <remarks/>
        public ASNReceiveService() {
            this.Url = "http://112.74.38.84/mestest/BenQGuru.eMES.Web.WebService/ASNReceiveService.asmx";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetAsnStatus", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public AsnSimple GetAsnStatus(string stno) {
            object[] results = this.Invoke("GetAsnStatus", new object[] {
                        stno});
            return ((AsnSimple)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetAsnStatus(string stno, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetAsnStatus", new object[] {
                        stno}, callback, asyncState);
        }
        
        /// <remarks/>
        public AsnSimple EndGetAsnStatus(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((AsnSimple)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetDataGrid", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataTable GetDataGrid(string ASNNo, bool istrail) {
            object[] results = this.Invoke("GetDataGrid", new object[] {
                        ASNNo,
                        istrail});
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetDataGrid(string ASNNo, bool istrail, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetDataGrid", new object[] {
                        ASNNo,
                        istrail}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Data.DataTable EndGetDataGrid(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetDataGridDoc", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataTable GetDataGridDoc(string ASNNo) {
            object[] results = this.Invoke("GetDataGridDoc", new object[] {
                        ASNNo});
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetDataGridDoc(string ASNNo, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetDataGridDoc", new object[] {
                        ASNNo}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Data.DataTable EndGetDataGridDoc(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataTable)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/QueryASNNO", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] QueryASNNO() {
            object[] results = this.Invoke("QueryASNNO", new object[0]);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginQueryASNNO(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("QueryASNNO", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string[] EndQueryASNNO(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/QueryResult", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ComBoxValue[] QueryResult(string resultType) {
            object[] results = this.Invoke("QueryResult", new object[] {
                        resultType});
            return ((ComBoxValue[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginQueryResult(string resultType, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("QueryResult", new object[] {
                        resultType}, callback, asyncState);
        }
        
        /// <remarks/>
        public ComBoxValue[] EndQueryResult(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ComBoxValue[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetEmergency", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetEmergency(string stno) {
            object[] results = this.Invoke("GetEmergency", new object[] {
                        stno});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetEmergency(string stno, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetEmergency", new object[] {
                        stno}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetEmergency(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CheckASNReceiveStatus", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool CheckASNReceiveStatus(string stno) {
            object[] results = this.Invoke("CheckASNReceiveStatus", new object[] {
                        stno});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginCheckASNReceiveStatus(string stno, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("CheckASNReceiveStatus", new object[] {
                        stno}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndCheckASNReceiveStatus(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/QueryASNDetailSN", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public StNoLine[] QueryASNDetailSN(string sn, string stno) {
            object[] results = this.Invoke("QueryASNDetailSN", new object[] {
                        sn,
                        stno});
            return ((StNoLine[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginQueryASNDetailSN(string sn, string stno, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("QueryASNDetailSN", new object[] {
                        sn,
                        stno}, callback, asyncState);
        }
        
        /// <remarks/>
        public StNoLine[] EndQueryASNDetailSN(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((StNoLine[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/QuerySN", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool QuerySN(string stno, string stline, string sn) {
            object[] results = this.Invoke("QuerySN", new object[] {
                        stno,
                        stline,
                        sn});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginQuerySN(string stno, string stline, string sn, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("QuerySN", new object[] {
                        stno,
                        stline,
                        sn}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndQuerySN(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/BindCarton2STLine", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool BindCarton2STLine(string Stline, string Stno, string cartonno, out string message) {
            object[] results = this.Invoke("BindCarton2STLine", new object[] {
                        Stline,
                        Stno,
                        cartonno});
            message = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginBindCarton2STLine(string Stline, string Stno, string cartonno, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("BindCarton2STLine", new object[] {
                        Stline,
                        Stno,
                        cartonno}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndBindCarton2STLine(System.IAsyncResult asyncResult, out string message) {
            object[] results = this.EndInvoke(asyncResult);
            message = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CancelCartonno", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CancelCartonno(string stno, string[] stlines) {
            object[] results = this.Invoke("CancelCartonno", new object[] {
                        stno,
                        stlines});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginCancelCartonno(string stno, string[] stlines, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("CancelCartonno", new object[] {
                        stno,
                        stlines}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndCancelCartonno(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UploadFile", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool UploadFile([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] bytes, string asn, string type, string userName) {
            object[] results = this.Invoke("UploadFile", new object[] {
                        bytes,
                        asn,
                        type,
                        userName});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginUploadFile(byte[] bytes, string asn, string type, string userName, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("UploadFile", new object[] {
                        bytes,
                        asn,
                        type,
                        userName}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndUploadFile(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UpdateASN", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void UpdateASN(string stno, int rejectQty, string rejectResult, System.Data.DataTable dt) {
            this.Invoke("UpdateASN", new object[] {
                        stno,
                        rejectQty,
                        rejectResult,
                        dt});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginUpdateASN(string stno, int rejectQty, string rejectResult, System.Data.DataTable dt, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("UpdateASN", new object[] {
                        stno,
                        rejectQty,
                        rejectResult,
                        dt}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndUpdateASN(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ReceiveDetail", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ReceiveDetail(System.Data.DataTable dt, string stno, string rejectResult, string usr) {
            object[] results = this.Invoke("ReceiveDetail", new object[] {
                        dt,
                        stno,
                        rejectResult,
                        usr});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginReceiveDetail(System.Data.DataTable dt, string stno, string rejectResult, string usr, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ReceiveDetail", new object[] {
                        dt,
                        stno,
                        rejectResult,
                        usr}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndReceiveDetail(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GiveinDetail", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GiveinDetail(System.Data.DataTable dt, string stno, string giveinResult) {
            object[] results = this.Invoke("GiveinDetail", new object[] {
                        dt,
                        stno,
                        giveinResult});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGiveinDetail(System.Data.DataTable dt, string stno, string giveinResult, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GiveinDetail", new object[] {
                        dt,
                        stno,
                        giveinResult}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGiveinDetail(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RejectDetail", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string RejectDetail(System.Data.DataTable dt, string stno, string rejectResult, string rejectCount) {
            object[] results = this.Invoke("RejectDetail", new object[] {
                        dt,
                        stno,
                        rejectResult,
                        rejectCount});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRejectDetail(System.Data.DataTable dt, string stno, string rejectResult, string rejectCount, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RejectDetail", new object[] {
                        dt,
                        stno,
                        rejectResult,
                        rejectCount}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndRejectDetail(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetASN", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int[] GetASN(string stno) {
            object[] results = this.Invoke("GetASN", new object[] {
                        stno});
            return ((int[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetASN(string stno, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetASN", new object[] {
                        stno}, callback, asyncState);
        }
        
        /// <remarks/>
        public int[] EndGetASN(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((int[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ASNReject", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool ASNReject(string stno) {
            object[] results = this.Invoke("ASNReject", new object[] {
                        stno});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginASNReject(string stno, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ASNReject", new object[] {
                        stno}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndASNReject(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/DeleteDoc", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void DeleteDoc(string[] fileNames) {
            this.Invoke("DeleteDoc", new object[] {
                        fileNames});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginDeleteDoc(string[] fileNames, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("DeleteDoc", new object[] {
                        fileNames}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndDeleteDoc(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
    }
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class AsnSimple {
        
        private int rejectCountField;
        
        private string rejectReasonField;
        
        private string giveReasonField;
        
        /// <remarks/>
        public int RejectCount {
            get {
                return this.rejectCountField;
            }
            set {
                this.rejectCountField = value;
            }
        }
        
        /// <remarks/>
        public string RejectReason {
            get {
                return this.rejectReasonField;
            }
            set {
                this.rejectReasonField = value;
            }
        }
        
        /// <remarks/>
        public string GiveReason {
            get {
                return this.giveReasonField;
            }
            set {
                this.giveReasonField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class StNoLine {
        
        private string stNoField;
        
        private string stLineField;
        
        /// <remarks/>
        public string StNo {
            get {
                return this.stNoField;
            }
            set {
                this.stNoField = value;
            }
        }
        
        /// <remarks/>
        public string StLine {
            get {
                return this.stLineField;
            }
            set {
                this.stLineField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class ComBoxValue {
        
        private string textField;
        
        private string valueField;
        
        /// <remarks/>
        public string Text {
            get {
                return this.textField;
            }
            set {
                this.textField = value;
            }
        }
        
        /// <remarks/>
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
}
