Function AppPoolStatus([Microsoft.SharePoint.Administration.SPFarm]$farm)   
{   
   Write-Host ""   
   write-host "Generating AppPool status report" -fore Magenta   
   $output = $scriptbase + "\" + "AppPoolStatus.csv""AppPoolName" + "," + "Status" + "," + "Server" | Out-File -Encoding Default -FilePath $Output;   
   foreach($server in $farm.Servers)   
   {   
      foreach($instance in $server.ServiceInstances)   
      {   
     # If the server has the timer service then stop the service   
		 if($instance.TypeName -eq $timerServiceInstanceName)   
		 {   
			[string]$serverName = $server.Name   
			write-host "Generating AppPool status report for server" $serverName -fore yellow   
				$status = ""   
			$AppPools = gwmi -namespace "root\webadministration" -Class applicationpool -ComputerName $serverName -Authentication PacketPrivacy -Impersonation Impersonate   
			foreach($AppPool in $AppPools )   
			{   
				if($AppPool.getstate().returnvalue -eq 1)   
			   {   
					$status = "Started"   
			   }   
			   else   
			   {   
					$status = "Stopped"   
			   }   
			   $AppPool.name + "," + $Status + "," + $serverName| Out-File -Encoding Default  -Append -FilePath $Output;   
			}   
		   write-host "AppPool status report generated" -fore green   
		 }   
      }   
  }   
   
}  




































