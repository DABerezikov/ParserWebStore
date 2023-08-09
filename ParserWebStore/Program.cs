using System.Net;
using System.Net.Http.Headers;
using WebClientService;
using static System.Net.WebRequestMethods;

namespace farfetch_com;

internal class Program
{
    static async Task Main(string[] args)
    {
        var proxy = new WebProxy("127.0.0.1:8888");
        var cookieContainer = new CookieContainer();
        var rootPath = @"https://www.farfetch.com";
        var path = @"/kz/sets/new-in-this-week-eu-women.aspx";
        var client = new GetRequest()
        {
            Address = $"{rootPath}{path}",
            Proxy = proxy,
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
       
        await System.IO.File.WriteAllTextAsync("farfetch_new.json", textResponse);


        var firstIndex = textResponse.IndexOf("\"urlToken\":\"women\"", StringComparison.Ordinal);
        textResponse = textResponse.Substring(firstIndex);
        var lastIndex = textResponse.IndexOf("\"Feature:pdp_membership_awareness_logged_out_module\"", StringComparison.Ordinal);
        textResponse = textResponse.Substring(0, lastIndex);
        firstIndex = textResponse.IndexOf("{", StringComparison.Ordinal);
        textResponse = textResponse.Substring(firstIndex);
        lastIndex = textResponse.LastIndexOf("\";", StringComparison.Ordinal);
        textResponse = textResponse.Substring(0, lastIndex);
        textResponse = textResponse.Replace("\\\"", "\"");
        Thread.Sleep(5000);
        
       
        Console.ReadLine();
    }
}