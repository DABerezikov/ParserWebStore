﻿using System.Net;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WebClientService;
using www.farfetch.com;

namespace www_farfetch_com
{
    public class Farfetch : IParse
    {

        public string NewInPageJsonPathWomen { get; set; } =
            $"Data/{DateTime.Now:d}_farfetch_new_full_page_women.json";
        public string NewInPageJsonPathMen { get; set; } =
            $"Data/{DateTime.Now:d}_farfetch_new_full_page_men.json";

        public string CategoryPath { get; set; } = $"Data/{DateTime.Now:d}_farfetch_new_category.json";
        public string RootPath { get; set; } = @"https://www.farfetch.com";
        public string PathWomen { get; set; } = @"/kz/sets/new-in-this-week-eu-women.aspx";
        public string PathMen { get; set; } = @"/kz/sets/new-in-this-week-eu-men.aspx";
        public string Filter { get; set; } = "Одежда";
        private CookieContainer _cookieContainer = new();

        private GetRequest? _client;

        [Obsolete("Obsolete")]
        public string GetFarfetchResponse(string farfetchPagePath)
        {

            if (_client==null)
                GetClientRequest(farfetchPagePath);

            if (_client?.Address!=farfetchPagePath)
                _client.Address=farfetchPagePath;

            _client.Run(ref _cookieContainer);

            var textResponse = _client.Response;
            return textResponse;
        }
        
        public byte[] GetFarfetchImage(string farfetchPagePath)
        {

            if (_client==null)
                GetClientRequest(farfetchPagePath);

            if (_client?.Address!=farfetchPagePath)
                _client.Address=farfetchPagePath;

            _client.ContentType = "image/webp";
            //_client.Accept = "image/avif, image/webp, image/apng";
            _client.Host = "cdn-images.farfetch-contents.com";
            var tempCookie = new CookieContainer();
            _client.Run(ref tempCookie);

            var byteImage = Convert.FromBase64String(_client.Response);
            _client.ContentType = "text/html; charset=utf-8";
            //_client.Accept = "application/json";
            _client.Host = "www.farfetch.com";
            return byteImage;
        }

        


        private void GetClientRequest(string farfetchPagePath) //TODO Men
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
                Referer = @$"{RootPath}{PathWomen}" //TODO Men
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
            
            var textResponse = response.Substring(response.IndexOf("__=\"", StringComparison.Ordinal) + 4);

            textResponse = textResponse.Substring(0, textResponse.IndexOf("\";", StringComparison.Ordinal))
                .Replace("\\\"", "\"");


            textResponse = textResponse.Remove(textResponse.IndexOf("(", StringComparison.Ordinal), textResponse.IndexOf(")\":", StringComparison.Ordinal));
            return textResponse;
        }

        public static void SaveNewInPageJson(string textResponse, string filePath)
        {

            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");

            File.WriteAllText(filePath, textResponse);

        }

        public static Categories? GetCategories(string filePathIn, string filePathOut)
        {
            string textResponse;
            using (StreamReader reader = new StreamReader(filePathIn))
            {
                textResponse = reader.ReadToEnd();
            }

            var firstIndex = textResponse.IndexOf("\"category\":{", StringComparison.Ordinal);
            var lastIndex = textResponse.IndexOf("\"designer\":{", StringComparison.Ordinal);
            textResponse = textResponse
                .Substring(firstIndex, lastIndex - firstIndex)
                .Replace("\"category\":", "{\"category\":");
            
            textResponse = textResponse.Substring(0, textResponse.LastIndexOf("},", StringComparison.Ordinal) + 1) + "}";
           
            File.WriteAllText(filePathOut, textResponse);
            Console.WriteLine("Категории новинок успешно выделены");

            var data = JsonConvert.DeserializeObject<Categories>(textResponse);
            return data;

        }

        public static ProductInfo? GetProductInfo(string response)
        {
            var textResponse = response;

            var firstIndex = textResponse.IndexOf("\"listingItems\":", StringComparison.Ordinal) + 15;
            var lastIndex = textResponse.IndexOf("\"listingPagination\":", StringComparison.Ordinal) - 1;
            textResponse = textResponse.Substring(firstIndex, lastIndex - firstIndex);

            var data = JsonConvert.DeserializeObject<ProductInfo>(textResponse);

            return data;

        }

        public void GenerateProductReport(ExcelPackage package) //TODO Men
        {
            var arrayBite = package.GetAsByteArray() ?? throw new ArgumentNullException("package.GetAsByteArray()");

            File.WriteAllBytes($"{DateTime.Now:d}NewIn_Women.xlsx", arrayBite);

        }

        [Obsolete("Obsolete")]
        public void StartParse() //TODO Men
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var textResponseWomen = GetFarfetchResponse($"{RootPath}{PathWomen}");

            var dataResponseWomen = GetDataPage(textResponseWomen);

            SaveNewInPageJson(dataResponseWomen, NewInPageJsonPathWomen);

            var categories = GetCategories(NewInPageJsonPathWomen, CategoryPath);
            if (categories == null) return;

            var package = new ExcelPackage();
            
            var productsCategories = categories.CategoryList.categoryNameLists
                .Where(c => c.Description == $"{Filter}")
                .SelectMany(c=>c.productsCategories, (_, category) => new 
                {
                    CategoryName = category.Description,
                    Product = category.Products
                });

            foreach (var productsCategory in productsCategories)
            {
                var sheet = package.Workbook.Worksheets.Add($"{productsCategory.CategoryName}");
                sheet.Cells[1, 1].Value = "Image";
                sheet.Cells[1, 2].Value = "Product";
                sheet.Cells[1, 3].Value = "Link";

                for (int i = 0, row = 2; i < productsCategory.Product.Length; i++, row++)
                {
                    var response = GetFarfetchResponse(
                        $"{RootPath}{PathWomen}?page=1&view=96&sort=3&category={productsCategory.Product[i].Value}");
                    response = GetDataPage(response);
                    var productsInfo = GetProductInfo(response).items[0];
                    var bytes = GetFarfetchImage(productsInfo.images.cutOut);
                    if (bytes.Length != 0)
                    {
                        sheet.Row(row).Height = 100;
                        sheet.Column(1).Width = 20;
                        using Stream str = new MemoryStream(bytes);
                        var excelImage = sheet.Drawings.AddPicture(row.ToString(), str);
                        excelImage.From.Row = row - 1;
                        excelImage.From.Column = 0;
                        excelImage.SetSize(100, 100);
                    }

                    sheet.Cells[row, 2].Value = productsInfo.shortDescription;
                    sheet.Cells[row, 3].Value = $"{RootPath}{productsInfo.url}";
                }
                sheet.Cells[1, 2, sheet.Rows.EndRow, sheet.Columns.EndColumn].AutoFitColumns();
                sheet.Cells[1, 1, sheet.Rows.EndRow, sheet.Columns.EndColumn].Style.HorizontalAlignment =
                    ExcelHorizontalAlignment.Center;
                sheet.Cells[1, 1, sheet.Rows.EndRow, sheet.Columns.EndColumn].Style.VerticalAlignment =
                    ExcelVerticalAlignment.Center;
            }
            GenerateProductReport(package);
            
        }


    }
}












        