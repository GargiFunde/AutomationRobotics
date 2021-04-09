using CommonLibrary;
using Logger;
using System;
using System.Activities;
using System.Activities.Presentation.Metadata;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;


namespace Bot.Activity.Intelligent
{
    [Designer(typeof(Intelligent_SpeechToText_ActivityDesigner))]
    [ToolboxBitmap("Resources/IntelligentSpeechToText.png")]

    public class Intelligent_SpeechToText : BaseNativeActivity
    {
            [RequiredArgument]
            public InArgument<string> InputWavFileFullPath { get; set; }
            public InArgument<string> OutputTextFileFullPath { get; set; }
        //  public OutArgument<string> Result { get; set; }

      //  SpeechSynthesizer synth;
       // SpeechRecognitionEngine recognizer;
        bool completed;
        string outputfilePath = string.Empty;
        protected override void Execute(NativeActivityContext context)
        {
            
            try
            {
                string filePath = InputWavFileFullPath.Get(context);
                outputfilePath = OutputTextFileFullPath.Get(context);
                string inputText = File.ReadAllText(filePath);

                using (SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine())
                {

                    Grammar dictation = new DictationGrammar();
                    dictation.Name = "Dictation Grammar";

                    recognizer.LoadGrammar(dictation);

                    // Configure the input to the recognizer.  
                    recognizer.SetInputToWaveFile(filePath);

                    // Attach event handlers for the results of recognition.  
                    recognizer.SpeechRecognized +=
                      new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
                    recognizer.RecognizeCompleted +=
                      new EventHandler<RecognizeCompletedEventArgs>(recognizer_RecognizeCompleted);

                    // Perform recognition on the entire file.  

                    completed = false;
                    recognizer.RecognizeAsync();

                    // Keep the console window open.  
                    while (!completed)
                    {
                        //continue till complete
                    }
                    Log.Logger.LogData("done", LogLevel.Info);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Cognitive: Microsoft Cognitive API", LogLevel.Error);
                context.Abort();
            }
        }
        // Handle the SpeechRecognized event.  
        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null && e.Result.Text != null)
            {
                if(!string.IsNullOrEmpty(outputfilePath))
                {
                    File.WriteAllText(outputfilePath, e.Result.Text);
                }
                
            }
            else
            {
                Log.Logger.LogData("Text recognition failed, recognized text not available.", LogLevel.Error);
            }
        }

        // Handle the RecognizeCompleted event.  
        void recognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Log.Logger.LogData("  Error encountered,"+ e.Error.GetType().Name + e.Error.Message, LogLevel.Error);
            }
            if (e.Cancelled)
            {
                Log.Logger.LogData("  Operation cancelled.", LogLevel.Warning);
            }
            if (e.InputStreamEnded)
            {
                Log.Logger.LogData("  End of stream encountered.",LogLevel.Info);
            }
            completed = true;
        }

    }

}


