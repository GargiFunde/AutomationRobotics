<#   
.SYNOPSIS   
   
  Gets servers from Active Directory, check for ICA sessions, get last boottime and output to HTML with the additional option of email. 
   
.DESCRIPTION   
    
  This script gets a list of computers from AD, gets all ICA sessions and the status of various servcies from each server and then writes the output to HTML.  
   
.COMPATABILITY    
    
  Tested on PS v3/v4 and against servers from 2000 Servers to Server 2008 R2. It should work on 2012 but I haven't tested it. 
     
.EXAMPLE 
  PS C:\> GetCitrixSessions.ps1 
  All options are set as variables in the GLOBALS section so you simply run the script. 
 
.NOTES   
     
  This script requires the ActiveDirectory and PSTerminalServices modules. The AD module should just need to be imported but you will need to download and install the PSTS module from http://psterminalservices.codeplex.com 
 
  This script also requires the sortable.js script to make the table columns sortable. Download the script and place it in the same directory as $OutputFile. Get it here: http://www.kryogenix.org/code/browser/sorttable 
   
  The account running the script or scheduled task obviously must have the appropriate permissions on each server.  
   
  NAME:       GetCitrixSessions.ps1   
   
  AUTHOR:     Brian D. Arnold   
   
  CREATED:    06/23/2014  
   
  LASTEDIT:   07/01/2014  
#>   
 
################### 
#### FUNCTIONS #### 
################### 

<#

.SYNOPSIS

  This function will help you to test if a computer is pingable.

.DESCRIPTION

  This function will help you to test if a computer is pingable.

.PARAMETER  Computer

  The name or IP address of the computer

.EXAMPLE

  PS C:\> Get-PingStatus server01
  True

.EXAMPLE

  PS C:\> if (Get-PingStatus server01) { Write-Host "I'm up!" }

.EXAMPLE

  PS C:\> if (!(Get-PingStatus server01)) { Write-Host "I'm not up!" }

.OUTPUTS

  True or False.

 .NOTES

  NAME:       Get-PingStatus

  AUTHOR:     Fredrik Wall, fredrik@poweradmin.se

  BLOG:  poweradmin.se/blog

  TWITTER: walle75

  CREATED: 21/07/2009

  LASTEDIT:   30/05/13

     Changed Variable names and Parameter header.

#>
function Get-PingStatus
{
    [cmdletbinding()]
    param(
        [parameter(Mandatory=$true,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
        [Alias('Name','IP','IPAddress')]
       [string]$Computer
    )

    $PingStatus = Get-WmiObject -Query "SELECT StatusCode FROM win32_PingStatus WHERE ADDRESS = '$Computer'"

    if ($PingStatus.StatusCode -eq 0) {
        $true
    }
    else
    {
        $false
    }
} # END Get-PingStatus 


################### 
##### GLOBALS ##### 
################### 
 
# Change the keyword to an name that will match all Citrix servers
$Filter = 'Name -like "*XenApp*" -OR Name -like "*Citrix*" -AND Name -notlike "*TST*" -AND Name -notlike "*DEV*"'
# get all computers from Active Directory matching the defined keword
$ServerList = Get-ADComputer -Properties Name -Filter $Filter | Select Name | Sort Name
# Output file that will become your web site
$OutputFile = "E:\inetpub\wwwroot\citrixsessions.contoso.com\default.htm" 
 
# Get domain name, date and time for report title 
$DomainName = (Get-ADDomain).NetBIOSName  
$Time = Get-Date -Format t 
$CurrDate = Get-Date -UFormat "%D" 

# Option to create transcript - change to $true to turn on.
$CreateTranscript = $false

###############
##### PRE #####
###############

# Start Transcript if $CreateTranscript variable above is set to $true.
if($CreateTranscript)
{
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
if( -not (Test-Path ($scriptDir + "\Transcripts"))){New-Item -ItemType Directory -Path ($scriptDir + "\Transcripts")}
Start-Transcript -Path ($scriptDir + "\Transcripts\{0:yyyyMMdd}_Log.txt"  -f $(get-date)) -Append
}
 
# Import modules - AD and PSTS which contains Get-TSSession 
Import-Module ActiveDirectory 
Import-Module PSTerminalServices 

################ 
##### MAIN ##### 
################ 

# HTML table formatting
$HTML = '<style type="text/css"> 
#TSHead body {font: normal small sans-serif;}
#TSHead table {border-collapse: collapse; width: 100%; background-color:#F5F5F5;}
#TSHead th {font: normal small sans-serif;text-align:left;padding-top:5px;padding-bottom:4px;background-color:#7FB1B3;}
#TSHead th, td {font: normal small sans-serif; padding: 0.25rem;text-align: left;border: 1px solid #FFFFFF;}
#TSHead tbody tr:nth-child(odd) {background: #D3D3D3;}
    </Style>' 

# Report Header
$Header = "<H2 align=center><font face=Arial>$DomainName Citrix Sessions as of $Time on $CurrDate</font></H2>"  

$HTML += "<HTML><BODY><script src=sorttable.js></script><Table border=1 cellpadding=0 cellspacing=0 width=100% id=TSHead class=sortable>
        <TR> 
			<TH><B>Citrix Server</B></TH>
			<TH><B>Active ICA Sessions</B></TH>
			<TH><B>Disconnected Sessions</B></TH>
			<TH><B>Last Boot Time</B></TH>
			<TH><B>Citrix IMA</B></TH>
			<TH><B>Citrix XML</B></TH>
        </TR>
        " 

ForEach ($ServerName in $ServerList.Name) 
{
# Ping each server first
If (Get-PingStatus $ServerName)
    {
    Try{
    # Get TS Sessions
    $TSSessions = Get-TSSession -ComputerName $ServerName -ErrorAction SilentlyContinue | Select UserAccount,State,WindowStationName | WHERE {$_.UserAccount -ne $null -AND $_.WindowStationName -notlike "*RDP*" -AND $_.WindowStationName -notlike "*Services*" -AND $_.WindowStationName -notlike "*Console*"}
    # Get time of last reboot
    $BootTime = Get-WmiObject -ComputerName $ServerName win32_operatingsystem | select @{LABEL='LastBootUpTime';EXPRESSION={$_.ConverttoDateTime($_.LastBootUpTime)}}
    # Get status of services
    $IMAStatus = get-service -ComputerName $ServerName | WHERE {$_.Name -eq "IMAService"}
    $XMLStatus = get-service -ComputerName $ServerName | WHERE {$_.Name -eq "CtxHttp"}
    }
    Catch{
    }
    }
# Apply HTML to the results. For service status, turn cell red if service is stopped
                $HTML += "<TR>
					<TD>$($ServerName.ToString().ToUpper())</TD>
					<TD>$(($TSSessions.State | WHERE {$_ -eq "Active"}).count)</TD>
					<TD>$(($TSSessions.State | WHERE {$_ -like "*Disconnected"}).count)</TD>
                    <TD>$($BootTime.LastBootUpTime)</TD>
                    <TD bgcolor=`"$(if($IMAStatus.Status -eq "Stopped"){"F5A9A9"})`">$($IMAStatus.Status)</TD> 
                    <TD bgcolor=`"$(if($XMLStatus.Status -eq "Stopped"){"F5A9A9"})`">$($XMLStatus.Status)</TD>
				</TR>"
} 
 
$HTML += "<H2></Table></BODY></HTML>" 
$Header + $HTML | Out-File $OutputFile 
 