using Newtonsoft.Json;
using System.IO;
using System.Net;
using WebClientService;
using www.farfetch.com;

namespace farfetch_com 
{

    internal class Program
    {
        static async Task Main(string[] args)
        {
            string newInPageJsonPath = $"Data/{DateTime.Now.ToString("d")}_farfetch_new_full_page.json";
            string categoryPath = $"Data/{DateTime.Now.ToString("d")}_farfetch_new_category.json";
            var rootPath = @"https://www.farfetch.com";
            var path = @"/kz/sets/new-in-this-week-eu-women.aspx";

            var textResponse = GetFarfetchResponse($"{rootPath}{path}");

            SaveNewInPageJson(textResponse, newInPageJsonPath);

            var categories = GetCategoriesAsync(newInPageJsonPath, categoryPath);

            Console.ReadLine();
        }

        public static string GetFarfetchResponse (string farfetchNewInPagePath)
        {
            //var proxy = new WebProxy("127.0.0.1:8888");
            var cookieContainer = new CookieContainer();

            var client = new GetRequest()
            {
                Address = farfetchNewInPagePath,
                //Proxy = proxy,
                Host = "www.farfetch.com",
                ContentType = "text/html; charset=utf-8",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36",
                Accept = "application/json",
                AcceptGZipEncoding = true,
                Referer = @"https://www.farfetch.com/kz/shopping/women/items.aspx"
            };


            client.AddHeader("Sec-Ch-Ua", "\"Not/A)Brand\";v=\"99\", \"Google Chrome\";v=\"115\", \"Chromium\";v=\"115\"");
            client.AddHeader("Sec-Ch-Ua-Arch", "\"x86\"");
            client.AddHeader("Sec-Ch-Ua-Full-Version-List", "\"Not/A)Brand\";v=\"99.0.0.0\", \"Google Chrome\";v=\"115.0.5790.171\", \"Chromium\";v=\"115.0.5790.171\"");
            client.AddHeader("Sec-Ch-Ua-Mobile", "?0");
            client.AddHeader("Sec-Ch-Ua-Model", "\"\"");
            client.AddHeader("Sec-Ch-Ua-Platform", "\"Windows\"");
            client.AddHeader("Sec-Ch-Ua-Platform-Version", "\"15.0.0\"");
            client.AddHeader("Sec-Fetch-Dest", "document");
            client.AddHeader("Sec-Fetch-Mode", "navigate");
            client.AddHeader("Sec-Fetch-Site", "same-origin");
            client.AddHeader("Sec-Fetch-User", "?1");
            client.AddHeader("Upgrade-Insecure-Requests", "1");


            client.Run(ref cookieContainer);

            var textResponse = client.Response;
            return textResponse;
        }

        public static void SaveNewInPageJson(string textResponse, string filePath)
        {
            var firstIndex = textResponse.IndexOf("__=\"", StringComparison.Ordinal) + 4;
            textResponse = textResponse.Substring(firstIndex);
            var lastIndex = textResponse.IndexOf("\";", StringComparison.Ordinal);
            textResponse = textResponse.Substring(0, lastIndex);
            textResponse = textResponse.Replace("\\\"", "\"");
            
            firstIndex = textResponse.IndexOf("(", StringComparison.Ordinal);
            lastIndex = textResponse.IndexOf(")\":", StringComparison.Ordinal);
            textResponse = textResponse.Remove(firstIndex, lastIndex - firstIndex);

            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");

            File.WriteAllText(filePath, textResponse);

        }

        public static Categories GetCategoriesAsync(string filePathIn, string filePathOut)
        {
            string textResponse;
            using (StreamReader reader = new StreamReader(filePathIn))
            {
                textResponse = reader.ReadToEnd();
            }

            var firstIndex = textResponse.IndexOf("\"category\":{", StringComparison.Ordinal);
            var lastIndex = textResponse.IndexOf("\"designer\":{", StringComparison.Ordinal);
            textResponse = textResponse.Substring(firstIndex, lastIndex - firstIndex);
            textResponse = textResponse.Replace("\"category\":", "{\"category\":");
            firstIndex = textResponse.LastIndexOf("},", StringComparison.Ordinal);
            textResponse = textResponse.Substring(0, firstIndex + 1);
            textResponse += "}";
            File.WriteAllText(filePathOut, textResponse);
            Console.WriteLine("Категории новинок успешно выделены");
            Categories data = JsonConvert.DeserializeObject<Categories>(textResponse);
            return data;

        }
    } 
}
