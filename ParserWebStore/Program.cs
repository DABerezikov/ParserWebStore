﻿using System.Net.Http.Headers;

namespace ParserWebStore;

internal class Program
{
    static async Task Main(string[] args)
    {
        var client = new HttpClient();
        var path = @"https://www.farfetch.com/kz/sets/new-in-this-week-eu-women.aspx";
        client.BaseAddress = new Uri(path);
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        var source = new MediaTypeWithQualityHeaderValue("application/json");
        //source.CharSet = "utf-8";
        //source.Quality = 0.9;
        client.DefaultRequestHeaders.Accept.Add(source);
        
        var response = await client.GetAsync(path).ConfigureAwait(false);
        var textResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Console.WriteLine(textResponse);
        var firstIndex = textResponse.IndexOf("<script type=\"application/ld+json\">", StringComparison.Ordinal);
        textResponse = textResponse.Substring(firstIndex);
        var lastIndex = textResponse.IndexOf("<style data-emotion=\"ltr 1j8wdcp\">", StringComparison.Ordinal);
        textResponse = textResponse.Substring(0, lastIndex);
        firstIndex = textResponse.IndexOf("{", StringComparison.Ordinal);
        textResponse = textResponse.Substring(firstIndex);
        lastIndex = textResponse.LastIndexOf("<", StringComparison.Ordinal);
        textResponse = textResponse.Substring(0, lastIndex);
        await File.WriteAllTextAsync("farfetch_new.json", textResponse);
        Thread.Sleep(5000);
        
        Console.WriteLine(response);
        Console.ReadLine();
    }
}