using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Diagnostics;
using System.Net;
using MioBot.CognitiveServices.CustomService;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using MioBot.BarCode;

namespace MioBot.Dialogs
{
    public class BarCodeRecognize
    {
        private readonly IBarcodeReaderImage reader;
        private readonly string imageFile_;
        public BarCodeRecognize(string imageFile)
        {
            reader = new BarcodeReaderImage();
            imageFile_ = imageFile;
        }

        public string decode()
        {
            var src = new Mat((imageFile_));
            var result = reader.Decode(src);
            if (result != null)
            {
                Trace.WriteLine(result.Text);
                Trace.WriteLine(result.BarcodeFormat.ToString());
                return result.Text;
            }

            return "";
        }
    }

    [Serializable]
    public class BarCodeImageDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }
		
		private static async Task<Stream> GetImageStream(ConnectorClient connector, Attachment imageAttachment)
        {
            using (var httpClient = new HttpClient())
            {
                // The Skype attachment URLs are secured by JwtToken,
                // you should set the JwtToken of your bot as the authorization header for the GET request your bot initiates to fetch the image.
                // https://github.com/Microsoft/BotBuilder/issues/662
                var uri = new Uri(imageAttachment.ContentUrl);
                //if (uri.Host.EndsWith("skype.com") && uri.Scheme == "https")
                //{
                //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync(connector));
                //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                //}

                return await httpClient.GetStreamAsync(uri);
            }
        }
		
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Attachments != null && message.Attachments.Any())
            {
                var attachment = message.Attachments.First();
                using (HttpClient httpClient = new HttpClient())
                {
                    // Skype & MS Teams attachment URLs are secured by a JwtToken, so we need to pass the token from our bot.
                    if ((message.ChannelId.Equals("skype", StringComparison.InvariantCultureIgnoreCase) || message.ChannelId.Equals("msteams", StringComparison.InvariantCultureIgnoreCase))
                        && new Uri(attachment.ContentUrl).Host.EndsWith("skype.com"))
                    {
                        var token = await new MicrosoftAppCredentials().GetTokenAsync();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    Trace.WriteLine(attachment.ContentUrl);

                    string imageFilePath = attachment.ContentUrl;
                    string filename = "d://temp.jpg";
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile(new Uri(imageFilePath), filename);
                        }
                        catch (Exception ex)
                        {
                            // WebClient DownloadFile failed: System.Net.WebException: An exception occurred during a WebClient request. ---> System.IO.IOException: The process cannot access the file 'd:\temp.jpg' because it is being used by another process.
                            Trace.WriteLine(string.Format("WebClient DownloadFile failed: {0}", ex));
                        }
                    }

                    BarCodeRecognize bar_code_recognize = new BarCodeRecognize(filename);
                    string bar_code = bar_code_recognize.decode();

                    String productType = await GetProductTypeBySN(bar_code);
                    string mio_url = "http://www.mio.com.cn/";
                    string mio_register_url = "https://advantage.mio.com/MioAdvantage/login/RegisterAction!toRegister.action?param=vip&com=zh_cn";
                    string mio_login_url = "https://advantage.mio.com/MioAdvantage/login/LoginAction!toLogin.action?com=zh_cn";
                    await context.PostAsync($@"您的機型是{productType}， 
                                        請您至Mio宇达電通官方網址 {mio_url} ，先行註冊會員，
                                        若已註冊，請先登入");

                    Thread.Sleep(1000);
                    await context.PostAsync($"請問是否已經登入完成？");
                    context.Wait(MessageReceivedAsync);
                }
            }
            else
            {
                await context.PostAsync("請重新上傳條形碼圖像！");
            }

            context.Wait(MessageReceivedAsync);
        }

        private async Task<string> GetProductTypeBySN(string sn)
        {
            string prodcutType = ""/*= await ProductTypeBySNAsync(sn)*/;
            if (prodcutType == null)
            {
                return "";
            }
            else
            {
                return "dcr 772";
            }
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                //await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                //context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}