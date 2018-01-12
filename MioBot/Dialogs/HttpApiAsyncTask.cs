using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading.Tasks;

namespace MioBot.Dialogs
{
    public class HttpApiAsyncTask
    {
        public static async Task<double?> QueryRepairProgressAsync(string PhoneorSN)
        {
            try
            {
                string ServiceURL = $"https://";
                string ResultString;
                using (WebClient client = new WebClient())
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    ResultString = await client.DownloadStringTaskAsync(ServiceURL).ConfigureAwait(false);
                }
                //WeatherData weatherData = (WeatherData)JsonConvert.DeserializeObject(ResultString, typeof(WeatherData));
                //return weatherData;
                return 0;
            }
            catch (WebException ex)
            {
                //handle your exception here  
                //throw ex;
                return null;
            }
        }
    }
}