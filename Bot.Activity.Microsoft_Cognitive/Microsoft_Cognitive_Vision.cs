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
using System.Text;
using System.Threading.Tasks;

//Distill actionable information from images
//5,000 transactions, 20 per minute.
//Endpoints
//https://westcentralus.api.cognitive.microsoft.com/vision/v1.0
//https://westcentralus.api.cognitive.microsoft.com/vision/v2.0

//Key 1: 5fa6d477df114cc58c4ffeda0acb57af

//Key 2: 12721bcb45e74d39ac770eb210cf6814

namespace Bot.Activity.Microsoft_Cognitive
{  
    [Designer(typeof(Microsoft_Cognitive_Vision_ActivityDesigner))]
    [ToolboxBitmap("Resources/MicrosoftCognitiveVision.png")]
    public class Microsoft_Cognitive_Vision : BaseNativeActivity, IRegisterMetadata
    {
            [RequiredArgument]
            public InArgument<string> ImagePath { get; set; }

            [RequiredArgument]
            public InArgument<string> Uri { get; set; }

            [RequiredArgument]
            public InArgument<string> APIKey { get; set; }
            public OutArgument<string> Result { get; set; }

            public void Register()
            {
                throw new NotImplementedException();
            }
            public class StringTable
            {
                public string[] ColumnNames { get; set; }
                public string[,] Values { get; set; }
            }
            public string Test(string imagePath, string uri, string apikey)
            {
                
                return Task.Run<string>((Func<Task<string>>)(() => this.MicrosoftCognitiveVision(imagePath, uri, apikey))).Result;
                //str.Wait();
                // str.Result;
            }
            public async Task<string> MicrosoftCognitiveVision(string imagePath, string strUri, string strApikey)
            {
            string imageTextContent = string.Empty;
            using (var client = new HttpClient())
                {
                    try
                    {

                        HttpClient httpclient = new HttpClient();
                        // Add Subscription Key in Request headers.  
                        httpclient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", strApikey);
                        //Set "handwriting" to true in case handwritin text else true for printed text.  
                        string requestParams = "handwriting=false & detectOrientation=true";
                        // Final URI  
                        string uri = "https://westus.api.cognitive.microsoft.com/sts/v1.0" + "?" + requestParams;
                     strApikey = "1fd7ebbac42b489c93d5436fc09ef34f";
                   // uri = "https://westus.api.cognitive.microsoft.com/sts/v1.0/analyze?visualFeatures=Categories&language=en";
                        HttpResponseMessage httpresponse = null;
                        string resultStorageLocation = null;
                        // Get the image as byte array; this method is defined below  
                        byte[] imagebByteData = GetByteArrayOfImage(imagePath);
                        ByteArrayContent imageContent = new ByteArrayContent(imagebByteData);
                        //Set content type: "application/octet-stream" or "application/json"  
                        imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        //The 1st REST APT call to start the async process by submitting the image.  
                        httpresponse = await httpclient.PostAsync(uri, imageContent);
                        //Get location of result from response  
                        if (httpresponse.IsSuccessStatusCode) resultStorageLocation = httpresponse.Headers.GetValues("Operation-Location").FirstOrDefault();
                        //2nd REST API call to get the text content from image  
                        httpresponse = await httpclient.GetAsync(resultStorageLocation);
                        imageTextContent = await httpresponse.Content.ReadAsStringAsync();
                        return imageTextContent;
                        //HttpResponseMessage response = await client.PostAsync(strUri, imageContent).ConfigureAwait(false);
                        //// HttpResponseMessage response = await HttpClientExtensions.PostAsJsonAsync(client, "", scoreRequest).ConfigureAwait(false);
                        //if (response.IsSuccessStatusCode)
                        //    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        //string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        //return string.Format("The request failed with status code: {0}", (object)response.StatusCode);

                }
                    catch (Exception ex)
                    {
                        Logger.Log.Logger.LogData("Error in Microsoft_Cognitive_Vision activity while calling API", LogLevel.Error);
                        return "";
                    }
                    finally
                    {
                        if (client != null)
                            client.Dispose();
                    }
                }
            }
        private byte[] GetByteArrayOfImage(string imagePath)
        {
            FileStream filestreamObj = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryreaderObj = new BinaryReader(filestreamObj);
            return binaryreaderObj.ReadBytes((int)filestreamObj.Length);
        }
        protected override void Execute(NativeActivityContext context)
            {
                try
                {
                    string result = string.Empty;
                    string url = Uri.Get(context);
                    string apiKey = APIKey.Get(context);
                    string imagePath = ImagePath.Get(context);
                    //ThreadInvoker.Instance.RunByUiThread(() =>
                    //{
                    result = Test(imagePath, url, apiKey);

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


