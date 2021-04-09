Function CPUUtilization([Microsoft.SharePoint.Administration.SPFarm]$farm)   
{   
    Write-Host ""   
    write-host "Generating CPU utilization report" -fore Magenta   
       
    $output = $scriptbase + "\" + "CPUUtilization.csv"   
    "ServerName" + "," + "DeviceID" + "," + "LoadPercentage" + "," + "Status" | Out-File -Encoding Default -FilePath $Output;   
   
    foreach($server in $farm.Servers)   
    {   
        foreach($instance in $server.ServiceInstances)   
        {   
            # If the server has the timer service then stop the service   
            if($instance.TypeName -eq $timerServiceInstanceName)   
            {   
               [string]$serverName = $server.Name   
                write-host "Generating CPU utilization report for server" $serverName -fore yellow   
                $CPUDataCol = Get-WmiObject -Class Win32_Processor -ComputerName $ServerName    
                foreach($Data in $CPUDataCol)   
                {   
                    $serverName + "," + $Data.DeviceID + "," + $Data.loadpercentage + "," + $Data.status | Out-File -Encoding Default  -Append -FilePath $Output;   
                }   
                write-host "CPU utilization report generated" -fore green   
            }   
        }   
    }   
   
}  





















