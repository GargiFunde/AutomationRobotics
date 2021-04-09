using System.Windows.Input;

namespace BOTDesigner.Helpers
{
    /// <summary>
    /// Custom Commands     
    /// </summary>
    public static class CustomCommands
    {
        public static ICommand CmdWfNewCSharp = new RoutedCommand("CmdWfNew", typeof(CustomCommands));
        public static ICommand CmdWfNewVB = new RoutedCommand("CmdWfNewVB", typeof(CustomCommands));
        public static ICommand CmdWfNew = new RoutedCommand("CmdWfNew", typeof(CustomCommands));
        public static ICommand CmdWfRun = new RoutedCommand("CmdWfRun", typeof(CustomCommands));
        public static ICommand CmdWfStop = new RoutedCommand("CmdWfStop", typeof(CustomCommands));
        public static ICommand CmdWfDebug = new RoutedCommand("CmdWfDebug", typeof(CustomCommands));
        public static ICommand CmdWfShowLog = new RoutedCommand("CmdWfShowLog", typeof(CustomCommands));
        public static ICommand CmdWfClearLog = new RoutedCommand("CmdWfClearLog", typeof(CustomCommands));
        public static ICommand CmdWfSaveAll = new RoutedCommand("CmdWfSaveAll", typeof(CustomCommands));
        public static ICommand CmdWfMax = new RoutedCommand("CmdWfMax", typeof(CustomCommands));
        public static ICommand CmdWfMin = new RoutedCommand("CmdWfMin", typeof(CustomCommands));
        public static ICommand CmdWfPublish = new RoutedCommand("CmdWfPublish", typeof(CustomCommands));
        public static ICommand CmdWfImport = new RoutedCommand("CmdWfImport", typeof(CustomCommands));
        public static ICommand CmdWfExport = new RoutedCommand("CmdWfExport", typeof(CustomCommands));
        public static ICommand CmdWfSettings = new RoutedCommand("CmdWfSettings", typeof(CustomCommands));
        public static ICommand CmdWfNewFlowChart = new RoutedCommand("CmdWfNewFlowChart", typeof(CustomCommands));
        public static ICommand CmdWfNewStateMachine = new RoutedCommand("CmdWfNewStateMachine", typeof(CustomCommands));
        public static ICommand CmdWfResumeBreakPoint = new RoutedCommand("CmdWfResumeBreakPoint", typeof(CustomCommands));
        public static ICommand CmdLogout = new RoutedCommand("CmdLogout", typeof(CustomCommands));
        public static ICommand CmdWfXPathWeb = new RoutedCommand("CmdWfXPathWeb", typeof(CustomCommands));
		public static ICommand CmdWfXPathWin = new RoutedCommand("CmdWfXPathWin", typeof(CustomCommands));
        //Debug
        public static ICommand CmdWfStepInto = new RoutedCommand("CmdWfStepInto", typeof(CustomCommands));
        public static ICommand CmdWfStepOver = new RoutedCommand("CmdWfStepOver", typeof(CustomCommands));

        //Invoke XAML Workflow
        public static ICommand CmdLoadInvokedWf = new RoutedCommand("CmdLoadInvokedWf", typeof(CustomCommands));

    }
}
