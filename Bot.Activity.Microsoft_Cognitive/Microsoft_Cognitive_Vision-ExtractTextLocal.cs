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
    [Designer(typeof(Microsoft_Cognitive_Vision_ExtractTextLocal_ActivityDesigner))]
    [ToolboxBitmap("Resources/MicrosoftCognitiveVisionExtractTextLocal.png")]
    public class Microsoft_Cognitive_Vision_ExtractTextLocal : BaseNativeActivity, IRegisterMetadata
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



        private static async Task ExtractLocalTextAsync(
            ComputerVisionClient computerVision, string imagePath, string opTextPath)
        {
            if (!File.Exists(imagePath))
            {
                Log.Logger.LogData(
                    "\nUnable to open or read localImagePath:\n"+imagePath+" \n", LogLevel.Error);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                // Start the async process to recognize the text
                BatchReadFileInStreamHeaders textHeaders =
                    await computerVision.BatchReadFileInStreamAsync(
                        imageStream,TextRecognitionMode.Handwritten);

                await GetTextAsync(computerVision, textHeaders.OperationLocation, opTextPath);
            }
        }

        private static async Task GetTextAsync(
            ComputerVisionClient computerVision, string operationLocation, string opTextPath)
        {
            // Retrieve the URI where the recognized text will be
            // stored from the Operation-Location header
            string operationId = operationLocation.Substring(
                operationLocation.Length - 36);

            //Log.Logger.LogData("\nCalling GetHandwritingRecognitionOperationResultAsync()", LogLevel.Error);
            ReadOperationResult result =
                await computerVision.GetReadOperationResultAsync(operationId);

            // Wait for the operation to complete
            int i = 0;
            int maxRetries = 10;
            while ((result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries)
            {
               
                await Task.Delay(1000);

                result = await computerVision.GetReadOperationResultAsync(operationId);
            }

            // Display the results
            //Console.WriteLine();
            var recResults = result.RecognitionResults;
            string ExText = "";
            foreach (TextRecognitionResult recResult in recResults)
            {
                foreach (Line line in recResult.Lines)
                {
                    ExText = ExText + line.Text + System.Environment.NewLine;
                }
            }
            File.WriteAllText(opTextPath, ExText);
            Log.Logger.LogData("File Saved!", LogLevel.Info);
        }


        protected override void Execute(NativeActivityContext context)
            {
                try
                {
                    string result = string.Empty;
                    string url = EndpointURL.Get(context);
                    string opTextPath = OutputTextPath.Get(context);
                    string subscrptionKey = SubscriptionKey.Get(context);
                    string imagePath = InputImagePath.Get(context);
                    int timeOut = TimeOut.Get(context);

                    ComputerVisionClient computerVision = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscrptionKey),new System.Net.Http.DelegatingHandler[] { });
                    computerVision.Endpoint = url;
                    Task.WhenAll(ExtractLocalTextAsync(computerVision, imagePath, opTextPath)).Wait(timeOut);



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


