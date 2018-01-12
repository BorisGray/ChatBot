namespace MioBot.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    static class LuisRequest
    {
        private static readonly String id = "ba78620f-b457-4320-978a-5873c98f9719";
        private static readonly String key = "971d1d53933a4f8bbd11bd2051be42d3";

        public static async Task<T> RequestAsync<T>(String input)
        {
            var strEscaped = Uri.EscapeDataString(input);
            var url = $"https://api.projectoxford.ai/luis/v1/application?id={id}&subscription-key={key}&q={strEscaped}";
            //var url = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{id}?subscription-key={key}&verbose=true&timezoneOffset=0&q=";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }

            return default(T);
        }
    }
}
