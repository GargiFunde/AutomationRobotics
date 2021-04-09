using System.Diagnostics;
using System.IO;

namespace RobotLibrary
{
    public class CheckRobotState
    {

        public static bool CheckIfRobotIsRunning(int robotProcessId, string robotInstallationPath)
        {
            if(robotProcessId!= 0)
            {
                foreach (Process process in Process.GetProcesses())
                {
                    if(process.Id==robotProcessId)
                    {
                        var seProcess = Process.GetProcessById(robotProcessId);
                        if(seProcess.MainModule.FileName.ToLower().Contains((new FileInfo(robotInstallationPath)).FullName.ToLower()))
                        {
                            return true;
                        }
                        break;
                    }
                }
            }
            return false;
        }
    }
}
