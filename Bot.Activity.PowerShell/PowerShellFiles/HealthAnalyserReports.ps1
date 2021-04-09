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
