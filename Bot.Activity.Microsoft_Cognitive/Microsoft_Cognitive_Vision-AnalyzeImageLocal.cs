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
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Drawing;

namespace Bot.Activity.Microsoft_Cognitive
{  
    [Designer(typeof(Microsoft_Cognitive_Vision_AnalyzeImageLocal_ActivityDesigner))]
    [ToolboxBitmap("Resources/MicrosoftCognitiveVisionAnalyzeImageLocal.png")]
    public class Microsoft_Cognitive_Vision_AnalyzeImageLocal : BaseNativeActivity, IRegisterMetadata
    {
            [RequiredArgument]
            public InArgument<string> InputImagePath { get; set; }

            [RequiredArgument]
            public InArgument<string> EndpointURL { get; set; }

            [RequiredArgument]
            public InArgument<string> SubscriptionKey { get; set; }

            [RequiredArgument]
            public InArgument<int> TimeOut { get; set; }

        [RequiredArgument]
            public InArgument<string> OutputTextPath { get; set; }
            public OutArgument<string> Result { get; set; }


            public void Register()
            {
                throw new NotImplementedException();
            }


            private static readonly List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags
            };


            private static void DisplayResults(ImageAnalysis analysis, string imageUri, string opTextFile)
            {
                Log.Logger.LogData(imageUri, LogLevel.Error);
                if (analysis.Description.Captions.Count != 0)
                {
                    File.WriteAllText(opTextFile, analysis.Description.Captions[0].Text + "\n");
                    Log.Logger.LogData(analysis.Description.Captions[0].Text + "\n", LogLevel.Error);
                }
                else
                {
                    Log.Logger.LogData("No description generated.", LogLevel.Error);
                }
            }
        private static async Task AnalyzeLocalAsync(ComputerVisionClient computerVision, string imagePath, string opText)
        {
            if (!File.Exists(imagePath))
            {
                Log.Logger.LogData(
                    "\nUnable to open or read localImagePath: \n"+ imagePath, LogLevel.Error);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                    imageStream, features);
                DisplayResults(analysis, imagePath, opText);
            }
        }

        protected override void Execute(NativeActivityContext context)
            {
                try
                {
                    string result = string.Empty;
                    string url = EndpointURL.Get(context);
                    string textPath = OutputTextPath.Get(context);
                    string subscrptionKey = SubscriptionKey.Get(context);
                    string imagePath = InputImagePath.Get(context);
                    int timeOut = TimeOut.Get(context);
               
                    ComputerVisionClient computerVision = new ComputerVisionClient(
                    new ApiKeyServiceClientCredentials(subscrptionKey),
                    new System.Net.Http.DelegatingHandler[] { });

                    computerVision.Endpoint = url;

                    Task.WhenAll(AnalyzeLocalAsync(computerVision, imagePath, textPath)).Wait(timeOut);
                    
                


                    Result.Set(context, result);
                }
                catch (Exception ex)
                {
                    Log.Logger.LogData(ex.Message.ToString() + " in activity Cognitive: Microsoft Cognitive API", LogLevel.Error);
                    context.Abort();
                }
            }
        }
    }


