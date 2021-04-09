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
    [Designer(typeof(Microsoft_Cognitive_Vision_AnalyzeImageRemote_ActivityDesigner))]
    [ToolboxBitmap("Resources/MicrosoftCognitiveVisionAnalyzeImageRemote.png")]
    public class Microsoft_Cognitive_Vision_AnalyzeImageRemote : BaseNativeActivity, IRegisterMetadata
    {
            [RequiredArgument]
            public InArgument<string> InputImageURL { get; set; }

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
        private static async Task AnalyzeRemoteAsync(ComputerVisionClient computerVision, string imageUrl, string opText)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Log.Logger.LogData("\nInvalid remoteImageUrl:\n"+imageUrl+" \n", LogLevel.Error);
                return;
            }

            ImageAnalysis analysis =
                await computerVision.AnalyzeImageAsync(imageUrl, features);
            DisplayResults(analysis, imageUrl, opText);
        }

        protected override void Execute(NativeActivityContext context)
            {
                try
                {
                    string result = string.Empty;
                    string url = EndpointURL.Get(context);
                    string textPath = OutputTextPath.Get(context);
                    string subscrptionKey = SubscriptionKey.Get(context);
                    string imagePath = InputImageURL.Get(context);
                    int timeOut = TimeOut.Get(context);
               
                    ComputerVisionClient computerVision = new ComputerVisionClient(
                    new ApiKeyServiceClientCredentials(subscrptionKey),
                    new System.Net.Http.DelegatingHandler[] { });

                    computerVision.Endpoint = url;

                    Task.WhenAll(AnalyzeRemoteAsync(computerVision, imagePath, textPath)).Wait(timeOut);
                    
                


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


