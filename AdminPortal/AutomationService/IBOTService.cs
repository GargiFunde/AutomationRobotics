using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace AutomationService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    //[ServiceContract(SessionMode = SessionMode.Required)]
    public interface IBOTService
    {

        #region Asset Management
        [OperationContract]
        string getKey();
        //[OperationContract]
        //int createKey(string key, string assetName, int groupid, int tenantId);
        [OperationContract]
        int DeleteCredential(int id, int groupid, int tenantid, string CredentialName, string user);

        [OperationContract]
        int DeleteAsset(int id, int groupid, int tenantid, string AssetName, string user);
        [OperationContract]
        DataTable getCredentials(int groupid, int tenantid);
        [OperationContract]
        DataTable getAssets(int groupid, int tenantid);
        [OperationContract]
        int AddCredentials(string strAssetName, string strUserName, string Encrypted_Pwd, int tenantid, int groupid, string strcreatedBy);
        [OperationContract]
        int AddAssets(string strAssetName,string strValue,int tenantid,int groupid,string strcreatedBy);
        #endregion

        [OperationContract]
        int getRoleId(int TenantId, string rolename, int groupid);
        [OperationContract]
        DataTable getRoleType(string groupname, int groupid, int tenantid);
        [OperationContract]

        DataTable getGrounNames(int tenantid);
        [OperationContract]
        string GetQueueItem(string iGroupName, string iTenantName, string iQueueName);
        [OperationContract]
        int SaveRoleAccess(string roleName, int roleid, int tenantid, string role, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bQueueManagementR, bool bQueueManagementC, bool bQueueManagementE, bool bQueueManagementD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bProcessManagementR, bool bProcessManagementC, bool bProcessManagementE, bool bProcessManagementD, bool bDetailLogR, bool bDetailLogC, bool bDetailLogE, bool bDetailLogD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD);
        [OperationContract]
        DataTable getRoledata(int tenantid);
        [OperationContract]
        DataTable GetRoleAccess(int roleid, int groupid, int tenantid, string roleName);
        [OperationContract]
        int AddRole(string roleName, int groupid, int tenantid, string createdby, string groupname);

        #region GetCredentialsActivity
        [OperationContract]
        DataTable GetCredentialsActivity(string credentialName, int TenantId, int groupid);
        #endregion
        #region GetAssetsActivity
        [OperationContract]
        DataTable GetAssetsActivity(string assetname, int TenantId, int groupid);
        #endregion

        [OperationContract]
        DataTable getMicrosoftOCR();
        
        [OperationContract]
        int DeleteProcessSchedule(string strId, string strprocessename, string strBotName, string strChronExp, int groupid, int tenantid, string createdby);
        [OperationContract]
        DataTable GetProcessSchedules(int groupid, int tenantid);
        [OperationContract]
        int updateBotNameTouserBotMapping(int groupid, int tenantid, string botname, string username);
        [OperationContract]
        int updateQueueToBotMapping(int groupid, int tenantid, string botname, string QueueName);

        [OperationContract]
        int ChangeGroup(string currentRoleCG, string userName, string newRole, int tenantid);

        [OperationContract]
        int UpdateIsactiveStatusTenantRelatedTables(int tenantid, int isactive);

        [OperationContract]
        int CheckIsactiveStatusTenant(int tenantid);

        [OperationContract]
        int DeleteBot(string botname, int groupid, int tenantid, string createdby);

        [OperationContract]
        int CheckScheduleStatus(String botname, int tenantid);

        [OperationContract]
        DataTable GetCountScheduleRelatedBot(int tenantid, string botName);

        [OperationContract]
        int insertLog(string Message,string detailLog ,int groupid, int tenantid);

        #region Charts
        /*Charts Contracts*/
        [OperationContract]
        DataTable getMonthlyChartData(string status);

        [OperationContract]
        DataTable GetData_createschedulestatus();

        [OperationContract]
        DataTable GetddlData_createschedulestatus();

        [OperationContract]
        DataTable getChartData(string status);

        [OperationContract]
        DataTable getDoughnutChartData();
        /*Charts Contracts End*/
        #endregion

        [OperationContract]
        int insertActivityLog(string domainName, string userName, string groupName, string action, int groupid, int tenantid);


        [OperationContract]
        int PiyushLogs(string Message);

        [OperationContract]
        DataTable GetRQDetailsForBotDashboard(int groupid, int tenantid);

        [OperationContract]
        int DeleteTenantWithAllRelatedData(int groupid, int tenantid, string createdby);

        [OperationContract]
        DataTable GetCountToDeleteTenant(int tenantid);

        [OperationContract]
        DataTable getActivityLog(int groupid, int tenantid);

        [OperationContract]
        DataTable getCompleteLogs(int groupid, int tenantid);

        [OperationContract]
        int[] GetDetailsDashboard(int groupid, int tenantid);

        [OperationContract]
        int UpdateUserPassword(string userid, string OldPassword, string NewPassword, int groupid, int tenantid);

        [OperationContract]
        int CreateScheduleStatus(string QueueName, string BotName, string ChronExpression, string Status, int GroupId, int TenantId, string StartTime, string EndTime);

        [OperationContract]
        DataTable GetScheduleStatus(int groupid, int tenantid,string status);

        [OperationContract]
        int AddSchedule(string strQueueName, string strBotName, string strChronExp,string stopAfter, int groupid, int tenantid, string createdby);

        [OperationContract]
        int AddScheduleForProcess(string strProcessName, string strBotName, string strChronExp, string StopAfter, int groupid, int tenantid, string createdby);

        [OperationContract]
        int AddQueueDetails(string queueName, int groupid, int iTenantID, string createdby);

        [OperationContract]
        int DeleteSchedule(string strId, string strQueueName, string strBotName, string strChronExp, int groupid, int tenantid, string createdby);

        [OperationContract]
        DataTable GetSchedules(int groupid, int tenantid);

        #region Role Based Access
        /*Role Based Access*/
        [OperationContract]
        DataTable GetRoleBaseAccess(int groupid, int tenantid);

        [OperationContract]
        DataTable GetUserBaseAccess(int groupid, int tenantid);

        [OperationContract]
        //int SaveRoleBaseAccess(int groupid, int tenantid, string role, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bAddQueueR, bool bAddQueueC, bool bAddQueueE, bool bAddQueueD, bool bAddGroupR, bool bAddGroupC, bool bAddGroupE, bool bAddGroupD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bAddUpdateProcessR, bool bAddUpdateProcessC, bool bAddUpdateProcessE, bool bAddUpdateProcessD, bool bUploadProcessR, bool bUploadProcessC, bool bUploadProcessE, bool bUploadProcessD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD);
        int SaveRoleBaseAccess(int groupid, int tenantid, string role, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bQueueManagementR, bool bQueueManagementC, bool bQueueManagementE, bool bQueueManagementD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bProcessManagementR, bool bProcessManagementC, bool bProcessManagementE, bool bProcessManagementD, bool bDetailLogR, bool bDetailLogC, bool bDetailLogE, bool bDetailLogD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD);
        [OperationContract]
        DataTable GetPageAccess(int roleid, int groupid, int tenantid, string pagename);
        [OperationContract]
        DataTable GetPageAccessUser(int groupid, int tenantid, string username);
        [OperationContract]
        //int SaveRoleBaseAccessUser(int Igroupid, string usernameI, int Itenantid, bool bbBotDashboardR, bool bbBotDashboardC, bool bbBotDashboardE, bool bbBotDashboardD, bool bbqueueR, bool bbqueueC, bool bbqueueE, bool bbqueueD, bool bbAddScheduleR, bool bbAddScheduleC, bool bbAddScheduleE, bool bbAddScheduleD, bool bbAddUserR, bool bbAddUserC, bool bbAddUserE, bool bbAddUserD, bool bbAddRobotR, bool bbAddRobotC, bool bbAddRobotE, bool bbAddRobotD, bool bbAddQueueR, bool bbAddQueueC, bool bbAddQueueE, bool bbAddQueueD, bool bbAddGroupR, bool bbAddGroupC, bool bbAddGroupE, bool bbAddGroupD, bool bbAssignQueueBotR, bool bbAssignQueueBotC, bool bbAssignQueueBotE, bool bbAssignQueueBotD, bool bbAssignBotUserR, bool bbAssignBotUserC, bool bbAssignBotUserE, bool bbAssignBotUserD, bool bbBotLogR, bool bbBotLogC, bool bbBotLogE, bool bbBotLogD, bool bbAuditTrailR, bool bbAuditTrailC, bool bbAuditTrailE, bool bbAuditTrailD, bool bbScheduleDetailsR, bool bbScheduleDetailsC, bool bbScheduleDetailsE, bool bbScheduleDetailsD, bool bbConfigurationR, bool bbConfigurationC, bool bbConfigurationE, bool bbConfigurationD, bool bbAddUpdateProcessR, bool bbAddUpdateProcessC, bool bbAddUpdateProcessE, bool bbAddUpdateProcessD, bool bbUploadProcessR, bool bbUploadProcessC, bool bbUploadProcessE, bool bbUploadProcessD, bool bbPromoteDemoteR, bool bbPromoteDemoteC, bool bbPromoteDemoteE, bool bbPromoteDemoteD);
        int SaveRoleBaseAccessUser(int IGroupID, string usernameI, int ITenantID, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bQueueManagementR, bool bQueueManagementC, bool bQueueManagementE, bool bQueueManagementD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bProcessManagementR, bool bProcessManagementC, bool bProcessManagementE, bool bProcessManagementD, bool bDetailLogR, bool bDetailLogC, bool bDetailLogE, bool bDetailLogD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD);
            /*End of Role Based Access*/
        #endregion

        [OperationContract]
        bool AutomationRequest(RequestInput _requestInput);

        [OperationContract]
        //bool AutomationRequestPriority(string iQueueName, string strRoutingKey, string iMessage, int iMsgPriority, string iGroupName, string iTenantName);
        bool AutomationRequestPriority(string iQueueName, string strRoutingKey, Dictionary<string, object> iMessage1, int iMsgPriority, string iGroupName, string iTenantName);

        [OperationContract]
        int AddUser(string domainname, string userid, string pwd, int groupid, string groupname, int tenantid, string strUserRole, string createdby);

        [OperationContract]
        int AddTenant(string TenantName,string owner, int groupid, int iTenantId, string createdBy);

        [OperationContract]
        int DeleteTenant(int groupid, int TenantId,string CurrentUser);

        [OperationContract]
        int GetTenantId(int groupid, int tenantid,string TenantName);

        [OperationContract]
        int GetGroupId(string groupName, int tenantid);

        [OperationContract]
        int AddGroup(string groupName, int groupid, int tenantid, string createdby);

        [OperationContract]
        int DeleteGroup(int id,int groupId,int tenantId, string groupName,string currentUser);

        [OperationContract]
        int UpdateDefaultVersion(int iTenantId, int groupid, string ProcessId, string updatedVersion, bool isLatest);

        [OperationContract]
        int AddProcess(string ProcessName, int EnvironmentName, int groupid, int tenantid, string ProcessVersion, bool latestVersion, string createdBy);
        
        [OperationContract]
        int AddProcessWithZip(string ProcessName, int groupid, int TenantId, string ProcessVersion, bool LatestVersion, string createdBy, byte[] ZipDataFile);

        [OperationContract]
        int AddBot(string strBotName, string strBotId, string pwd, string botkey, string strMachineName, int groupid, int tenantid, string createdby);

        //[OperationContract]
        //DataTable CheckBot(string MachineName);

        [OperationContract]
        int AddConfigParameters(string strParameterName, string strParameterValue, int iAccessLevelProcessId, int groupid, int tenantid, string createdby);

        [OperationContract]
        int DeleteConfigParameters(int iParameterId, int groupid, int tenantid, string strCurrentUser);

        [OperationContract]
        int DeleteUser(string strId, int groupid, int tenantid, string createdby);

        [OperationContract]
        int DeleteProcess(string strId, int groupid, int tenantid, string createdby);

        [OperationContract]
        int DeleteProcessVersion(string strProcessId, string strProcessName, string strProcessVersion, int groupid, int tenantid, string createdby);

        [OperationContract]
        int DeleteQueueToBotMapping(string strBotId, int groupid, int tenantid, string createdby);

        [OperationContract]
        int DeleteUserToBotMapping(string strId, int groupid, int tenantid, string createdby);

        [OperationContract]
        int UpdateCustomRoleBasedAccess(bool QDetailsDevVal, bool QDetailsProdVal, bool QDetailsTestVal, bool AddSchedDevVal, bool AddSchedProdVal, bool AddSchedTestVal, bool AddUserDevVal, bool AddUserProdVal, bool AddUserTestVal,
            bool AddRobotDevVal, bool AddRobotProdVal, bool AddRobotTestVal, bool AddQDevVal, bool AddQProdVal, bool AddQTestVal, bool AddGroupDevVal, bool AddGroupProdVal, bool AddGroupTestVal,
            bool AddQueToBotDevVal, bool AddQueToBotProdVal, bool AddQueToBotTestVal, bool AddBotToUserDevVal, bool AddBotToUserProdVal, bool AddBotToUserTestVal,
            bool BotLogDevVal, bool BotLogProdVal, bool BotLogTestVal, bool AuditTrailDevVal, bool AuditTrailProdVal, bool AuditTrailTestVal, int groupid, int tenantid);

        [OperationContract]
        DataTable GetCustomRoleBasedAccess(int groupid, int iTenantId);

        [OperationContract]
        DataTable LoginUser(string domainname, string userid, string pwd, string tenantName, string groupName);
        
        [OperationContract]
        int AssignBotToUser(string strBotId, string strUserId, string createdby, int groupid, int tenantid);

        [OperationContract]
        int AssignQueueToBot(string strBotId, string queuename, string createdby, int groupid, int tenantid);

        [OperationContract]
        DataTable GetBots(int groupid, int tenantid);

        [OperationContract]
        DataTable GetGroups(int groupid, int tenantid);

        [OperationContract]
        DataTable GetConfigParameters(int groupid, int tenantid);

        [OperationContract]
        DataTable GetQueues(int groupid, int tenantid);

        [OperationContract]
        DataTable GetDashboardBots(int groupid, int tenantid, string userid);

        [OperationContract]
        DataTable GetQueueToBotMapping(int groupid, int tenantid);

        [OperationContract]
        DataTable GetUserToBotMapping(string userid, int groupid, int TenantId);

        [OperationContract]
        DataTable GetBotStartDetails(string botname,string MachineName,int groupid, int tenantid);

        [OperationContract]
        DataTable GetBotStartDetailsFromDesktop(string userid, string machinename);
        //DataTable GetBotStartDetailsFromDesktop(string UserName, int groupId,int tenantId,string machinename);

        [OperationContract]
        DataTable GetStompDetails(string botid, int groupid, int tenantid);
        [OperationContract]
        DataTable GetRQDetails(string botid);

        [OperationContract]
        DataTable GetLogsForDashboardBots(String strbotid, string strmachinename, string StartTime, string EndTime, int groupid, int tenantid);

        [OperationContract]
        DataTable GetUsers(int groupid, int tenantid);

        [OperationContract]
        DataTable GetAllTenants(int groupid, int tenantid);

        [OperationContract]
        int InsertIntoLogger(string MachineName, string UserName,string RobotName,string ProcessName, DateTime dateUtc,string Logger,string Message, int groupid, int tenantid);

        [OperationContract]
        DataTable GetLog(string userid, int groupid, int tenantid);

        [OperationContract]
        DataTable GetAuditTrail(int groupid, int tenantid, string userid);

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        void LogDataToDB(DataTable LogData);
        //[OperationContract]
        //bool AutomationRequest(RequestInput _requestInput);

       // [OperationContract]
       // bool PublishProcess(PublishProcessEntity publishProcessEntity);

        [OperationContract]
        //DataTable GetProcessDetails(string ProcessName, int groupid, int tenantid);
        DataTable GetProcessDetails(string ProcessName, string groupname, string tenantname);
        
        [OperationContract]
        DataTable DownloadAutomationZipBinary(string ProcessName, string ProcessVersion, string AutomationGroupName, string TenantName/*, int groupid, int tenantid*/);
        // TODO: Add your service operations here

        [OperationContract]
        DataTable GetProcessDataWitoutZipFile(int groupid, int tenantid);

        [OperationContract]
        DataTable LoadProcessVersion(int groupid, int tenantid, string ProcessId);

        /*Delete Group Contracts*/
        [OperationContract]
        int DeleteGroupWithAllRelatedData(int groupid, int tenantid, string createdby);

        [OperationContract]
        int UpdateIsactiveStatusGroupRelatedTables(int groupid, int isactive);

        [OperationContract]
        int CheckIsactiveStatusGroup(int groupid);

        [OperationContract]
        DataTable GetCountToDeleteGroup(int groupid);
        /*End of delete Group Contract*/
        [OperationContract]
        int DeleteQueue(string queueName, int tenantid, int groupid);
        [OperationContract]
        int PurgeQueue(string queueName, int tenantid);
        [OperationContract]
        int AddQueue(string queueName, string exchangeName, int tenantid, int groupid);
        [OperationContract]
        DataTable getQueueNames(int tenantid, int groupid);

        //SignalR Service Contract
        //[OperationContract(IsOneWay = true)]
        //void sendMessageToClient();

        #region GetCredentials
        [OperationContract]
        DataTable GetCredentials(string credentialName, int TenantId, int groupid);
        #endregion

        [OperationContract]
        DataTable GetProcessQMapping(int groupid, int tenantid);
        [OperationContract]
        int AddProcessQueueMapping(string processName, string groupName, string tenantName);

        [OperationContract]
        int deleteprocessQMapping(string processname, int GroupId, int TenanatId);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    [DataContract]
    public class QueueValue
    {
        [DataMember]
        public int MessagePriority { get; set; }

        [DataMember]
        public Dictionary<string, Object> Items { get; set; }

    }
}
