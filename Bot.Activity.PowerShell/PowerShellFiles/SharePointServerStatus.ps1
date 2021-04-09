Function SPServerStatus([Microsoft.SharePoint.Administration.SPFarm]$farm)   
{      
    Write-Host ""   
    write-host "SharePoint server status report" -fore Magenta   
       
    $output = $scriptbase + "\" + "SPServerStatus.csv"   
    "ServerName" + "," + "Role" + "," + "Status" + "," + "CanUpgrade" + "," + "NeedsUpgrade" | Out-File -Encoding Default -FilePath $Output;   
   
    foreach($server in $farm.Servers)   
    {   
       foreach($instance in $server.ServiceInstances)   
       {   
          # If the server has the timer service then stop the service   
           if($instance.TypeName -eq $timerServiceInstanceName)   
            {   
   
                $server.Name + "," + $server.role + "," + $server.status + "," + $server.canupgrade + "," + $server.NeedsUpgrade | Out-File -Encoding Default  -Append -FilePath $Output;   
                write-host "SP server status report generated" -fore green   
            }   
        }   
    }   
}  















































































