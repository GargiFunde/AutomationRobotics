using CommonLibrary;
using Logger;
using System;
using System.Activities;
using System.Activities.Debugger;
using System.Activities.Presentation;
using System.Activities.Presentation.Debug;
using System.Activities.Presentation.Services;
using System.Activities.Tracking;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using BOTDesigner.Views;

namespace BOTDesigner
{
    public class WorkflowEntity
    {
        int i = 0;
        
        public WorkflowApplication _wfApp { get; set; }
        public IDesignerDebugView DebuggerService { get; set; }
        public string _currentWorkflowFile { get; set; }
        public DataGrid dgInfoErrorWarnings { get; set; }
        public System.Windows.Threading.Dispatcher dispatcher { get; set; }

        Dictionary<int, SourceLocation> textLineToSourceLocationMap;
        public Dictionary<object, SourceLocation> designerSourceLocationMapping = new Dictionary<object, SourceLocation>();

        public List<SourceLocation> breakpointList = new List<SourceLocation>();
        public AutoResetEvent resumeRuntimeFromHost = null;
        private Helpers.CustomTrackingParticipant _executionLog;
        DynamicActivity activityExecute = null;
        Dictionary<object, SourceLocation> wfElementToSourceLocationMap = null;
        Dictionary<string, Activity> activityIdToWfElementMap = null;
        public TextBox consoleExecutionLog { get; set; }
        public string DebugType {get;set;}
        public int ChildCount { get; set; }
        public bool isParent { get; set; }
        public WorkflowEntity()
        {
            MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(Helpers.CustomWfDesigner.Instance.Text));
            activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;
            _wfApp = new WorkflowApplication(activityExecute);
            wfElementToSourceLocationMap = UpdateSourceLocationMappingInDebuggerService(activityExecute);


        }

        public void InitStartDebuggingForWorkflow()
        {
            MemoryStream workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(Helpers.CustomWfDesigner.Instance.Text));
            activityExecute = ActivityXamlServices.Load(workflowStream) as DynamicActivity;
            _wfApp = new WorkflowApplication(activityExecute);
            string DebugStyle = "continue";
            DebugType = DebugStyle;
        }

        public void StartDebuggingForWorkflow()
        {

            resumeRuntimeFromHost = new AutoResetEvent(false);
            textLineToSourceLocationMap = new Dictionary<int, SourceLocation>();
            //Mapping between the Object and Line No.
            //Mapping between the Object and the Instance Id
            _executionLog = new Helpers.CustomTrackingParticipant();
            _wfApp.Extensions.Add(_executionLog);
            wfElementToSourceLocationMap = UpdateSourceLocationMappingInDebuggerService(activityExecute);
            activityIdToWfElementMap = BuildActivityIdToWfElementMap(wfElementToSourceLocationMap);

            #region Set up Custom Tracking
            const String all = "*";
            VisualTrackingParticipant simTracker = new VisualTrackingParticipant()
            {
                TrackingProfile = new TrackingProfile()
                {
                    Name = "CustomTrackingProfile",
                    Queries =
                        {
                            new CustomTrackingQuery()
                            {
                                Name = all,
                                ActivityName = all
                            },
                            new WorkflowInstanceQuery()
                            {
                                // Limit workflow instance tracking records for started and completed workflow states
                                States = { WorkflowInstanceStates.Started, WorkflowInstanceStates.Completed },
                            },
                             new ActivityScheduledQuery()
                            {
                                // Subscribe for track records from all activities for all states
                                ActivityName = all,
                               

                                // Extract workflow variables and arguments as a part of the activity tracking record
                                // VariableName = "*" allows for extraction of all variables in the scope
                                // of the activity
                               
                            },
                            new ActivityStateQuery()
                            {
                                // Subscribe for track records from all activities for all states
                                ActivityName = all,
                                States = { all },

                                // Extract workflow variables and arguments as a part of the activity tracking record
                                // VariableName = "*" allows for extraction of all variables in the scope
                                // of the activity
                                Variables =
                                {
                                    { all }
                                }
                            }
                        }
                }
            };
            simTracker.ActivityIdToWorkflowElementMap = activityIdToWfElementMap;
            #endregion


            //As the tracking events are received
            simTracker.TrackingRecordReceived += (trackingParticpant, trackingEventArgs) =>
            {
                if (trackingEventArgs.Activity != null)
                {
                    System.Diagnostics.Debug.WriteLine(
                        String.Format("<+=+=+=+> Activity Tracking Record Received for ActivityId: {0}, record: {1} ",
                        trackingEventArgs.Activity.Id,
                        trackingEventArgs.Record
                        )
                    );

                    //&& ChildCount==0
                    //****************************************STEP OVER SERVICE******************************************************************
                    if (!(trackingEventArgs.Record.GetType().ToString().Equals("System.Activities.Tracking.ActivityScheduledRecord")))
                    { 
                        if (DebugType.Equals("stepover")  && (((ActivityStateRecord)trackingEventArgs.Record).State).Equals("Executing"))
                    {
                        IEnumerable<Activity> children = null;
                        children = System.Activities.WorkflowInspectionServices.GetActivities(trackingEventArgs.Activity);
                        int count = 0;
                        foreach (Activity child in children)
                        {
                            //  children = children.Concat();
                            //  ActivityScheduledRecord abc = trackingEventArgs.Record as ActivityScheduledRecord;
                            if (!(child.DisplayName.Contains("Literal")) && !(child.DisplayName.Contains("VisualBasicValue")))
                            {
                                    //foreach(var abc in child)
                                count++;
                            }
                           
                        }
                        // children.OfType<Activity>();
                        ChildCount = ChildCount + count;
                    }
                    }
                    //****************************************STEP OVER SERVICE******************************************************************






                    if ((trackingEventArgs.Record.GetType().ToString().Equals("System.Activities.Tracking.ActivityScheduledRecord")))
                    {
                       


                        ShowDebug(wfElementToSourceLocationMap[trackingEventArgs.Activity]);
                    }
                    
                    dispatcher.Invoke(DispatcherPriority.SystemIdle, (Action)(() =>
                    {
                         if(!(trackingEventArgs.Record.GetType().ToString().Equals("System.Activities.Tracking.ActivityScheduledRecord")))
                          { 
                        //Textbox Updates
                        consoleExecutionLog.AppendText(trackingEventArgs.Activity.DisplayName + " " + ((ActivityStateRecord)trackingEventArgs.Record).State + "\n");
                        consoleExecutionLog.AppendText("******************\n");
                        textLineToSourceLocationMap.Add(i, wfElementToSourceLocationMap[trackingEventArgs.Activity]);
                        i = i + 2;

                        dgInfoErrorWarnings.DataContext = Logger.Log.Logger.DatatableLog;
                        dgInfoErrorWarnings.ItemsSource = null;
                        dgInfoErrorWarnings.ItemsSource = Logger.Log.Logger.DatatableLog.DefaultView;
                        }
                        //Add a sleep so that the debug adornments are visible to the user
                        System.Threading.Thread.Sleep(1000);
                    }));

                }
            };

            _wfApp.Extensions.Add(simTracker);
            ThreadPool.QueueUserWorkItem(new WaitCallback((context) =>
            {
                try
                {
                    //Invoking the Workflow Instance with Input Arguments
                    _wfApp.Run();
                }
                catch (Exception ex)
                {
                    Log.Logger.LogData(ex.Message, LogLevel.Error);
                }
                //This is to remove the final debug adornment
               dispatcher.Invoke(DispatcherPriority.Render
                    , (Action)(() =>
                    {
                        SelectHelper._wfDesigner.DebugManagerView.CurrentLocation = new SourceLocation(_currentWorkflowFile, 1, 1, 1, 10);
                    }));

            }));
        }

        void ShowDebug(SourceLocation srcLoc)
        {
            try
            {
                dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
                {
                    SelectHelper._wfDesigner.DebugManagerView.CurrentLocation = srcLoc;

                }));
                //Check if this is where any BP is set
                bool isBreakpointHit = false;


                foreach (SourceLocation src in breakpointList)
                {
                    if (src.StartLine == srcLoc.StartLine && src.EndLine == srcLoc.EndLine)
                    {
                        isBreakpointHit = true;
                    }
                }

                //**************************STEP OVER************************************************************
                if (DebugType.Equals("stepover") && ChildCount==0)
                {
                    isBreakpointHit = true;
                  
                }
                //**************************STEP OVER************************************************************

                if (isBreakpointHit == true)
                {
                    resumeRuntimeFromHost.WaitOne();
                }
                else if(DebugType.Equals("stepinto"))
                {
                    resumeRuntimeFromHost.WaitOne();
                }
                //**************************STEP OVER************************************************************
                else if (DebugType.Equals("stepover"))
                {
                    ChildCount--;
                  
                   // resumeRuntimeFromHost.WaitOne();
                   
                }
                //**************************STEP OVER************************************************************
                else if (DebugType.Equals("continue"))
                {

                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        private Dictionary<string, Activity> BuildActivityIdToWfElementMap(Dictionary<object, SourceLocation> wfElementToSourceLocationMap)
        {
            Dictionary<string, Activity> map = new Dictionary<string, Activity>();
            try
            {
                Activity wfElement;
                foreach (object instance in wfElementToSourceLocationMap.Keys)
                {
                    wfElement = instance as Activity;
                    if (wfElement != null)
                    {
                        map.Add(wfElement.Id, wfElement);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return map;
        }

        Dictionary<object, SourceLocation> UpdateSourceLocationMappingInDebuggerService(Activity activityExecute)
        {
            object rootInstance = GetRootInstance();
            Dictionary<object, SourceLocation> sourceLocationMapping = new Dictionary<object, SourceLocation>();
           // Dictionary<object, SourceLocation> designerSourceLocationMapping = new Dictionary<object, SourceLocation>();
            designerSourceLocationMapping = new Dictionary<object, SourceLocation>();
            try
            {
                if (rootInstance != null)
                {
                    Activity documentRootElement = GetRootWorkflowElement(rootInstance);
                    SourceLocationProvider.CollectMapping(GetRootRuntimeWorkflowElement(activityExecute), documentRootElement, sourceLocationMapping,
                    SelectHelper._wfDesigner.Context.Items.GetValue<WorkflowFileItem>().LoadedFile);
                    try
                    {
                        SourceLocationProvider.CollectMapping(documentRootElement, documentRootElement, designerSourceLocationMapping,
                    SelectHelper._wfDesigner.Context.Items.GetValue<WorkflowFileItem>().LoadedFile);



                    }
                    catch (Exception)
                    {


                    }

                }

                // Notify the DebuggerService of the new sourceLocationMapping.
                // When rootInstance == null, it'll just reset the mapping.
                //DebuggerService debuggerService = debuggerService as DebuggerService;
                if(DebuggerService == null)
                {
                    DebuggerService = SelectHelper._wfDesigner.DebugManagerView;
                }
                if (DebuggerService != null)
                {
                     ((DebuggerService)DebuggerService).UpdateSourceLocations(designerSourceLocationMapping);
                   
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }

             //  return sourceLocationMapping;
            return designerSourceLocationMapping;
        }


        object GetRootInstance()
        {
            try
            {
                ModelService modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
                if (modelService != null)
                {
                    return modelService.Root.GetCurrentValue();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return null;
        }

        // Get root WorkflowElement.  Currently only handle when the object is ActivitySchemaType or WorkflowElement.
        // May return null if it does not know how to get the root activity.
        Activity GetRootWorkflowElement(object rootModelObject)
        {
            Activity rootWorkflowElement = null;
            try
            {
                System.Diagnostics.Debug.Assert(rootModelObject != null, "Cannot pass null as rootModelObject");
                IDebuggableWorkflowTree debuggableWorkflowTree = rootModelObject as IDebuggableWorkflowTree;
                if (debuggableWorkflowTree != null)
                {
                    rootWorkflowElement = debuggableWorkflowTree.GetWorkflowRoot();
                }
                else // Loose xaml case.
                {
                    rootWorkflowElement = rootModelObject as Activity;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }

            return rootWorkflowElement;
        }

        Activity GetRuntimeExecutionRoot(Activity root)
        {
            try
            {
                // Activity root = ActivityXamlServices.Load(_currentWorkflowFile);
                WorkflowInspectionServices.CacheMetadata(root);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return root;
        }
    
        Activity GetRootRuntimeWorkflowElement(Activity root)
        {
          
            try
            {
                //Activity root = ActivityXamlServices.Load(_currentWorkflowFile);
                WorkflowInspectionServices.CacheMetadata(root);

                IEnumerator<Activity> enumerator1 = WorkflowInspectionServices.GetActivities(root).GetEnumerator();
                //Get the first child of the x:class
                enumerator1.MoveNext();
                root = enumerator1.Current;
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return root;
        }

    }
}
