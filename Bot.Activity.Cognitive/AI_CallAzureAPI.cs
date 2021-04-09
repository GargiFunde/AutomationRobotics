using CommonLibrary;
using Logger;
using System;
using System.Activities;
using System.Activities.Presentation.Metadata;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.Cognitive
{
    [ToolboxBitmap("Resources/AICallAzureAPI.png")]
    [Designer(typeof(AI_CallAzureAPI_ActivityDesigner))]
    public class AI_CallAzureAPI : NativeActivity,IRegisterMetadata
    {
        [RequiredArgument]
        public InArgument<string[]> Inputs { get; set; }

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
        public string Test(string[] colvalues, string uri, string apikey)
        {
            string col1 = colvalues[0];
            string col2 = colvalues[1];
            string col3 = colvalues[2];
            string col4 = colvalues[3];
            string col5 = colvalues[4];
           
            return Task.Run<string>((Func<Task<string>>)(() => this.PreApproveCredit(col1, col2, col3, col4, col5, uri, apikey))).Result;
            //str.Wait();
            // str.Result;
        }
        public async Task<string> PreApproveCredit(string col1, string col2, string col3, string col4, string col5, string strUri, string strApikey)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"Col1", "Col2", "Col3", "Col4", "Col5"},
                                Values = new string[,] { { col1, col2, col3, col4, col5 } }
                                //Values = new string[,] {  { "5", "3.6", "1.4", "0.2", "value" },  { "0", "0", "0", "0", "value" },  }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                string apiKey = strApikey;// "Uqk2QeoLoSlQ6y8zS9uxuJtJFNJujrjX7sL4V4xGIFoJdRX1fXEfppUL0CXCvCNULDR1Txmu57lw9zkZw9ecOg=="; // Replace this with the API key for the web service
                //const string apiKey = "Uqk2QeoLoSlQ6y8zS9uxuJtJFNJujrjX7sL4V4xGIFoJdRX1fXEfppUL0CXCvCNULDR1Txmu57lw9zkZw9ecOg=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri(strUri); // ("https://ussouthcentral.services.azureml.net/workspaces/da6fbfecfe9d4b6ebd7b4f7f19d06469/services/42b0937b0ae740afba1c36e4cbcbf24d/execute?api-version=2.0&details=true");
                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)
                try
                {
                  
                    HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);
                   // HttpResponseMessage response = await HttpClientExtensions.PostAsJsonAsync(client, "", scoreRequest).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return string.Format("The request failed with status code: {0}", (object)response.StatusCode);

                }
                catch(Exception ex)
                {
                    Logger.Log.Logger.LogData("Error in AI_CallAzureAPI activity while calling API", LogLevel.Error);
                    return "";
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
               string url = Uri.Get(context);
                string apiKey = APIKey.Get(context);
                string[] colValues = Inputs.Get(context);
                //ThreadInvoker.Instance.RunByUiThread(() =>
                //{
                    result = Test(colValues,url,apiKey);
                   
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
