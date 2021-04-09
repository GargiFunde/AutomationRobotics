Function SharePointServices([Microsoft.SharePoint.Administration.SPFarm]$farm)   
{   
   Write-Host ""   
   write-host "Generating SharePoint services report" -fore Magenta    
  $output = $scriptbase + "\" + "SharePointServices.csv" "ServiceName" + "," + "ServiceStatus" + "," + "MachineName" | Out-File -Encoding Default -FilePath $Output;   
   foreach($server in $farm.Servers)   
   {   
     foreach($instance in $server.ServiceInstances)   
     {   
		# If the server has the timer service then stop the service   
		if($instance.TypeName -eq $timerServiceInstanceName)   
		{   
		   [string]$serverName = $server.Name   
		   write-host "Generating SP services report for server" $serverName -fore yellow   
		  $Monitor = "SPAdminV4" , "SPTimerV4" , "SPTraceV4" , "SPUserCodeV4" , "SPWriterV4" , "OSearch14" , "W3SVC" , "IISADMIN" , "C2WTS" , "FIMService" , "FIMSynchronizationService"   
		   $services = Get-Service -ComputerName $serverName -Name $Monitor -ea silentlycontinue   
		   foreach($service in $services)   
		   {   
			  $service.displayname + "," + $service.status + "," + $service.MachineName | Out-File -Encoding Default  -Append -FilePath $Output;   
		   }   
		   write-host "SP services report generated" -fore green   
		 }   
     }   
  }   
}  
