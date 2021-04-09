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
    [Designer(typeof(Microsoft_Cognitive_Vision_GenerateThumbnailRemote_ActivityDesigner))]
    [ToolboxBitmap("Resources/MicrosoftCognitiveVisionGenerateThumbnailRemote.png")]

    public class Microsoft_Cognitive_Vision_GenerateThumbnailRemote : BaseNativeActivity, IRegisterMetadata
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

        

        private static async Task GetRemoteThumbnailAsync(ComputerVisionClient computerVision, string imagePath, int thumbnailHeight, int thumbnailWidth, string thumbnailFilePath)
        {
            if (!Uri.IsWellFormedUriString(imagePath, UriKind.Absolute))
            {
                Log.Logger.LogData("Invalid URL: " +imagePath + "\n", LogLevel.Error);
                return;
            }

            Stream thumbnail = await computerVision.GenerateThumbnailAsync(
                thumbnailWidth, thumbnailHeight, imagePath, true);

            
            // Save the thumbnail to the current working directory,
            // using the original name with the suffix "_thumb".
            SaveThumbnail(thumbnail, thumbnailFilePath);
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
                    string imagePath = InputImageURL.Get(context);
                    int timeOut = TimeOut.Get(context);
                    int thumbHeight = ThumbnailHeight.Get(context);
                    int thumbWidth = ThumbnailWidth.Get(context);

                    ComputerVisionClient computerVision = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscrptionKey),new System.Net.Http.DelegatingHandler[] { });
                    computerVision.Endpoint = url;

                    Task.WhenAll(GetRemoteThumbnailAsync(computerVision, imagePath, thumbHeight, thumbWidth,thumbnailPath)).Wait(timeOut);





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


