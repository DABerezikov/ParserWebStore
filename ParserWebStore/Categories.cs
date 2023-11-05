using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace www.farfetch.com
{


    public class Categories

    {
        [JsonProperty("category")] 
        public CategoryList categoryList { get; set; }
    }

    public class CategoryList
    {
        [JsonProperty("hiddenValues")]
        public object[] CategoryListInfo { get; set; }
        [JsonProperty("values")]
        public CategoryNameList[] categoryNameLists { get; set; }
        public string description { get; set; }
        public string filter { get; set; }
        public bool isServerSideRendering { get; set; }
    }

    public class CategoryNameList
    {
        [JsonProperty("children")]
        public ProductsCategory[] productsCategories { get; set; }
        public string urlToken { get; set; }
        public string url { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public int deep { get; set; }
    }

    public class ProductsCategory
    {
        [JsonProperty("children")]
        public Product[] products { get; set; }
        public string urlToken { get; set; }
        public string url { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public int deep { get; set; }
    }

    public class Product
    {
        public Child2[] children { get; set; }
        public string urlToken { get; set; }
        public string url { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public int deep { get; set; }
    }

    public class Child2
    {
        public object[] children { get; set; }
        public string urlToken { get; set; }
        public string url { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public int deep { get; set; }
    }

}
