using Microsoft.AspNet.SignalR;
//using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class MyHub : Hub
{
    public void Hello()
    {
        Clients.All.hello();
    }
    //public void Send(string name, string message)
    public void Send(string sr,int tenantId)
    {
        // Call the addNewMessageToPage method to update clients       
        //Clients.All.addNewMessageToPage(sr);
        // Call the addNewMessageToPage method to update clients  
        //  string a = "1";
     
       Clients.Group(tenantId.ToString()).addNewMessageToPage(sr);
       // Clients.Group(a).addNewMessageToPage(sr);
    }

    /*Method Name: Join Group
    *Purpose    : When Client(Javscript or .NET ) Will call This method join Group
    *Note       : When user Login in application accordingly His tenant Name will become the group Name
    *             and When Bot Start Before Hand It Will Register The back office Bot for Same TenantName in Group
    ***/
    public async Task JoinGroup(string groupName)
    {
     
        await Groups.Add(Context.ConnectionId, groupName); // Is there any need to make it Await ?
    }



    public async Task LeaveGroup(string groupName)
    {
        await Groups.Remove(Context.ConnectionId, groupName);
    }
}