using Logger;
using System;
using System.Activities;
using System.Activities.Presentation.Metadata;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Drawing;

namespace Bot.Activity.Microsoft_Cognitive
{  
    [Designer(typeof(Microsoft_Cognitive_SpeechToText_ActivityDesigner))]
    [ToolboxBitmap("Resources/MicrosoftCognitiveSpeechToText.png")]
    public class Microsoft_Cognitive_SpeechToText : BaseNativeActivity
    {

            [RequiredArgument]
            public InArgument<string> InputTextFilePath { get; set; }

            [RequiredArgument]
            public InArgument<string> OutputWavFilePath { get; set; }
        
            [RequiredArgument]
            public InArgument<string> Region { get; set; }//"westus"

        [RequiredArgument]
            public InArgument<string> APIKey { get; set; } //63703e27904144758cc77486b0269624
            public OutArgument<string> Result { get; set; }

          
            public void Test(string inputTextFile, string outputWavFile, string region, string apikey)
            {
                
                this.MicrosoftCognitiveVision(inputTextFile, outputWavFile, region, apikey).Wait();
            //str.Wait();
            // str.Result;
            
        }
            public async Task MicrosoftCognitiveVision(string inputtextFile, string outputwavefile, string region, string strApikey)
            {
            //string imageTextContent = string.Empty;
            using (var client = new HttpClient())
                {
                    try
                    {
                    var opfileName = outputwavefile;
                    var ipfilename = inputtextFile;
                    string text = File.ReadAllText(ipfilename);

                    var config = SpeechConfig.FromSubscription(strApikey, region);
                    
                    using (var fileOutput = AudioConfig.FromWavFileOutput(opfileName))
                    using (var synthesizer = new SpeechSynthesizer(config, fileOutput))
                    {
                        
                        await synthesizer.SpeakTextAsync(text);
                    }

                }
                    catch (Exception ex)
                    {
                        Logger.Log.Logger.LogData("Error in Microsoft_Cognitive_Vision activity while calling API", LogLevel.Error);
                        
                    }
                    finally
                    {
                        if (client != null)
                            client.Dispose();
                    }
                }
            }
      
        protected override void Execute(NativeActivityContext context)
            {
                try
                {
                    string result = string.Empty;
                    string region = Region.Get(context);
                    string apiKey = APIKey.Get(context);
                    string inputtextFile = InputTextFilePath.Get(context);
                    string outputWavFile = OutputWavFilePath.Get(context);
                //ThreadInvoker.Instance.RunByUiThread(() =>
                //{
                    Test(inputtextFile, outputWavFile, region, apiKey);

                    //});
                    Result.Set(context, result);
                }
                catch (Exception ex)
                {
                    Log.Logger.LogData(ex.Message + " in activity Cognitive: Microsoft Cognitive API", LogLevel.Error);
                    context.Abort();
                }
            }
        }
    }


