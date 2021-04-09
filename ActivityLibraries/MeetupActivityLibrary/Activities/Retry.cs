using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Statements;
using System.Activities.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Markup;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/Retry.png")]
    [Designer(typeof(Retry_ActivityDesigner))]
    [ContentProperty("Body")]
    public sealed class Retry : BaseNativeActivity, IActivityTemplateFactory
    {
        private static readonly TimeSpan _defaultRetryInterval = new TimeSpan(0, 0, 0, 1);

        private readonly Variable<Int32> _attemptCount = new Variable<Int32>();

        private readonly Variable<TimeSpan> _delayDuration = new Variable<TimeSpan>();

        private readonly Delay _internalDelay;

        public Retry()
        {
            _internalDelay = new Delay
            {
                Duration = new InArgument<TimeSpan>(_delayDuration)
            };
            Body = new ActivityAction();
            MaxAttempts = 5;
            ExceptionType = typeof(TimeoutException);
            RetryInterval = _defaultRetryInterval;
        }

        [DebuggerNonUserCode]
        public System.Activities.Activity Create(DependencyObject target)
        {
            return new Retry
            {
                Body =
                            {
                                Handler = new Sequence()
                            }
            };
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddDelegate(Body);
            metadata.AddImplementationChild(_internalDelay);
            metadata.AddImplementationVariable(_attemptCount);
            metadata.AddImplementationVariable(_delayDuration);

            RuntimeArgument maxAttemptsArgument = new RuntimeArgument("MaxAttempts", typeof(Int32), ArgumentDirection.In, true);
            RuntimeArgument retryIntervalArgument = new RuntimeArgument("RetryInterval", typeof(TimeSpan), ArgumentDirection.In, true);

            metadata.Bind(MaxAttempts, maxAttemptsArgument);
            metadata.Bind(RetryInterval, retryIntervalArgument);

            Collection<RuntimeArgument> arguments = new Collection<RuntimeArgument>
                                                    {
                                                        maxAttemptsArgument,
                                                        retryIntervalArgument
                                                    };

            metadata.SetArgumentsCollection(arguments);

            if (Body == null)
            {
                ValidationError validationError = new ValidationError("No Child Activities Defined", true, "Body");

                metadata.AddValidationError(validationError);
            }

            if (typeof(Exception).IsAssignableFrom(ExceptionType) == false)
            {
                ValidationError validationError = new ValidationError("Invalid Exception Type", false, "ExceptionType");

                metadata.AddValidationError(validationError);
            }
        }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                ExecuteAttempt(context);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Retry", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

        private static Boolean ShouldRetryAction(Type exceptionType, Exception thrownException)
        {
            if (exceptionType == null)
            {
                return false;
            }

            return exceptionType.IsAssignableFrom(thrownException.GetType());
        }

        private void ActionFailed(NativeActivityFaultContext faultcontext, Exception propagatedexception, ActivityInstance propagatedfrom)
        {
            Int32 currentAttemptCount = _attemptCount.Get(faultcontext);

            currentAttemptCount++;

            _attemptCount.Set(faultcontext, currentAttemptCount);

            Int32 maxAttempts = MaxAttempts.Get(faultcontext);

            if (currentAttemptCount >= maxAttempts)
            {
                // There are no further attempts to make
                return;
            }

            if (ShouldRetryAction(ExceptionType, propagatedexception) == false)
            {
                return;
            }

            faultcontext.CancelChild(propagatedfrom);
            faultcontext.HandleFault();

            TimeSpan retryInterval = RetryInterval.Get(faultcontext);

            if (retryInterval == TimeSpan.Zero)
            {
                ExecuteAttempt(faultcontext);
            }
            else
            {
                // We are going to wait before trying again
                _delayDuration.Set(faultcontext, retryInterval);

                faultcontext.ScheduleActivity(_internalDelay, DelayCompleted);
            }
        }

        private void DelayCompleted(NativeActivityContext context, ActivityInstance completedinstance)
        {
            ExecuteAttempt(context);
        }

        private void ExecuteAttempt(NativeActivityContext context)
        {
            if (Body == null)
            {
                return;
            }

            context.ScheduleAction(Body, null, ActionFailed);
        }

        [Browsable(false)]
        public ActivityAction Body
        {
            get;
            set;
        }

        [DefaultValue(typeof(TimeoutException))]
        public Type ExceptionType
        {
            get;
            set;
        }

        [Category("Input")]
        [DisplayName("Max Attempts")]
        [Description("Enter Max Attempts")]
        public InArgument<Int32> MaxAttempts
        {
            get;
            set;
        }

        [Category("Input")]
        [DisplayName("RetryInterval")]
        [Description("Enter Retry Interval")]
        public InArgument<TimeSpan> RetryInterval
        {
            get;
            set;
        }
    }
}



//One of the tools missing out of the WF toolbox is the ability to run some retry logic.Applications often have known scenarios where something can go wrong such that a retry of the last action could get a successful outcome.One such example is a connection timeout to a database.You may want to try a couple of times before throwing the exception in order to get more success over time.

//The specific scenario I am addressing is a little different.I have created some custom MSF providers that will allow me to run a MSF sync session behind a WCF service.The main issue with this design is that it is possible that multiple clients may want to sync the same data set at the same time. MSF stores the metadata (Replica) that describes the items to sync in a SQLCE file.It will throw an exception if the replica is already open and in use.This doesn’t work well in a services environment.The way I will attempt to manage this is to limit the amount of time the replica is in use and then implement some retry logic around opening the replica in order to run a sync.

//The Retry activity shown below will execute its child activity and watch for a match on a nominated exception type or derivative.It will then determine whether it can make another attempt at the child activity by comparing the current number of attempts against a MaxAttempts property.If another attempt will be made then it will determine whether the child activity will be invoked immediately or whether a delay should occur first.

//    The CacheMetadata method has some logic in it to validate the state of the activity. This validation will display a warning if no child activity has been defined. It will also raise an error if the ExceptionType property has not been assigned a type that derives from System.Exception.The activity implements IActivityTemplateFactory and uses the Create method to include a child Sequence activity when Retry is added to an activity on the designer.