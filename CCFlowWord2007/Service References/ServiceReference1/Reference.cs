﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.239
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CCFlowWord2007.ServiceReference1 {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.DocFlowSoap")]
    public interface DocFlowSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetSettingByKey", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetSettingByKey(string key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DoType", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string DoType(string type, string p1, string p2, string p3, string p4, string p5, string p6);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RunSQLReturnString", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string RunSQLReturnString(string sql);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RunSQL", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int RunSQL(string sql);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/RunSQLReturnTable", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable RunSQLReturnTable(string sql);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GenerOID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int GenerOID();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface DocFlowSoapChannel : CCFlowWord2007.ServiceReference1.DocFlowSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DocFlowSoapClient : System.ServiceModel.ClientBase<CCFlowWord2007.ServiceReference1.DocFlowSoap>, CCFlowWord2007.ServiceReference1.DocFlowSoap {
        
        public DocFlowSoapClient() {
        }
        
        public DocFlowSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DocFlowSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DocFlowSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DocFlowSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetSettingByKey(string key) {
            return base.Channel.GetSettingByKey(key);
        }
        
        public string DoType(string type, string p1, string p2, string p3, string p4, string p5, string p6) {
            return base.Channel.DoType(type, p1, p2, p3, p4, p5, p6);
        }
        
        public string RunSQLReturnString(string sql) {
            return base.Channel.RunSQLReturnString(sql);
        }
        
        public int RunSQL(string sql) {
            return base.Channel.RunSQL(sql);
        }
        
        public System.Data.DataTable RunSQLReturnTable(string sql) {
            return base.Channel.RunSQLReturnTable(sql);
        }
        
        public int GenerOID() {
            return base.Channel.GenerOID();
        }
    }
}