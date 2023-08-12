﻿using Microsoft.VisualBasic;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebClientService;
using www.farfetch.com;
using static System.Net.Mime.MediaTypeNames;

namespace www.farfetch.com
{
    public class Farfetch : IParse
    {

        public string NewInPageJsonPath { get; set; } =
            $"Data/{DateTime.Now.ToString("d")}_farfetch_new_full_page.json";

        public string CategoryPath { get; set; } = $"Data/{DateTime.Now.ToString("d")}_farfetch_new_category.json";
        public string RootPath { get; set; } = @"https://www.farfetch.com";
        public string Path { get; set; } = @"/kz/sets/new-in-this-week-eu-women.aspx";
        public string Filter { get; set; } = "Одежда";
        private CookieContainer _cookieContainer = new CookieContainer();

        private GetRequest? _client;

        public string GetFarfetchResponse(string farfetchPagePath)
        {

            if (_client==null) GetClientRequest(farfetchPagePath);
            if (_client.Address!=farfetchPagePath) _client.Address=farfetchPagePath;

            _client.Run(ref _cookieContainer);

            var textResponse = _client.Response;
            return textResponse;
        }
        
        public Bitmap GetFarfetchImage(string farfetchPagePath)
        {

            if (_client==null) GetClientRequest(farfetchPagePath);
            if (_client.Address!=farfetchPagePath) _client.Address=farfetchPagePath;
            _client.ContentType = "image/webp";
            //_client.Accept = "image/avif, image/webp, image/apng";
            _client.Host = "cdn-images.farfetch-contents.com";
            var tempCookie = new CookieContainer();
            _client.Run(ref tempCookie);

            var image = Base64StringToBitmap(_client.Response);
            _client.ContentType = "text/html; charset=utf-8";
            //_client.Accept = "application/json";
            _client.Host = "www.farfetch.com";
            return image;
        }

        public Bitmap Base64StringToBitmap(string base64String)
        {
            try
            {
                Bitmap bmpReturn = null;


                byte[] byteBuffer = Convert.FromBase64String(base64String);
                MemoryStream memoryStream = new MemoryStream(byteBuffer);


                memoryStream.Position = 0;


                bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);


                memoryStream.Close();
                memoryStream = null;
                byteBuffer = null;


                return bmpReturn;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        private void GetClientRequest(string farfetchPagePath)
        {
            //var proxy = new WebProxy("127.0.0.1:8888");
            
            _client = new GetRequest
            {
                Address = farfetchPagePath,
                //Proxy = proxy,
                Host = "www.farfetch.com",
                ContentType = "text/html; charset=utf-8;",
                UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36",
                Accept = "application/json",
                AcceptGZipEncoding = true,
                Referer = @"https://www.farfetch.com/kz/shopping/women/items.aspx"
            };


            _client.AddHeader("Sec-Ch-Ua",
                "\"Not/A)Brand\";v=\"99\", \"Google Chrome\";v=\"115\", \"Chromium\";v=\"115\"");
            _client.AddHeader("Sec-Ch-Ua-Arch", "\"x86\"");
            _client.AddHeader("Sec-Ch-Ua-Full-Version-List",
                "\"Not/A)Brand\";v=\"99.0.0.0\", \"Google Chrome\";v=\"115.0.5790.171\", \"Chromium\";v=\"115.0.5790.171\"");
            _client.AddHeader("Sec-Ch-Ua-Mobile", "?0");
            _client.AddHeader("Sec-Ch-Ua-Model", "\"\"");
            _client.AddHeader("Sec-Ch-Ua-Platform", "\"Windows\"");
            _client.AddHeader("Sec-Ch-Ua-Platform-Version", "\"15.0.0\"");
            _client.AddHeader("Sec-Fetch-Dest", "document");
            _client.AddHeader("Sec-Fetch-Mode", "navigate");
            _client.AddHeader("Sec-Fetch-Site", "same-origin");
            _client.AddHeader("Sec-Fetch-User", "?1");
            _client.AddHeader("Upgrade-Insecure-Requests", "1");
        }

        public static string GetDataPage(string response)
        {
            var textResponse = response;
            var firstIndex = textResponse.IndexOf("__=\"", StringComparison.Ordinal) + 4;
            textResponse = textResponse.Substring(firstIndex);
            var lastIndex = textResponse.IndexOf("\";", StringComparison.Ordinal);
            textResponse = textResponse.Substring(0, lastIndex);
            textResponse = textResponse.Replace("\\\"", "\"");

            firstIndex = textResponse.IndexOf("(", StringComparison.Ordinal);
            lastIndex = textResponse.IndexOf(")\":", StringComparison.Ordinal);
            textResponse = textResponse.Remove(firstIndex, lastIndex - firstIndex);
            return textResponse;
        }

        public static void SaveNewInPageJson(string textResponse, string filePath)
        {

            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");

            File.WriteAllText(filePath, textResponse);

        }

        public static Categories GetCategories(string filePathIn, string filePathOut)
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

        public static ProductInfo GetProductInfo(string response)
        {
            string textResponse = response;

            var firstIndex = textResponse.IndexOf("\"listingItems\":", StringComparison.Ordinal) + 15;
            var lastIndex = textResponse.IndexOf("\"listingPagination\":", StringComparison.Ordinal) - 1;
            textResponse = textResponse.Substring(firstIndex, lastIndex - firstIndex);
            ProductInfo data = JsonConvert.DeserializeObject<ProductInfo>(textResponse);
            return data;

        }

        public void GenerateProductReport(ExcelPackage package)
        {

            var sheet = package;
            var arrayBite = package.GetAsByteArray();

            File.WriteAllBytes($"{DateTime.Now.ToString("d")}NewIn.xlsx", arrayBite);

        }

        public void StartParse()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var textResponse = GetFarfetchResponse($"{RootPath}{Path}");

            var dataResponse = GetDataPage(textResponse);

            SaveNewInPageJson(dataResponse, NewInPageJsonPath);

            var categories = GetCategories(NewInPageJsonPath, CategoryPath);
                var package = new ExcelPackage();

            foreach (var category in categories.category.values)
            {
                var namePage = category.description;

                foreach (var child in category.children)
                {
                    if (child.description != $"{Filter}") continue;
                    var sheet = package.Workbook.Worksheets.Add($"{Filter} {namePage}");
                    sheet.Cells[1, 1].Value = "Image";
                    sheet.Cells[1, 2].Value = "Category";
                    sheet.Cells[1, 3].Value = "Link";

                    var positionRow = 2;
                    var positionCol = 1;

                    foreach (var productCategory in child.children)
                    {
                        sheet.Cells[positionRow, positionCol].Value = productCategory.description;
                        positionRow++;
                        foreach (var product in productCategory.children)
                        {
                            var response = GetFarfetchResponse(
                                $"{RootPath}/kz/sets/new-in-this-week-eu-women.aspx?page=1&view=96&sort=3&category={product.value}");
                            response = GetDataPage(response);
                            var productsInfo = GetProductInfo(response);
                            foreach (var productInfo in productsInfo.items)
                            {
                                var image = GetFarfetchImage(productInfo.images.cutOut);
                                sheet.Cells[positionRow, positionCol].Value = image;
                                positionCol++;
                                sheet.Cells[positionRow, positionCol].Value = productInfo.shortDescription;
                                positionCol++;
                                sheet.Cells[positionRow, positionCol].Value = $"{RootPath}{productInfo.url}";
                                positionRow++;
                                positionCol = 1;
                            }

                            


                        }
                    }
                }


            }

            GenerateProductReport(package);
        }


    }
}












        