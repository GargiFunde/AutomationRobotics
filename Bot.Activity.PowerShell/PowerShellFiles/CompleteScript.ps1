
#	Introduction
#
#	From a SharePoint administration perspective monitoring tasks are very important for maintaining a healthy Farm. Monitoring should be done at specific time intervals. You can then differentiate monitoring into the 3 categories daily, weekly and monthly. This article outlines how to automate some of the tasks for daily monitoring.
#
#	What the script does
#
#	The daily monitoring script automates the following tasks and captures the information into CSV files:
#	•SharePoint services status 
#	•IISWebsite status 
#	•AppPool status 
#	•Disk space info 
#	•Health analyser report 
#	•CPU utilization 
#	•Memory utilization 
#	•SharePoint server status 
#
#	Manually doing these tasks consumes time and human error can occur. So the PowerShell script would become handy in this scenario.
#
#	There is one more function that is included with this script. This is the most interesting part. The script generates output in CSV files for each of the preceding tasks. There is a separate function that converts all the CSV files into an Excel sheet with tabs separated for each of the CSV file. This is done to consolidate all the reports into a single file and provide it to the customer or upper management for better readability. 
#
#	Note: This functionality of consolidating files into Excel only works if you have an Excel client installed.
 

$LogTime = Get-Date -Format yyyy-MM-dd_hh-mm   
$LogFile = ".\DailyMonitoringPatch-$LogTime.rtf"   
  
# Add SharePoint PowerShell Snapin   
 
   
if ( (Get-PSSnapin -Name Microsoft.SharePoint.PowerShell -ErrorAction SilentlyContinue) -eq $null )  
{   
    Add-PSSnapin Microsoft.SharePoint.Powershell   
}   
import-module WebAdministration   
   
$scriptBase = split-path $SCRIPT:MyInvocation.MyCommand.Path -parent   
Set-Location $scriptBase   
   
write-host "TESTING FOR LOG FOLDER EXISTENCE" -fore yellow   
$TestLogFolder = test-path -path $scriptbase\Logs   
if($TestLogFolder)   
{   
    write-host "The log folder already exist in the script location" -fore yellow   
    $clearlogfolder = read-host "Do you want to clear the log folder (y/n)"   
    if($clearlogfolder -eq 'y')   
    {   
        write-host "The user choosen to clear the log folder" -fore yellow   
        write-host "Clearing the log folder" -fore yellow   
        remove-item $scriptbase\Logs\* -recurse -confirm:$false   
        write-host "Log folder cleared" -fore yellow   
    }   
    else   
    {   
        write-host "The user choosen not to clear the log files" -fore yellow   
    }   
}   
else   
{   
    write-host "Log folder does not exist" -fore yellow   
    write-host "Creating a log folder" -fore yellow   
    New-Item $Scriptbase\Logs -type directory   
    write-host "Log folder created" -fore yellow   
}          
  
#moving any .rtf files in the scriptbase location   
$FindRTFFile = Get-ChildItem $scriptBase\*.* -include *.rtf   
if($FindRTFFile)   
{   
    write-host "Some old log files are found in the script location" -fore yellow   
    write-host "Moving old log files into the Logs folder" -fore yellow   
    foreach($file in $FindRTFFile)   
        {   
            move-item -path $file -destination $scriptbase\logs   
        }   
    write-host "Old log files moved successfully" -fore yellow   
}   
   
start-transcript $logfile   
   
$global:timerServiceName = "SharePoint 2010 Timer"   
$global:timerServiceInstanceName = "Microsoft SharePoint Foundation Timer"   
  
# Get the local farm instance   
[Microsoft.SharePoint.Administration.SPFarm]$farm = [Microsoft.SharePoint.Administration.SPFarm]::get_Local()   
   
Function SharePointServices([Microsoft.SharePoint.Administration.SPFarm]$farm)   
{   
   
    Write-Host ""   
    write-host "Generating SharePoint services report" -fore Magenta   
       
    $output = $scriptbase + "\" + "SharePointServices.csv"   
    "ServiceName" + "," + "ServiceStatus" + "," + "MachineName" | Out-File -Encoding Default -FilePath $Output;   
   
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
   
   
Function AppPoolStatus([Microsoft.SharePoint.Administration.SPFarm]$farm)   
{   
   
    Write-Host ""   
    write-host "Generating AppPool status report" -fore Magenta   
       
    $output = $scriptbase + "\" + "AppPoolStatus.csv"   
    "AppPoolName" + "," + "Status" + "," + "Server" | Out-File -Encoding Default -FilePath $Output;   
   
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
   
   
Function HealthAnalyserReports()   
{   
       
    write-host ""   
    write-host "Generating health analyser report" -fore magenta   
   
    $output = $scriptbase + "\" + "HealthAnalyser.csv"     
    "Severity" + "," + "Category" + "," + "Modified" + "," + "Failing servers" + "," + "Failing services"  | Out-File -Encoding Default -FilePath $Output;   
   
    $ReportsList = [Microsoft.SharePoint.Administration.Health.SPHealthReportsList]::Local   
    $Items = $ReportsList.items | where {      
   
        if($_['Severity'] -eq '1 - Error')   
        {   
            #write-host $_['Name']   
            #write-host $_['Severity']   
            $_['Severity'] + "," + $_['Category'] + "," + $_['Modified'] + "," + $_['Failing Servers'] + "," + $_['Failing Services']  | Out-File -Encoding Default  -Append -FilePath $Output;   
        }   
    }   
    write-host "Health analyser report generated" -fore green   
}   
   
   
   
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
#######################Function to combine multiple CSV files into single excel sheet with seperated tabs for each CSV#########################   
   
Function Release-Ref ($ref)    
{   
    ([System.Runtime.InteropServices.Marshal]::ReleaseComObject(   
    [System.__ComObject]$ref) -gt 0)   
    [System.GC]::Collect()   
    [System.GC]::WaitForPendingFinalizers()    
}   
   
Function ConvertCSV-ToExcel   
{   
   
[CmdletBinding(   
    SupportsShouldProcess = $True,   
    ConfirmImpact = 'low',   
    DefaultParameterSetName = 'file'   
    )]   
Param (       
    [Parameter(   
     ValueFromPipeline=$True,   
     Position=0,   
     Mandatory=$True,   
     HelpMessage="Name of CSV/s to import")]   
     [ValidateNotNullOrEmpty()]   
    [array]$inputfile,   
    [Parameter(   
     ValueFromPipeline=$False,   
     Position=1,   
     Mandatory=$True,   
     HelpMessage="Name of excel file output")]   
     [ValidateNotNullOrEmpty()]   
    [string]$output       
    )   
   
Begin {        
    #Configure regular expression to match full path of each file   
    [regex]$regex = "^\w\:\\"   
      
    #Find the number of CSVs being imported   
    $count = ($inputfile.count -1)   
     
    #Create Excel Com Object   
    $excel = new-object -com excel.application   
      
    #Disable alerts   
    $excel.DisplayAlerts = $False   
  
    #Show Excel application   
    $excel.Visible = $False   
  
    #Add workbook   
    $workbook = $excel.workbooks.Add()   
  
    #Remove other worksheets   
    $workbook.worksheets.Item(2).delete()   
    #After the first worksheet is removed,the next one takes its place   
    $workbook.worksheets.Item(2).delete()      
  
    #Define initial worksheet number   
    $i = 1   
    }   
   
Process {   
    ForEach ($input in $inputfile) {   
        #If more than one file, create another worksheet for each file   
        If ($i -gt 1) {   
            $workbook.worksheets.Add() | Out-Null   
            }   
        #Use the first worksheet in the workbook (also the newest created worksheet is always 1)   
        $worksheet = $workbook.worksheets.Item(1)   
        #Add name of CSV as worksheet name   
        $worksheet.name = "$((GCI $input).basename)"   
  
        #Open the CSV file in Excel, must be converted into complete path if no already done   
        If ($regex.ismatch($input)) {   
            $tempcsv = $excel.Workbooks.Open($input)    
            }   
        ElseIf ($regex.ismatch("$($input.fullname)")) {   
            $tempcsv = $excel.Workbooks.Open("$($input.fullname)")    
            }       
        Else {       
            $tempcsv = $excel.Workbooks.Open("$($pwd)\$input")         
            }   
        $tempsheet = $tempcsv.Worksheets.Item(1)   
        #Copy contents of the CSV file   
        $tempSheet.UsedRange.Copy() | Out-Null   
        #Paste contents of CSV into existing workbook   
        $worksheet.Paste()   
  
        #Close temp workbook   
        $tempcsv.close()   
  
        #Select all used cells   
        $range = $worksheet.UsedRange   
  
        #Autofit the columns   
        $range.EntireColumn.Autofit() | out-null   
        $i++   
        }    
    }           
   
End {   
    #Save spreadsheet   
    $workbook.saveas("$pwd\$output")   
   
    Write-Host -Fore Green "File saved to $pwd\$output"   
  
    #Close Excel   
    $excel.quit()     
  
    #Release processes for Excel   
    $a = Release-Ref($range)   
    }   
}          
  
#################################################################################################################################################   
  
  
##########Calling Functions#################   
SharePointServices $farm   
IISWebsite $farm   
AppPoolStatus $farm   
DiskSpace $farm   
HealthAnalyserReports   
CPUUtilization $farm   
MemoryUtilization $farm   
SPServerStatus $farm   
   
write-host ""   
write-host "Combining all CSV files into single file" -fore yellow   
Get-Item $scriptbase\*.csv | ConvertCSV-ToExcel -output "DailyMonitoringReports.xlsx"   
   
   
write-host ""   
write-host "SCRIPT COMPLETED" -fore green   
   
stop-transcript  






































































































