using Logger;
using Newtonsoft.Json.Linq;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.MicrosoftOCR
{
    [Designer(typeof(MicrosoftOcr_ActivityDesigner))]
    [ToolboxBitmap("Resources/MicrosoftOCR.png")]
    public class MicrosoftOcr : BaseNativeActivity
    {

        [RequiredArgument]
        [Category("Input Paramaters")]
        [DisplayName("Image Path")]
        [Description("Enter Image Path")]
        public InArgument<string> ImagePath { get; set; }

        [RequiredArgument]
        [Category("Output Paramaters")]
        [DisplayName("Text Value")]
        [Description("Enter Text Values")]
        public OutArgument<string> TextValues { get; set; }

        // Add your Computer Vision subscription key and endpoint to your environment variables.

        static string subscriptionKey = ReadKey();

        static string endpoint = ReadEndPoint();

        //static string subdadscriptday = ConfigurationManager.AppSettings["LogstashUrl"];
        //static string subscriptionKey = ConfigurationManager.AppSettings["subkey"];

        //static string endpoint = ConfigurationManager.AppSettings["subendpoint"];
        //static string endpoint = "https://eastus2.api.cognitive.microsoft.com/";

        // the OCR method endpoint
        //static string uriBase = endpoint + "vision/v2.1/ocr";
        static string uriBase = endpoint + "vision/v2.1/ocr";
        static string fullString = null;

        //Method to fetch a Endpoint From file
        public static string ReadEndPoint()
        {

            try
            {
                string path = "C:\\localizationGlobalization\\forXpath 01-04-2020\\BotDesignerMaster\\Bot.Activity.GoogleOCR\\EndPoint.txt";
                if (!string.IsNullOrEmpty(path))
                {
                    string readText = File.ReadAllText(path);
                    StringBuilder strbuild = new StringBuilder();
                    //foreach (string s in readText)
                    //{
                    //    strbuild.Append(s);
                    //    strbuild.AppendLine();
                    //}


                    readText = readText.Replace("\n", String.Empty);
                    readText = readText.Replace("\r", String.Empty);

                    strbuild.Append(readText);
                    return strbuild.ToString();
                }


            }
            catch (Exception e)
            {
                Log.Logger.LogData(e.Message, LogLevel.Error);
            }
            return null;
        }

        //Method to fetch a Key From file
        public static string ReadKey()
        {
            try
            {
                string path = "C:\\localizationGlobalization\\forXpath 01-04-2020\\BotDesignerMaster\\Bot.Activity.GoogleOCR\\Key.txt";
                if (!string.IsNullOrEmpty(path))
                {
                    string readText = File.ReadAllText(path);
                    StringBuilder strbuild = new StringBuilder();
                    //foreach (string s in readText)
                    ////{
                    //    strbuild.Append(s);
                    //    strbuild.AppendLine();
                    //}
                    readText = readText.Replace("\n", String.Empty);
                    readText = readText.Replace("\r", String.Empty);
                    strbuild.Append(readText);

                    return strbuild.ToString();
                }

            }
            catch (Exception e)
            {

                Log.Logger.LogData(e.Message, LogLevel.Error);
            }
            return null;
        }

        protected override async void Execute(NativeActivityContext context)
        {
            try
            {

                //bOTServiceClient = new Logger.ServiceReference1.BOTServiceClient();
                //bOTServiceClient.getMicrosoftOCR();

                // Get the path and filename to process from the user.

                string imageFilePath = ImagePath.Get(context);

                if (File.Exists(imageFilePath))
                {
                    // Call the REST API method.
                    MakeOCRRequest(imageFilePath).Wait();
                    TextValues.Set(context, fullString);
                    fullString = null;

                }
                else
                {
                    Log.Logger.LogData("Invalid file path", LogLevel.Error);
                }
            }
            catch (Exception)
            {

                Log.Logger.LogData("Invalid file path", LogLevel.Error);
            }

        }

        static async Task MakeOCRRequest(string imageFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API method.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Read the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                JObject results = JObject.Parse(contentString);


                foreach (var i in results["regions"])
                {
                    foreach (var lineIndex in i["lines"])
                    {
                        foreach (var wordIndex in lineIndex["words"])
                        {
                            fullString += wordIndex["text"] + " ";

                        }

                    }
                }




                // Display the JSON response.
                //MessageBox.Show("\nResponse:\n\n{0}\n",
                //    JToken.Parse(contentString).ToString());

            }
            catch (Exception e)
            {
                Log.Logger.LogData(e.Message, LogLevel.Error);
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }


    }

}
