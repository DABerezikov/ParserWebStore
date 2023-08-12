using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www.farfetch.com
{

    public class ProductInfo
    {
        public Item[] items { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public string shortDescription { get; set; }
        public int merchantId { get; set; }
        public Brand brand { get; set; }
        public string gender { get; set; }
        public Images images { get; set; }
        public Priceinfo priceInfo { get; set; }
        public string merchandiseLabel { get; set; }
        public string merchandiseLabelField { get; set; }
        public bool isCustomizable { get; set; }
        public Availablesize[] availableSizes { get; set; }
        public int stockTotal { get; set; }
        public string url { get; set; }
        public object promotionLabel { get; set; }
        public bool isForMembers { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Brand
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Images
    {
        public string cutOut { get; set; }
        public string model { get; set; }
        public object all { get; set; }
    }

    public class Priceinfo
    {
        public string formattedFinalPrice { get; set; }
        public string formattedInitialPrice { get; set; }
        public int finalPrice { get; set; }
        public int initialPrice { get; set; }
        public string currencyCode { get; set; }
        public bool isOnSale { get; set; }
        public object discountLabel { get; set; }
        public object installmentsLabel { get; set; }
    }

    public class Properties
    {
    }

    public class Availablesize
    {
        public int scaleId { get; set; }
        public string size { get; set; }
    }



}

