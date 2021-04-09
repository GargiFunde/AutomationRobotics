using System;
using System.Data;
using System.Diagnostics;
using System.IO;

public class ServiceInterface
{
    ServiceReference1.BOTServiceClient bOTServiceClient = null;
   
    public int Adduser(string domainname, string userid, string pwd, int groupid, string groupname, int tenantid, string strUserRole, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.AddUser(domainname, userid, pwd, groupid, groupname, tenantid, strUserRole, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }

    public DataTable GetProcessQMapping(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetProcessQMapping(groupid, tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    public int deleteprocessQMapping(string processname, int GroupId, int TenanatId)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.deleteprocessQMapping(processname, GroupId, TenanatId);
        }
        catch (Exception ex)
        {
            return result;
        }
        return result;
    }

    public int AddProcessQueueMapping(string processName, string groupName, string tenantName)
    {

        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.AddProcessQueueMapping(processName, groupName, tenantName);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;

    }



    public int getRoleId(int TenantId, string rolename, int groupid)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getRoleId(TenantId, rolename, groupid);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }

    public int DeleteBot(string botname, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteBot(botname, groupid, tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }

    #region ChangeGroup
    public int ChangeGroup(string currentRoleCG, string userName, string newRole, int tenantid)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.ChangeGroup(currentRoleCG, userName, newRole, tenantid);
        }
        catch (Exception ex)
        {
            Debug.Write("Exception while changing Group : " + ex.Message);
            return 0;
        }
        return result;
    }


    #endregion



    //To Update A queue *Sanket*
    public int updateQueueToBotMapping(int groupid, int tenantid, string botname, string QueueName)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.updateQueueToBotMapping(groupid, tenantid, botname, QueueName);
        }
        catch (Exception ex)
        {

            return 0;
        }
        return result;
    }

    public int updateBotNameTouserBotMapping(int groupid, int tenantid, string botname, string username)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.updateBotNameTouserBotMapping(groupid, tenantid, botname, username);
        }
        catch (Exception ex)
        {

            return 0;
        }
        return result;
    }


    public int DeleteProcessSchedule(string strId, string strprocessename, string strBotName, string strChronExp, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteProcessSchedule(strId, strprocessename, strBotName, strChronExp, groupid, tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }


    public DataTable GetCountScheduleRelatedBot(int tenantid, string botName)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetCountScheduleRelatedBot(tenantid, botName);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }


    public int CheckScheduleStatus(String botname, int tenantid)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.CheckScheduleStatus(botname, tenantid);
        }
        catch (Exception ex)
        {

            return 0;
        }
        return result;
    }




    /*Delete Group Service Interfaces*/
    public int CheckIsactiveStatusGroup(int groupid)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.CheckIsactiveStatusGroup(groupid);
        }
        catch (Exception ex)
        {

            return 0;
        }
        return result;
    }


    public int UpdateIsactiveStatusGroupRelatedTables(int groupid, int isactive)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.UpdateIsactiveStatusGroupRelatedTables(groupid, isactive);
        }
        catch (Exception ex)
        {
            return result;
        }
        return result;
    }


    public DataTable GetCountToDeleteGroup(int groupid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetCountToDeleteGroup(groupid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }



    public int DeleteGroupWithAllRelatedData(int groupid, int tenantid, string CurrentUser)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteGroupWithAllRelatedData(groupid, tenantid, CurrentUser);
        }
        catch (Exception ex)
        {
            Debug.Write("UpdateDefaultVersion Exception : " + ex.Message);
            return 0;
        }
        return result;
    }
    /*End Delete Group Service Interface*/


    /*DashBoard Card Details Service*/
    public int[] GetDashboardData(int groupid, int tenantid)
    {
        int[] results = new int[0];
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }

            results = bOTServiceClient.GetDetailsDashboard(groupid, tenantid);
        }
        catch (Exception)
        {

        }
        return results;
    }

    //Process on Masterpage for updating Password
    public int UpdateUserPassword(string userid, string OldPassword, string NewPassword, int groupid, int tenantid)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.UpdateUserPassword(userid, OldPassword, NewPassword, groupid, tenantid);
        }
        catch (Exception ex)
        {
            return result;
        }
        return result;
    }

    /*Adding Add Process Method on 27/04/2019*/
    public int AddProcess(string ProcessName, int EnvironmentName, int groupid, int tenantid, string ProcessVersion, bool latestVersion, string createdBy)
    {

        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            int result = bOTServiceClient.AddProcess(ProcessName, EnvironmentName,groupid, tenantid, ProcessVersion, latestVersion, createdBy);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return 1;
    }

    /*Role Based Access Start*/
    public DataTable GetRoleBaseAccess(int groupid, int tenantid)
    {

        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetRoleBaseAccess(groupid, tenantid);
        }
        catch (Exception ex)
        {

        }
        return result;
    }

    public DataTable GetRoleAccess(int roleid, int groupid, int tenantid, string roleName)
    {

        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {


                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetRoleAccess(roleid, groupid, tenantid, roleName);
        }
        catch (Exception ex)
        {

        }
        return result;
    }
    /*User Based Access Start*/
    public DataTable GetUserBaseAccess(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetUserBaseAccess(groupid, tenantid);
        }
        catch (Exception ex)
        {

        }
        return result;
    }

    public DataTable GetPageAccess(int roleid, int groupid, int tenantid, string pagename)
    {

        DataTable result = null;
        try
        {

            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetPageAccess(roleid, groupid, tenantid, pagename);
            //remember passing the parameters here vinay
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);


        }


        return result;
    }

    public int SaveRoleAccess(string roleName, int roleid, int tenantid, string role, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bQueueManagementR, bool bQueueManagementC, bool bQueueManagementE, bool bQueueManagementD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bProcessManagementR, bool bProcessManagementC, bool bProcessManagementE, bool bProcessManagementD, bool bDetailLogR, bool bDetailLogC, bool bDetailLogE, bool bDetailLogD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.SaveRoleAccess(roleName, roleid, tenantid, role, bBotDashboardR, bBotDashboardC, bBotDashboardE, bBotDashboardD, bqueueR, bqueueC, bqueueE, bqueueD, bAddScheduleR, bAddScheduleC, bAddScheduleE, bAddScheduleD, bAddUserR, bAddUserC, bAddUserE, bAddUserD, bAddRobotR, bAddRobotC, bAddRobotE, bAddRobotD, bQueueManagementR, bQueueManagementC, bQueueManagementE, bQueueManagementD, bAssignQueueBotR, bAssignQueueBotC, bAssignQueueBotE, bAssignQueueBotD, bAssignBotUserR, bAssignBotUserC, bAssignBotUserE, bAssignBotUserD, bBotLogR, bBotLogC, bBotLogE, bBotLogD, bAuditTrailR, bAuditTrailC, bAuditTrailE, bAuditTrailD, bScheduleDetailsR, bScheduleDetailsC, bScheduleDetailsE, bScheduleDetailsD, bConfigurationR, bConfigurationC, bConfigurationE, bConfigurationD, bProcessManagementR, bProcessManagementC, bProcessManagementE, bProcessManagementD, bDetailLogR, bDetailLogC, bDetailLogE, bDetailLogD, bPromoteDemoteR, bPromoteDemoteC, bPromoteDemoteE, bPromoteDemoteD);
        }
        catch (Exception e)
        {
            Console.WriteLine("Excption");

        }
        return result;
    }




    /*Save role based access for GROUPS*/
    //public int SaveRoleBaseAccess(int groupid, int tenantid, string role, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bAddQueueR, bool bAddQueueC, bool bAddQueueE, bool bAddQueueD, bool bAddGroupR, bool bAddGroupC, bool bAddGroupE, bool bAddGroupD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bAddUpdateProcessR, bool bAddUpdateProcessC, bool bAddUpdateProcessE, bool bAddUpdateProcessD, bool bUploadProcessR, bool bUploadProcessC, bool bUploadProcessE, bool bUploadProcessD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD)
    //{
    //    int result = 0;
    //    try
    //    {
    //        if (bOTServiceClient == null)
    //        {
    //            bOTServiceClient = new ServiceReference1.BOTServiceClient();
    //        }
    //        result = bOTServiceClient.SaveRoleBaseAccess(groupid, tenantid, role, bBotDashboardR, bBotDashboardC, bBotDashboardE, bBotDashboardD, bqueueR, bqueueC, bqueueE, bqueueD, bAddScheduleR, bAddScheduleC, bAddScheduleE, bAddScheduleD, bAddUserR, bAddUserC, bAddUserE, bAddUserD, bAddRobotR, bAddRobotC, bAddRobotE, bAddRobotD, bAddQueueR, bAddQueueC, bAddQueueE, bAddQueueD, bAddGroupR, bAddGroupC, bAddGroupE, bAddGroupD, bAssignQueueBotR, bAssignQueueBotC, bAssignQueueBotE, bAssignQueueBotD, bAssignBotUserR, bAssignBotUserC, bAssignBotUserE, bAssignBotUserD, bBotLogR, bBotLogC, bBotLogE, bBotLogD, bAuditTrailR, bAuditTrailC, bAuditTrailE, bAuditTrailD, bScheduleDetailsR, bScheduleDetailsC, bScheduleDetailsE, bScheduleDetailsD, bConfigurationR, bConfigurationC, bConfigurationE, bConfigurationD, bAddUpdateProcessR, bAddUpdateProcessC, bAddUpdateProcessE, bAddUpdateProcessD, bUploadProcessR, bUploadProcessC, bUploadProcessE, bUploadProcessD, bPromoteDemoteR, bPromoteDemoteC, bPromoteDemoteE, bPromoteDemoteD);
    //    }
    //    catch (Exception)
    //    {
    //        Console.WriteLine("Excption");

    //    }
    //    return result;
    //}
    public int SaveRoleBaseAccess(int groupid, int tenantid, string role, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bQueueManagementR, bool bQueueManagementC, bool bQueueManagementE, bool bQueueManagementD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bProcessManagementR, bool bProcessManagementC, bool bProcessManagementE, bool bProcessManagementD, bool bDetailLogR, bool bDetailLogC, bool bDetailLogE, bool bDetailLogD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.SaveRoleBaseAccess(groupid, tenantid, role, bBotDashboardR, bBotDashboardC, bBotDashboardE, bBotDashboardD, bqueueR, bqueueC, bqueueE, bqueueD, bAddScheduleR, bAddScheduleC, bAddScheduleE, bAddScheduleD, bAddUserR, bAddUserC, bAddUserE, bAddUserD, bAddRobotR, bAddRobotC, bAddRobotE, bAddRobotD, bQueueManagementR, bQueueManagementC, bQueueManagementE, bQueueManagementD, bAssignQueueBotR, bAssignQueueBotC, bAssignQueueBotE, bAssignQueueBotD, bAssignBotUserR, bAssignBotUserC, bAssignBotUserE, bAssignBotUserD, bBotLogR, bBotLogC, bBotLogE, bBotLogD, bAuditTrailR, bAuditTrailC, bAuditTrailE, bAuditTrailD, bScheduleDetailsR, bScheduleDetailsC, bScheduleDetailsE, bScheduleDetailsD, bConfigurationR, bConfigurationC, bConfigurationE, bConfigurationD, bProcessManagementR, bProcessManagementC, bProcessManagementE, bProcessManagementD, bDetailLogR, bDetailLogC, bDetailLogE, bDetailLogD, bPromoteDemoteR, bPromoteDemoteC, bPromoteDemoteE, bPromoteDemoteD);
        }
        catch (Exception)
        {
            Console.WriteLine("Excption");

        }
        return result;
    }
    /*Save role based access for USERS in Users Page*/
    //public int SaveRoleBaseAccessUser(int IGroupID, string usernameI, int ITenantID, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bAddQueueR, bool bAddQueueC, bool bAddQueueE, bool bAddQueueD, bool bAddGroupR, bool bAddGroupC, bool bAddGroupE, bool bAddGroupD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bAddUpdateProcessR, bool bAddUpdateProcessC, bool bAddUpdateProcessE, bool bAddUpdateProcessD, bool bUploadProcessR, bool bUploadProcessC, bool bUploadProcessE, bool bUploadProcessD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD)
    //{
    //    int result = 0;
    //    try
    //    {
    //        if (bOTServiceClient == null)
    //        {
    //            bOTServiceClient = new ServiceReference1.BOTServiceClient();
    //        }
    //        result = bOTServiceClient.SaveRoleBaseAccessUser(IGroupID, usernameI, ITenantID, bBotDashboardR, bBotDashboardC, bBotDashboardE, bBotDashboardD, bqueueR, bqueueC, bqueueE, bqueueD, bAddScheduleR, bAddScheduleC, bAddScheduleE, bAddScheduleD, bAddUserR, bAddUserC, bAddUserE, bAddUserD, bAddRobotR, bAddRobotC, bAddRobotE, bAddRobotD, bAddQueueR, bAddQueueC, bAddQueueE, bAddQueueD, bAddGroupR, bAddGroupC, bAddGroupE, bAddGroupD, bAssignQueueBotR, bAssignQueueBotC, bAssignQueueBotE, bAssignQueueBotD, bAssignBotUserR, bAssignBotUserC, bAssignBotUserE, bAssignBotUserD, bBotLogR, bBotLogC, bBotLogE, bBotLogD, bAuditTrailR, bAuditTrailC, bAuditTrailE, bAuditTrailD, bScheduleDetailsR, bScheduleDetailsC, bScheduleDetailsE, bScheduleDetailsD, bConfigurationR, bConfigurationC, bConfigurationE, bConfigurationD, bAddUpdateProcessR, bAddUpdateProcessC, bAddUpdateProcessE, bAddUpdateProcessD, bUploadProcessR, bUploadProcessC, bUploadProcessE, bUploadProcessD, bPromoteDemoteR, bPromoteDemoteC, bPromoteDemoteE, bPromoteDemoteD);

    //    }
    //    catch (Exception ex)
    //    {
    //        return 0;
    //    }
    //    return result;
    //}
    public int SaveRoleBaseAccessUser(int IGroupID, string usernameI, int ITenantID, bool bBotDashboardR, bool bBotDashboardC, bool bBotDashboardE, bool bBotDashboardD, bool bqueueR, bool bqueueC, bool bqueueE, bool bqueueD, bool bAddScheduleR, bool bAddScheduleC, bool bAddScheduleE, bool bAddScheduleD, bool bAddUserR, bool bAddUserC, bool bAddUserE, bool bAddUserD, bool bAddRobotR, bool bAddRobotC, bool bAddRobotE, bool bAddRobotD, bool bQueueManagementR, bool bQueueManagementC, bool bQueueManagementE, bool bQueueManagementD, bool bAssignQueueBotR, bool bAssignQueueBotC, bool bAssignQueueBotE, bool bAssignQueueBotD, bool bAssignBotUserR, bool bAssignBotUserC, bool bAssignBotUserE, bool bAssignBotUserD, bool bBotLogR, bool bBotLogC, bool bBotLogE, bool bBotLogD, bool bAuditTrailR, bool bAuditTrailC, bool bAuditTrailE, bool bAuditTrailD, bool bScheduleDetailsR, bool bScheduleDetailsC, bool bScheduleDetailsE, bool bScheduleDetailsD, bool bConfigurationR, bool bConfigurationC, bool bConfigurationE, bool bConfigurationD, bool bProcessManagementR, bool bProcessManagementC, bool bProcessManagementE, bool bProcessManagementD, bool bDetailLogR, bool bDetailLogC, bool bDetailLogE, bool bDetailLogD, bool bPromoteDemoteR, bool bPromoteDemoteC, bool bPromoteDemoteE, bool bPromoteDemoteD)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.SaveRoleBaseAccessUser(IGroupID, usernameI, ITenantID, bBotDashboardR, bBotDashboardC, bBotDashboardE, bBotDashboardD, bqueueR, bqueueC, bqueueE, bqueueD, bAddScheduleR, bAddScheduleC, bAddScheduleE, bAddScheduleD, bAddUserR, bAddUserC, bAddUserE, bAddUserD, bAddRobotR, bAddRobotC, bAddRobotE, bAddRobotD, bQueueManagementR, bQueueManagementC, bQueueManagementE, bQueueManagementD, bAssignQueueBotR, bAssignQueueBotC, bAssignQueueBotE, bAssignQueueBotD, bAssignBotUserR, bAssignBotUserC, bAssignBotUserE, bAssignBotUserD, bBotLogR, bBotLogC, bBotLogE, bBotLogD, bAuditTrailR, bAuditTrailC, bAuditTrailE, bAuditTrailD, bScheduleDetailsR, bScheduleDetailsC, bScheduleDetailsE, bScheduleDetailsD, bConfigurationR, bConfigurationC, bConfigurationE, bConfigurationD, bProcessManagementR, bProcessManagementC, bProcessManagementE, bProcessManagementD, bDetailLogR, bDetailLogC, bDetailLogE, bDetailLogD, bPromoteDemoteR, bPromoteDemoteC, bPromoteDemoteE, bPromoteDemoteD);

        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }

    /*Role Based Access End*/

    /*Adding Add Process Method on 27/04/2019*/
    public int AddTenant(string TenantName,string owner, int groupid, int tenantid,string createdBy)
    {
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            int result = bOTServiceClient.AddTenant(TenantName,owner,groupid,tenantid, createdBy);
        }
        catch (Exception ex)
        {
            Debug.Write("UpdateDefaultVersion Exception : " + ex.Message);
            return 0;
        }
        return 1;
    }

    
    public DataTable GetProcessSchedules(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetProcessSchedules(groupid, tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }
    public DataTable GetCustomRoleBasedAccess(int groupid,int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetCustomRoleBasedAccess(groupid,tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }
    

    public int UpdateCustomRoleBasedAccess(bool QDetailsDevVal, bool QDetailsProdVal, bool QDetailsTestVal, bool AddSchedDevVal, bool AddSchedProdVal, bool AddSchedTestVal, bool AddUserDevVal, bool AddUserProdVal, bool AddUserTestVal,
        bool AddRobotDevVal, bool AddRobotProdVal, bool AddRobotTestVal, bool AddQDevVal, bool AddQProdVal, bool AddQTestVal, bool AddGroupDevVal, bool AddGroupProdVal, bool AddGroupTestVal,
        bool AddQueToBotDevVal, bool AddQueToBotProdVal, bool AddQueToBotTestVal, bool AddBotToUserDevVal, bool AddBotToUserProdVal, bool AddBotToUserTestVal,
        bool BotLogDevVal, bool BotLogProdVal, bool BotLogTestVal, bool AuditTrailDevVal, bool AuditTrailProdVal, bool AuditTrailTestVal, int groupid, int tenantid)
    {
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            int result = bOTServiceClient.UpdateCustomRoleBasedAccess(QDetailsDevVal, QDetailsProdVal, QDetailsTestVal, AddSchedDevVal, AddSchedProdVal, AddSchedTestVal, AddUserDevVal, AddUserProdVal, AddUserTestVal,
                AddRobotDevVal, AddRobotProdVal, AddRobotTestVal, AddQDevVal, AddQProdVal, AddQTestVal, AddGroupDevVal, AddGroupProdVal, AddGroupTestVal,
                AddQueToBotDevVal, AddQueToBotProdVal, AddQueToBotTestVal, AddBotToUserDevVal, AddBotToUserProdVal, AddBotToUserTestVal,
                BotLogDevVal, BotLogProdVal, BotLogTestVal, AuditTrailDevVal, AuditTrailProdVal, AuditTrailTestVal, groupid,tenantid);
            
        }
        catch (Exception ex)
        {
            return 0;
        }
        return 1;
    }







    public int AddQueueDetails(string queueName,int groupid, int tenantid, string createdby)
    {
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            int result = bOTServiceClient.AddQueueDetails(queueName, groupid,tenantid, createdby);
        }
        catch (Exception)
        {
            return 0;
        }
        return 1;
    }

 public DataTable GetRQDetailsForBotDashboard(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {


                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetRQDetailsForBotDashboard(groupid, tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }


    #region Complete Logs Service Interface Method
    public int insertLog(string Message,string detailLog, int groupid, int tenantid)
    {
        int resultForInsertLog = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            resultForInsertLog = bOTServiceClient.insertLog(Message,detailLog, groupid, tenantid);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return resultForInsertLog;

    }
    #endregion



    #region Get Activity Logs Service Interface Method
    public DataTable getActivityLog(int groupid, int tenantid)
    {
        DataTable resultForActivityLog = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            resultForActivityLog = bOTServiceClient.getActivityLog(groupid, tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return resultForActivityLog;

    }
    #endregion

    #region Get Activity Logs Service Interface Method
    public DataTable getCompleteLogs(int groupid, int tenantid)
    {
        DataTable resultForActivityLog = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            resultForActivityLog = bOTServiceClient.getCompleteLogs(groupid, tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return resultForActivityLog;

    }
    #endregion


    #region Activity Logs Service Interface Method
    public int insertActivityLog(string domainName,string userName,string groupName,string action, int groupid, int tenantid)
    {
        int resultForInsertLog = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            resultForInsertLog = bOTServiceClient.insertActivityLog(domainName, userName, groupName, action, groupid, tenantid);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return resultForInsertLog;

    }
    #endregion


    public DataTable GetLogsForDashboardBots(string strbotid, string strmachinename, string StartTime, string EndTime, int groupid,int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetLogsForDashboardBots(strbotid, strmachinename, StartTime, EndTime ,groupid, tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;

    }


    public int GetGroupId(string groupName, int tenantid)
    {
        int result = 0;
        try
        {

            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetGroupId(groupName, tenantid);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }
    public int GetTenantId(string TenantName,int groupid, int tenantid)
    {
        int result = 0;
        try
        {
            
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
             result = bOTServiceClient.GetTenantId(groupid,tenantid,TenantName);
        }
        catch (Exception ex)
        {
            Debug.Write("UpdateDefaultVersion Exception : " + ex.Message);
            return 0;
        }
        return result;
    }

    
    /*Delete Tenant Method on 17/July/2019*/
    public int DeleteTenant(int groupid, int tenantid, string CurrentUser)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteTenant(groupid,  tenantid, CurrentUser);
        }
        catch (Exception ex)
        {
            Debug.Write("UpdateDefaultVersion Exception : " + ex.Message);
            return 0;
        }
        return result;
    }
    /*Adding Add Process Method on 27/04/2019*/
    public int UpdateDefaultVersion(int groupid, int tenantid, string ProcessId,string updatedVersion,bool isLatest)
    {
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            int result = bOTServiceClient.UpdateDefaultVersion( groupid,  tenantid, ProcessId, updatedVersion, isLatest);
        }
        catch (Exception ex)
        {
            Debug.Write("UpdateDefaultVersion Exception : "+ex.Message);
            return 0;
        }
        return 1;
    }
    

    /*Adding Add Process With Zip Method on 27/04/2019*/
    public int AddProcessWithZip(string ProcessName, int groupid, int tenantid, string ProcessVersion,bool LatestVersion,string createdBy,byte[] ZipDataFile)
    {

        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            int result = bOTServiceClient.AddProcessWithZip(ProcessName,  groupid,  tenantid, ProcessVersion, LatestVersion, createdBy, ZipDataFile);
            //int result = bOTServiceClient.AddProcessWithZip1(ProcessName, EnvironmentName, TenantId, ProcessVersion, LatestVersion, createdBy, ZipDataFile);
        }
        catch (Exception ex)
        {
            Debug.Write("Exception For REsult Piyush**************",ex.Message);
            return 0;
        }
        return 1;
    }

    





    //public bool LoginUser(string domainname, string userid, string pwd, int tenantid)
    public DataTable LoginUser(string domainname, string userid, string pwd,string tenantName,string groupName)
        {
        //bool result = false;
        //int result = 0;
      //  File.AppendAllText(@"c:\E2EDebug\file.txt", "In Service Interface Object" + "\n");
       // File.AppendAllText(@"c:\E2EDebug\file.txt", "domainname:" + domainname + " userid: "+ userid+ " pwd: "+ pwd+ " .tenantName:"+ tenantName+ " .groupName"+ groupName + "\n");
        DataTable result = null;
        try
            {
                if (bOTServiceClient == null)
                {
               // File.AppendAllText(@"c:\E2EDebug\file.txt", "Creating BotServer Client" + "\n");
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
              //  File.AppendAllText(@"c:\E2EDebug\file.txt", "Created BotServer Client" + "\n");
            }
            //int iresult = bOTServiceClient.LoginUser(domainname, userid, pwd, tenantid);
           // File.AppendAllText(@"c:\E2EDebug\file.txt", "Calling LogIn Method in Service" + "\n");
            result = bOTServiceClient.LoginUser(domainname, userid, pwd, tenantName, groupName);
           // File.AppendAllText(@"c:\E2EDebug\file.txt", "Row Count : " + result.Rows.Count.ToString() + "\n");
            //if (iresult == 1)
            //    return true;
            //else
            //    return false;

            //if (0 == iresult)
            //    return 0;
            //else
            //    return iresult;
        }
        //catch (Exception ex)
        //{
        //    return 0;
        //}
        //return result;
        catch (Exception ex)
            {
            //File.AppendAllText(@"c:\E2EDebug\file.txt", "Exception " + "Exception : " + ex.Message + "\n");
          //  File.AppendAllText(@"c:\E2EDebug\file.txt", "Exception => Row Count : "+ result.Rows.Count.ToString()+ "Exception : "+ex.Message);
            return null;
            }
            return result;
    }
        public int AddBot(string strBotName, string strBotId, string pwd, string botkey, string strMachineName, int groupid, int tenantid ,string createdby)
        {
            int result = 0;
            try
            {
                if (bOTServiceClient == null)
                {

                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.AddBot(strBotName, strBotId, pwd, botkey, strMachineName,  groupid,  tenantid, createdby);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }

    public DataTable CheckBot(string MachineName)
    {
        DataTable result = null ;
        try
        {
            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            //result = bOTServiceClient.CheckBot(MachineName);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    public int AddSchedule(string strQueueName, string strBotName, string strChronExp,string stopAfter, int groupid, int tenantid ,string createdby)
        {
        int result = 0;
            try
            {
                if (bOTServiceClient == null)
                {

                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.AddSchedule(strQueueName,strBotName, strChronExp,stopAfter,  groupid,  tenantid, createdby);
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

    #region Add Schedule for Process 
    public int AddScheduleForProcess(string strProcessName, string strBotName, string strChronExp, string stopAfter, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.AddScheduleForProcess(strProcessName, strBotName, strChronExp, stopAfter, groupid, tenantid, createdby);
        }
        catch (Exception ex)
        {
            return result;
        }
        return result;
    }
    #endregion

    public int AddConfigParameters(string strParameterName, string strParameterValue, int iAccessLevelProcessId, int groupid, int tenantid, string createdby)
    {
        try
        {
            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            int result = bOTServiceClient.AddConfigParameters(strParameterName, strParameterValue, iAccessLevelProcessId,  groupid,  tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return 1;
    }
    public int AssignBotToUser(string strBotId, string strUserId, string createdby, int groupid, int tenantid)
        {
            try
            {
                if (bOTServiceClient == null)
                {

                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                int result = bOTServiceClient.AssignBotToUser(strBotId, strUserId, createdby,  groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }
        public int AssignQueueToBot(string strBotId, string queuename, string createdby, int groupid, int tenantid)
        {
            try
            {
                if (bOTServiceClient == null)
                {

                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                int result = bOTServiceClient.AssignQueueToBot(strBotId, queuename, createdby,  groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }
        public DataTable GetQueueToBotMapping(int groupid, int tenantid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {

                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetQueueToBotMapping( groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

        public DataTable GetUserToBotMapping(string userid, int groupid, int tenantid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {

                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetUserToBotMapping(userid,  groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }
        public DataTable GetBots(int groupid, int tenantid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetBots( groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }
        public DataTable GetSchedules(int groupid, int tenantid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetSchedules( groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }
    public DataTable GetConfigParameters(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetConfigParameters( groupid,  tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

        public DataTable GetQueues(int groupid, int tenantid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetQueues( groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

    public int CreateScheduleStatus(string QueueName, string BotName, string ChronExpression, string Status,int GroupId,int TenantId,string StartTime, string EndTime)
    {

        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            int result = bOTServiceClient.CreateScheduleStatus(QueueName, BotName, ChronExpression, Status, GroupId, TenantId, StartTime, EndTime);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return 1;
    }


    public int AddGroup(string groupName, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.AddGroup(groupName,  groupid,  tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }
    //Add Roles //Sanket
    public int AddRole(string roleName, int groupid, int tenantid, string createdby, string groupname)
    {

        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.AddRole(roleName, groupid, tenantid, createdby, groupname);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }

    public DataTable getRoledata(int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getRoledata(tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }
    public int DeleteGroup(int id,int groupId, int tenantId, string groupName, string currentUser)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteGroup(id,groupId,tenantId, groupName, currentUser);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }

    public DataTable GetGroups(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetGroups( groupid,  tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }
    public DataTable getGrounNames(int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getGrounNames(tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    public DataTable getRoleType(string groupname, int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getRoleType(groupname, groupid, tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    public DataTable GetScheduleStatus(int groupid, int tenantid,string status)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetScheduleStatus(groupid, tenantid,status);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    public DataTable GetBotStartDetails(string botname,string MachineName, int groupid, int tenantid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetBotStartDetails(botname,MachineName,  groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }


        public DataTable GetDashboardBots(int groupid, int tenantid, string userid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetDashboardBots( groupid,  tenantid, userid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

    public DataTable GetCountToDeleteTenant(int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetCountToDeleteTenant(tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }


    /*Charts */
    public DataTable GetData_createschedulestatus()
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetData_createschedulestatus();
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    public DataTable getMonthlyChartData(string status)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getMonthlyChartData(status);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }


    public DataTable GetddlData_createschedulestatus()
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetddlData_createschedulestatus();
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }


    public DataTable getChartData(string status)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getChartData(status);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    /*Disable and delete tenants and all his/her details*/
    public int CheckIsactiveStatusTenant(int tenantid)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.CheckIsactiveStatusTenant(tenantid);
        }
        catch (Exception ex)
        {

            return 0;
        }
        return result;
    }



    public int UpdateIsactiveStatusTenantRelatedTables(int tenantid, int isactive)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.UpdateIsactiveStatusTenantRelatedTables(tenantid, isactive);
        }
        catch (Exception ex)
        {
            return result;
        }


        return result;
    }

    public DataTable getDoughnutChartData()
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getDoughnutChartData();
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }



    /*End of chart services*/


    public int DeleteTenantWithAllRelatedData(int groupid, int tenantid, string CurrentUser)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteTenantWithAllRelatedData(groupid, tenantid, CurrentUser);
        }
        catch (Exception ex)
        {
            Debug.Write("UpdateDefaultVersion Exception : " + ex.Message);
            return 0;
        }
        return result;
    }



    public int DeleteSchedule(string strId, string strQueueName, string strBotName, string strChronExp, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
             result = bOTServiceClient.DeleteSchedule(strId, strQueueName, strBotName, strChronExp,  groupid,  tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }
    public int DeleteUser(string strId, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteUser(strId,  groupid,  tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }
    //public int DeleteBot(string strBotId, int groupid, int tenantid, string createdby)
    //    {
    //    int result = 0;
    //        try
    //        {
    //            if (bOTServiceClient == null)
    //            {
    //                bOTServiceClient = new ServiceReference1.BOTServiceClient();
    //            }
    //            result = bOTServiceClient.DeleteBot(strBotId,  groupid,  tenantid, createdby);
    //        }
    //        catch (Exception ex)
    //        {
    //            return 0;
    //        }
    //        return result;
    //    }
    public int DeleteConfigParameters(int iParameterId, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteConfigParameters(iParameterId,  groupid,  tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }
   
    public int DeleteQueueToBotMapping(string strBotId, int groupid, int tenantid, string createdby)
        {
        int result = 0;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                 result = bOTServiceClient.DeleteQueueToBotMapping(strBotId,  groupid,  tenantid, createdby);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }
    public int DeleteUserToBotMapping(string strBotId, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteUserToBotMapping(strBotId,  groupid,  tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }

    /*Delete Process added on 29/04/2019*/
    public int DeleteProcess(string strBotId, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
           result = bOTServiceClient.DeleteProcess(strBotId,  groupid,  tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return 1;
    }

    /*DeleteProcessVersion added on 19/06/2019*/
    public int DeleteProcessVersion(string strProcessId, string strProcessName, string strProcessVersion, int groupid, int tenantid, string createdby)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
           result = bOTServiceClient.DeleteProcessVersion(strProcessId, strProcessName,strProcessVersion,  groupid,  tenantid, createdby);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return 1;
    }

    public DataTable GetUsers(int groupid, int tenantid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetUsers( groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

    /*Get all Tenants from WEB API*/
    public DataTable GetAllTenants(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetAllTenants( groupid,  tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    public DataTable GetProcessDataWitoutFile(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetProcessDataWitoutZipFile( groupid,  tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    /*Get Process with Zip File Added on 29/04/2019*/
    public DataTable GetProcessDataWithFile(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.GetProcessDataWitoutZipFile( groupid,  tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    /*Get Load Process Version with Zip File Added on 29/04/2019*/
    public DataTable LoadProcessVersion(int groupid, int tenantid, string ProcessId)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.LoadProcessVersion( groupid,  tenantid, ProcessId);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    #region Queue Management
    public int DeleteQueue(string queueName, int tenantid, int groupid)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteQueue(queueName, tenantid, groupid);
        }
        catch (Exception ex)
        {
            return result;
        }
        return result;
    }

    public int PurgeQueue(string queueName, int tenantid)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
                result = bOTServiceClient.PurgeQueue(queueName, tenantid);
            }
        }
        catch (Exception ex)
        {
            return result;
        }
        return result;
    }

    public int AddQueue(string queueName, string exchangeName, int tenantid, int groupid)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.AddQueue(queueName, exchangeName, tenantid, groupid);
        }
        catch (Exception ex)
        {
            return result;
        }
        return result;
    }
    public DataTable getQueueNames(int tenantid, int groupid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {

                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getQueueNames(tenantid, groupid);
        }
        catch (Exception ex)
        {
            return  result;
        }
        return result;
    }
    #endregion


    public DataTable GetLog(string userid, int groupid, int tenantid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetLog(userid,  groupid,  tenantid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }
        public DataTable GetAuditTrail(int groupid,int tenantid, string userid)
        {
            DataTable result = null;
            try
            {
                if (bOTServiceClient == null)
                {
                    bOTServiceClient = new ServiceReference1.BOTServiceClient();
                }
                result = bOTServiceClient.GetAuditTrail(groupid,tenantid, userid);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

    //Asset Management Interfaces
    public int DeleteCredential(int id, int groupid, int tenantid, string CredentialName, string user)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteCredential(id, groupid, tenantid, CredentialName, user);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }

    public int DeleteAsset(int id, int groupid, int tenantid, string AssetName, string user)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.DeleteAsset(id, groupid, tenantid, AssetName, user);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }


    public string getKey()
    {
        string result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getKey();

        }
        catch (Exception)
        {

            return null;
        }
        return result;
    }



    public DataTable getCredentials(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
           //result = bOTServiceClient.getCredentials(groupid, tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    public DataTable getAssets(int groupid, int tenantid)
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getAssets(groupid, tenantid);
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }


    public int AddCredentials(string strAssetName, string strUserName, string Encrypted_Pwd, int tenantid, int groupid, string strcreatedBy)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.AddCredentials(strAssetName, strUserName, Encrypted_Pwd, tenantid, groupid, strcreatedBy);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }


    public int AddAssets(string strAssetName, string strValue, int tenantid, int groupid, string strcreatedBy)
    {
        int result = 0;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.AddAssets(strAssetName, strValue, tenantid, groupid, strcreatedBy);
        }
        catch (Exception ex)
        {
            return 0;
        }
        return result;
    }

    //Method to get MicrosoftOCR data 
    public DataTable getMicrosoftOCR()
    {
        DataTable result = null;
        try
        {
            if (bOTServiceClient == null)
            {
                bOTServiceClient = new ServiceReference1.BOTServiceClient();
            }
            result = bOTServiceClient.getMicrosoftOCR();
        }
        catch (Exception ex)
        {
            return null;
        }
        return result;
    }

    #region GetCredentials in Activity Designer
    //public DataTable GetCredentials(string assetName, int TenantId, int groupid)
    //{
    //    DataTable result = null;
    //    try
    //    {
    //        if (bOTServiceClient == null)
    //        {
    //            bOTServiceClient = new ServiceReference1.BOTServiceClient();
    //        }
    //        result = bOTServiceClient.GetCredentials(assetName, TenantId, groupid);
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    return result;
    //}

    #endregion

}
