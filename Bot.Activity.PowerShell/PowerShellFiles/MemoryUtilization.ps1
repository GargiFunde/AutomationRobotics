Function MemoryUtilization([Microsoft.SharePoint.Administration.SPFarm]$farm)   
{   
    Write-Host ""   
    write-host "Memory utilization report" -fore Magenta   
       
    $output = $scriptbase + "\" + "MemoryUtilization.csv"   
    "ServerName" + "," + "FreePhysicalMemory" + "," + "TotalVisibleMemorySize" + "," + "Status" | Out-File -Encoding Default -FilePath $Output;   
   
    foreach($server in $farm.Servers)   
        {   
        foreach($instance in $server.ServiceInstances)   
        {   
            # If the server has the timer service then stop the service   
            if($instance.TypeName -eq $timerServiceInstanceName)   
            {   
                [string]$serverName = $server.Name   
                write-host "Generating memory utilization report for server" $serverName -fore yellow   
                $MemoryCol = Get-WmiObject -Class Win32_OperatingSystem -ComputerName $ServerName   
                foreach($Data in $MemoryCol)   
                {   
                    $serverName + "," + $Data.FreePhysicalMemory + "," + $Data.TotalVisibleMemorySize + "," + $Data.status | Out-File -Encoding Default  -Append -FilePath $Output;   
                }   
                write-host "Memory utilization report generated" -fore green   
            }   
        }   
    }   
   
}  


















































