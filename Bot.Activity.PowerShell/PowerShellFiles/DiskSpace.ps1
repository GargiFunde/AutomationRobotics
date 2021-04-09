
Function DiskSpace([Microsoft.SharePoint.Administration.SPFarm]$farm)   
{   
   
    Write-Host ""   
    write-host "Generating Disk space report" -fore Magenta   
       
    $output = $scriptbase + "\" + "DiskSpace.csv"   
    "Computer Name" + "," + "Drive" + "," + "Size in (GB)" + "," + "Free Space in (GB)" + "," + "Critical (*)"  | Out-File -Encoding Default -FilePath $Output;   
   
    foreach($server in $farm.Servers)   
        {   
        foreach($instance in $server.ServiceInstances)   
        {   
            # If the server has the timer service then stop the service   
            if($instance.TypeName -eq $timerServiceInstanceName)   
            {   
                [string]$serverName = $server.Name   
                write-host "Generating disk space report for server" $serverName -fore yellow   
   
                $drives = Get-WmiObject -ComputerName $serverName Win32_LogicalDisk | Where-Object {$_.DriveType -eq 3}     
   
                foreach($drive in $drives)     
                {     
                    $id = $drive.DeviceID     
   
                    $size = [math]::round($drive.Size / 1073741824, 2)     
   
                    $free = [math]::round($drive.FreeSpace  / 1073741824, 2)     
   
                    $pct = [math]::round($free / $size, 2) * 100     
   
                    if ($pct -lt 30)    
                    {    
                        $pct = $pct.ToString() + "% *** "    
                    }     
                    else   
                    {   
                        $pct = $pct.ToString() + " %"    
                    }     
   
                    $serverName + "," + $id + "," + $size + "," + $free + "," + $pct  | Out-File -Encoding Default  -Append -FilePath $Output;   
                    $pct = 0      
                }   
                write-host "Disk space report generated" -fore green   
            }   
        }   
    }   
   
}  








































































