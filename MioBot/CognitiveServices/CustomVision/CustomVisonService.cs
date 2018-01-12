using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Net;
using System.Diagnostics;
using MioBot.Utilities;

namespace MioBot.CognitiveServices.CustomService
{
    static class CustomVisonService
    {
        private static string PREDICTKEY = "59b4fafbbc064ff6a7a52c8604ecded6";
        private static string PREDICTURL = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.1/Prediction/66d85d46-37f7-4649-89eb-915ac1c0afaa/image?iterationId=097b2057-b687-460f-a2ed-18b2e4ec574c";

        internal class RestHttpClient
        {
            public HttpResponseMessage ResponseMessage = new HttpResponseMessage();
            public async Task<HttpResponseMessage> SendRequestAsync(string url, ByteArrayContent content)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage responseMessage = null;
                    httpClient.DefaultRequestHeaders.Add("Prediction-Key", PREDICTKEY);

                    try
                    {
                        responseMessage = await httpClient.PostAsync(url, content);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("httpClient.PostAsync failed: {0}", ex));
                    }
                    return responseMessage;
                }
            }
        }

        static public void Invoke(string imageFilePath)
        {
            MakePredictionRequest(imageFilePath).Wait();
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = null;
            BinaryReader binaryReader = null;
            try
            {
                fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
                binaryReader = new BinaryReader(fileStream);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("GetImageAsByteArray failed: {0}", ex));
            }

            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        static async Task MakePredictionRequest(string imageFilePath)
        {
            var client = new HttpClient();
            string json = string.Empty;

            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Prediction-Key", PREDICTKEY);

            HttpResponseMessage response = null;
            // Request body. Try this sample with a locally stored image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                try
                {
                    var str = client.PostAsync(PREDICTURL, content).Result.Content.ReadAsStringAsync().Result;
                    // { "statusCode": 404, "message": "Resource not found" }
                    // { "Code":"BadRequestImageUrl","Message":""}
                    //{ "Code":"BadRequest","Message":"The request entity's media type 'application/octet-stream' is not supported for this resource."}
                    //response.EnsureSuccessStatusCode();

                    Trace.WriteLine(str);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("RestHttpClient.SendRequest failed: {0}", ex));
                }

                await response.Content.ReadAsStringAsync();
            }
        }
    }
}