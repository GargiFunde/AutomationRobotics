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
    [Designer(typeof(Microsoft_Cognitive_Vision_GenerateThumbnailLocal_ActivityDesigner))]
    [ToolboxBitmap("Resources/MicrosoftCognitiveVisionGenerateThumbnailLocal.png")]
    public class Microsoft_Cognitive_Vision_GenerateThumbnailLocal : BaseNativeActivity, IRegisterMetadata
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
            public InArgument<string> OutputThumbnailPath { get; set; }

        [RequiredArgument]
        public InArgument<int> ThumbnailHeight { get; set; }

        [RequiredArgument]
        public InArgument<int> ThumbnailWidth { get; set; }
        public OutArgument<string> Result { get; set; }


            public void Register()
            {
                throw new NotImplementedException();
            }

        private static async Task GetLocalThumbnailAsnc(ComputerVisionClient computerVision, string imagePath, int thumbnailHeight, int thumbnailWidth, string thumbnailFilePath)
        {
            if (!File.Exists(imagePath))
            {
                Log.Logger.LogData( "\nUnable to open or read localImagePath:\n"+imagePath+" \n", LogLevel.Error);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                Stream thumbnail = await computerVision.GenerateThumbnailInStreamAsync(
                    thumbnailWidth, thumbnailHeight, imageStream, true);

                

                
                SaveThumbnail(thumbnail, thumbnailFilePath);
            }
        }

        private static void SaveThumbnail(Stream thumbnail, string thumbnailFilePath)
        {
            using (Stream file = File.Create(thumbnailFilePath))
            {
                thumbnail.CopyTo(file);
                Log.Logger.LogData("Thumbail Generated Successfully!", LogLevel.Error);
            }
        }

        protected override void Execute(NativeActivityContext context)
            {
                try
                {
                    string result = string.Empty;
                    string url = EndpointURL.Get(context);
                    string thumbnailPath = OutputThumbnailPath.Get(context);
                    string subscrptionKey = SubscriptionKey.Get(context);
                    string imagePath = InputImagePath.Get(context);
                    int timeOut = TimeOut.Get(context);
                    int thumbHeight = ThumbnailHeight.Get(context);
                    int thumbWidth = ThumbnailWidth.Get(context);

                    ComputerVisionClient computerVision = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscrptionKey),new System.Net.Http.DelegatingHandler[] { });
                    computerVision.Endpoint = url;

                    Task.WhenAll(GetLocalThumbnailAsnc(computerVision, imagePath, thumbHeight, thumbWidth,thumbnailPath)).Wait(timeOut);





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


