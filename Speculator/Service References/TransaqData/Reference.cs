﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Speculator.TransaqData {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TransaqData.ITransaqData")]
    public interface ITransaqData {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITransaqData/ConnectToTransaq", ReplyAction="http://tempuri.org/ITransaqData/ConnectToTransaqResponse")]
        void ConnectToTransaq();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITransaqData/ConnectToTransaq", ReplyAction="http://tempuri.org/ITransaqData/ConnectToTransaqResponse")]
        System.Threading.Tasks.Task ConnectToTransaqAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITransaqData/DisconnectFromTransaq", ReplyAction="http://tempuri.org/ITransaqData/DisconnectFromTransaqResponse")]
        void DisconnectFromTransaq();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITransaqData/DisconnectFromTransaq", ReplyAction="http://tempuri.org/ITransaqData/DisconnectFromTransaqResponse")]
        System.Threading.Tasks.Task DisconnectFromTransaqAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITransaqDataChannel : Speculator.TransaqData.ITransaqData, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TransaqDataClient : System.ServiceModel.ClientBase<Speculator.TransaqData.ITransaqData>, Speculator.TransaqData.ITransaqData {
        
        public TransaqDataClient() {
        }
        
        public TransaqDataClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TransaqDataClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransaqDataClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransaqDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void ConnectToTransaq() {
            base.Channel.ConnectToTransaq();
        }
        
        public System.Threading.Tasks.Task ConnectToTransaqAsync() {
            return base.Channel.ConnectToTransaqAsync();
        }
        
        public void DisconnectFromTransaq() {
            base.Channel.DisconnectFromTransaq();
        }
        
        public System.Threading.Tasks.Task DisconnectFromTransaqAsync() {
            return base.Channel.DisconnectFromTransaqAsync();
        }
    }
}
