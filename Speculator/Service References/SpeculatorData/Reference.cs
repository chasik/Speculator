﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Speculator.SpeculatorData {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SpeculatorData.ISpeculatorData")]
    public interface ISpeculatorData {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISpeculatorData/GetHistory", ReplyAction="http://tempuri.org/ISpeculatorData/GetHistoryResponse")]
        void GetHistory(string symbol);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISpeculatorData/GetHistory", ReplyAction="http://tempuri.org/ISpeculatorData/GetHistoryResponse")]
        System.Threading.Tasks.Task GetHistoryAsync(string symbol);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISpeculatorData/DataSources", ReplyAction="http://tempuri.org/ISpeculatorData/DataSourcesResponse")]
        SpeculatorModel.MainData.DataSource[] DataSources();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISpeculatorData/DataSources", ReplyAction="http://tempuri.org/ISpeculatorData/DataSourcesResponse")]
        System.Threading.Tasks.Task<SpeculatorModel.MainData.DataSource[]> DataSourcesAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISpeculatorDataChannel : Speculator.SpeculatorData.ISpeculatorData, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SpeculatorDataClient : System.ServiceModel.ClientBase<Speculator.SpeculatorData.ISpeculatorData>, Speculator.SpeculatorData.ISpeculatorData {
        
        public SpeculatorDataClient() {
        }
        
        public SpeculatorDataClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SpeculatorDataClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SpeculatorDataClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SpeculatorDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void GetHistory(string symbol) {
            base.Channel.GetHistory(symbol);
        }
        
        public System.Threading.Tasks.Task GetHistoryAsync(string symbol) {
            return base.Channel.GetHistoryAsync(symbol);
        }
        
        public SpeculatorModel.MainData.DataSource[] DataSources() {
            return base.Channel.DataSources();
        }
        
        public System.Threading.Tasks.Task<SpeculatorModel.MainData.DataSource[]> DataSourcesAsync() {
            return base.Channel.DataSourcesAsync();
        }
    }
}