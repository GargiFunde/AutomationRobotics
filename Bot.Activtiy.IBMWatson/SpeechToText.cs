using IBM.Cloud.SDK.Core.Authentication;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using IBM.Watson.SpeechToText.v1;
using IBM.Watson.SpeechToText.v1.Model;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activtiy.IBMWatson
{
    public class SpeechToText : NativeActivity
    {
        [Category("Common_Category")]
        [DisplayName("ContinueOnError")]
        [Description("ContinueOnError")]
        public InArgument<bool> ContinueOnError { get; set; } = (InArgument<bool>)false;



        [DisplayName("URI")]
        [Description("URI")]
        [Category("Input_Category")]
        public InArgument<string> URI { get; set; }

        [DisplayName("Key")]
        [Description("Key")]
        [Category("Input_Category")]
        public InArgument<string> Key { get; set; }

        [DisplayName("AudioFilePath")]
        [Description("AudioFilePath")]
        [Category("Input_Category")]
        public InArgument<string> AudioFilePath { get; set; }

        [DisplayName("ContentType")]
        [Description("")]
        [Category("Input_Category")]
        public InArgument<string> ContentType { get; set; }

        [DisplayName("JSONResponse")]
        [Description("JSONResponse")]
        [Category("Output_Category")]
        public OutArgument<string> JSONResponse { get; set; }

        [DisplayName("StatusCode")]
        [Description("StatusCode")]
        [Category("Output_Category")]
        public OutArgument<long> StatusCode { get; set; }

        /// <summary>Recognize speech using IBM Watson Speech to tex api.</summary>
        /// <param name="apiKey"></param>
        /// <param name="apiURI"></param>
        /// <param name="audioFilePath"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>


        protected override void Execute(NativeActivityContext context)
        {
            try
            {

                GetData(context);

            }
            catch (Exception ex)
            {

                this.JSONResponse.Set(context, ex.Message);
                this.StatusCode.Set(context, -1L);
            }
        }


        private void GetData(NativeActivityContext context)
        {

            string apiURI = URI.Get(context), apiKey = Key.Get(context), audioFilePath = AudioFilePath.Get(context), contentType = ContentType.Get(context);

            var t = IBMWatsonSpeechToTextAsync(apiURI, apiKey, audioFilePath, contentType);
            t.Wait();
            try
            {
                this.JSONResponse.Set(context, t.Result.Response);
                this.StatusCode.Set(context, t.Result.StatusCode);
            }
            catch (Exception ex)
            {
                this.JSONResponse.Set(context, ex.Message);
                this.StatusCode.Set(context, -1L);


            }

        }

        private async Task<DetailedResponse<SpeechRecognitionResults>> IBMWatsonSpeechToTextAsync(string apiURI, string apiKey, string audioFilePath, string contentType)
        {
            DetailedResponse<SpeechRecognitionResults> detailedResponse;
            IamAuthenticator authenticator = new IamAuthenticator(apiKey);
            SpeechToTextService speechToText = new SpeechToTextService((IAuthenticator)authenticator);
            speechToText.SetServiceUrl(apiURI);
            speechToText.WithHeader("X-Watson-Learning-Opt-Out", "true");
            DetailedResponse<SpeechRecognitionResults> results = (DetailedResponse<SpeechRecognitionResults>)null;
            Task<DetailedResponse<SpeechRecognitionResults>> task = Task.Run<DetailedResponse<SpeechRecognitionResults>>((Func<DetailedResponse<SpeechRecognitionResults>>)(() =>
            {
                results = speechToText.Recognize(File.ReadAllBytes(audioFilePath), contentType, (string)null, (string)null, (string)null, (string)null, new double?(), new long?(), (List<string>)null, new float?(), new long?(), new float?(0.9f), new bool?(), new bool?(), new bool?(), new bool?(), new bool?(), (string)null, (string)null, new bool?(), new bool?(), new double?(), new bool?());
                return results;
            }));

            detailedResponse = await task.ConfigureAwait(false);

            return detailedResponse;
        }
    }
}
