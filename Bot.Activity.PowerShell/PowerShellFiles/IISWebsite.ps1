Function IISWebsite([Microsoft.SharePoint.Administration.SPFarm]$farm)   
{   
   Write-Host ""   
   write-host "Generating IIS website report" -fore Magenta   
   $output = $scriptbase + "\" + "IISWebsite.csv"   
   "WebSiteName" + "," + "WebsiteID" + "," + "WebSiteState" + "," + "Server" | Out-File -Encoding Default -FilePath $Output;   
   foreach($server in $farm.Servers)   
   {   
      foreach($instance in $server.ServiceInstances)   
      {   
     # If the server has the timer service then stop the service   
		 if($instance.TypeName -eq $timerServiceInstanceName)   
		 {   
			 [string]$serverName = $server.Name   
			 write-host "Generating IIS website report for server" $serverName -fore yellow   
				 $status = ""   
			 $Sites = gwmi -namespace "root\webadministration" -Class site -ComputerName $serverName -Authentication PacketPrivacy -Impersonation Impersonate   
			 foreach($site in $sites)   
			 {   
			if($site.getstate().returnvalue -eq 1)   
			{   
			   $status = "Started"   
			}   
			else   
			{   
			   $status = "Not Started"   
			}   
			$site.name + "," + $site.ID + "," + $Status + "," + $serverName | Out-File -Encoding Default  -Append -FilePath $Output;   
			}   
			write-host "IIS website report generated" -fore green   
		}   
      }   
   }   
}  


