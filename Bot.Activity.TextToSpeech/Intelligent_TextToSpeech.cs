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
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;


namespace Bot.Activity.Intelligent
{
    [Designer(typeof(Intelligent_TextToSpeech_ActivityDesigner))]
    [ToolboxBitmap("Resources/IntelligentTextToSpeech.png")]

    public class Intelligent_TextToSpeech : BaseNativeActivity
    {
            [RequiredArgument]
            public InArgument<string> InputTextFileFullPath { get; set; }
            public InArgument<string> OutputWavFileFullPath { get; set; }
        //  public OutArgument<string> Result { get; set; }

        SpeechSynthesizer synth;

        protected override void Execute(NativeActivityContext context)
        {
            
            try
            {
                string filePath = InputTextFileFullPath.Get(context);
                string outputfilePath = OutputWavFileFullPath.Get(context);
                string inputText = File.ReadAllText(filePath);

                if (!string.IsNullOrEmpty (inputText))
                {

                    synth = new SpeechSynthesizer();   //Using statement dont work for this   

                    if (!string.IsNullOrEmpty(outputfilePath))
                    {
                        // Configure the audio output.   
                        synth.SetOutputToWaveFile(outputfilePath,
                        new SpeechAudioFormatInfo(32000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
                        Log.Logger.LogData("output will be either .wav file or audio", LogLevel.Info);
                    }
                    else
                    {
                        Log.Logger.LogData("output .wav file name with path is not provided", LogLevel.Info);
                    }
                    // Build a prompt.  
                    // Speak the prompt.  
                    synth.SpeakAsync(inputText);
                    synth.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(reader_SpeakCompleted);

                }
                else
                {
                    Log.Logger.LogData("Please provide some text to convert to speech", LogLevel.Error);
                }

               
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Cognitive: Microsoft Cognitive API", LogLevel.Error);
                context.Abort();
            }
        }
        void reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if(synth != null)
            {
                synth.Dispose();
            }
        }
    }

}


