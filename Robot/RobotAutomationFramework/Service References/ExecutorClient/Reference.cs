// <copyright file=Reference company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:50</date>
// <summary></summary>

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RobotAutomationFramework.ExecutorClient {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ExecutorClient.IExecutor")]
    public interface IExecutor {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/FetchSignInData", ReplyAction="http://tempuri.org/IExecutor/FetchSignInDataResponse")]
        string FetchSignInData(int iGroupId, string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/FetchSignInData", ReplyAction="http://tempuri.org/IExecutor/FetchSignInDataResponse")]
        System.Threading.Tasks.Task<string> FetchSignInDataAsync(int iGroupId, string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/LoadAllDataAtLaunch", ReplyAction="http://tempuri.org/IExecutor/LoadAllDataAtLaunchResponse")]
        CommonLibrary.Entities.RootAutomationDataClass LoadAllDataAtLaunch(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/LoadAllDataAtLaunch", ReplyAction="http://tempuri.org/IExecutor/LoadAllDataAtLaunchResponse")]
        System.Threading.Tasks.Task<CommonLibrary.Entities.RootAutomationDataClass> LoadAllDataAtLaunchAsync(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/P_FetchMasterTypeData", ReplyAction="http://tempuri.org/IExecutor/P_FetchMasterTypeDataResponse")]
        Entities.ApplicationTypes[] P_FetchMasterTypeData(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/P_FetchMasterTypeData", ReplyAction="http://tempuri.org/IExecutor/P_FetchMasterTypeDataResponse")]
        System.Threading.Tasks.Task<Entities.ApplicationTypes[]> P_FetchMasterTypeDataAsync(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/P_FetchAutomationGroupTree", ReplyAction="http://tempuri.org/IExecutor/P_FetchAutomationGroupTreeResponse")]
        Entities.AutomationGroupTree P_FetchAutomationGroupTree(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/P_FetchAutomationGroupTree", ReplyAction="http://tempuri.org/IExecutor/P_FetchAutomationGroupTreeResponse")]
        System.Threading.Tasks.Task<Entities.AutomationGroupTree> P_FetchAutomationGroupTreeAsync(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/P_FetchAllApplicationDetails", ReplyAction="http://tempuri.org/IExecutor/P_FetchAllApplicationDetailsResponse")]
        System.Collections.Generic.Dictionary<int, Entities.EntityDetails> P_FetchAllApplicationDetails(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/P_FetchAllApplicationDetails", ReplyAction="http://tempuri.org/IExecutor/P_FetchAllApplicationDetailsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int, Entities.EntityDetails>> P_FetchAllApplicationDetailsAsync(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/P_FetchAllMapperProcessFlows", ReplyAction="http://tempuri.org/IExecutor/P_FetchAllMapperProcessFlowsResponse")]
        Entities.GroupProcesses P_FetchAllMapperProcessFlows(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/P_FetchAllMapperProcessFlows", ReplyAction="http://tempuri.org/IExecutor/P_FetchAllMapperProcessFlowsResponse")]
        System.Threading.Tasks.Task<Entities.GroupProcesses> P_FetchAllMapperProcessFlowsAsync(string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/FetchDiagramByName", ReplyAction="http://tempuri.org/IExecutor/FetchDiagramByNameResponse")]
        ProcessViewModel.Common.DiagramItem FetchDiagramByName(string diagramName, string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/FetchDiagramByName", ReplyAction="http://tempuri.org/IExecutor/FetchDiagramByNameResponse")]
        System.Threading.Tasks.Task<ProcessViewModel.Common.DiagramItem> FetchDiagramByNameAsync(string diagramName, string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/SaveSignInDetails", ReplyAction="http://tempuri.org/IExecutor/SaveSignInDetailsResponse")]
        int SaveSignInDetails(int iGroupId, string signInGroupUserData, string strCurrentUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExecutor/SaveSignInDetails", ReplyAction="http://tempuri.org/IExecutor/SaveSignInDetailsResponse")]
        System.Threading.Tasks.Task<int> SaveSignInDetailsAsync(int iGroupId, string signInGroupUserData, string strCurrentUser);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IExecutorChannel : RobotAutomationFramework.ExecutorClient.IExecutor, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ExecutorClient : System.ServiceModel.ClientBase<RobotAutomationFramework.ExecutorClient.IExecutor>, RobotAutomationFramework.ExecutorClient.IExecutor {
        
        public ExecutorClient() {
        }
        
        public ExecutorClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ExecutorClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExecutorClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExecutorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string FetchSignInData(int iGroupId, string strCurrentUser) {
            return base.Channel.FetchSignInData(iGroupId, strCurrentUser);
        }
        
        public System.Threading.Tasks.Task<string> FetchSignInDataAsync(int iGroupId, string strCurrentUser) {
            return base.Channel.FetchSignInDataAsync(iGroupId, strCurrentUser);
        }
        
        public CommonLibrary.Entities.RootAutomationDataClass LoadAllDataAtLaunch(string strCurrentUser) {
            return base.Channel.LoadAllDataAtLaunch(strCurrentUser);
        }
        
        public System.Threading.Tasks.Task<CommonLibrary.Entities.RootAutomationDataClass> LoadAllDataAtLaunchAsync(string strCurrentUser) {
            return base.Channel.LoadAllDataAtLaunchAsync(strCurrentUser);
        }
        
        public Entities.ApplicationTypes[] P_FetchMasterTypeData(string strCurrentUser) {
            return base.Channel.P_FetchMasterTypeData(strCurrentUser);
        }
        
        public System.Threading.Tasks.Task<Entities.ApplicationTypes[]> P_FetchMasterTypeDataAsync(string strCurrentUser) {
            return base.Channel.P_FetchMasterTypeDataAsync(strCurrentUser);
        }
        
        public Entities.AutomationGroupTree P_FetchAutomationGroupTree(string strCurrentUser) {
            return base.Channel.P_FetchAutomationGroupTree(strCurrentUser);
        }
        
        public System.Threading.Tasks.Task<Entities.AutomationGroupTree> P_FetchAutomationGroupTreeAsync(string strCurrentUser) {
            return base.Channel.P_FetchAutomationGroupTreeAsync(strCurrentUser);
        }
        
        public System.Collections.Generic.Dictionary<int, Entities.EntityDetails> P_FetchAllApplicationDetails(string strCurrentUser) {
            return base.Channel.P_FetchAllApplicationDetails(strCurrentUser);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int, Entities.EntityDetails>> P_FetchAllApplicationDetailsAsync(string strCurrentUser) {
            return base.Channel.P_FetchAllApplicationDetailsAsync(strCurrentUser);
        }
        
        public Entities.GroupProcesses P_FetchAllMapperProcessFlows(string strCurrentUser) {
            return base.Channel.P_FetchAllMapperProcessFlows(strCurrentUser);
        }
        
        public System.Threading.Tasks.Task<Entities.GroupProcesses> P_FetchAllMapperProcessFlowsAsync(string strCurrentUser) {
            return base.Channel.P_FetchAllMapperProcessFlowsAsync(strCurrentUser);
        }
        
        public ProcessViewModel.Common.DiagramItem FetchDiagramByName(string diagramName, string strCurrentUser) {
            return base.Channel.FetchDiagramByName(diagramName, strCurrentUser);
        }
        
        public System.Threading.Tasks.Task<ProcessViewModel.Common.DiagramItem> FetchDiagramByNameAsync(string diagramName, string strCurrentUser) {
            return base.Channel.FetchDiagramByNameAsync(diagramName, strCurrentUser);
        }
        
        public int SaveSignInDetails(int iGroupId, string signInGroupUserData, string strCurrentUser) {
            return base.Channel.SaveSignInDetails(iGroupId, signInGroupUserData, strCurrentUser);
        }
        
        public System.Threading.Tasks.Task<int> SaveSignInDetailsAsync(int iGroupId, string signInGroupUserData, string strCurrentUser) {
            return base.Channel.SaveSignInDetailsAsync(iGroupId, signInGroupUserData, strCurrentUser);
        }
    }
}
