using System.Drawing;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Net;
using WebClientService;
using www.farfetch.com;
using www_farfetch_com;

namespace farfetch_com
{

    internal class Program
    {
        static void Main(string[] args)
        {
            IParse shop = new Farfetch();
            shop.StartParse();
        }
    }
}
