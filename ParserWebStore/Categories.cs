using Newtonsoft.Json;

namespace www_farfetch_com
{


    public class Categories

    {
        [JsonProperty("category")] 
        public CategoryList CategoryList { get; set; }
    }

    public class CategoryList
    {
        [JsonProperty("hiddenValues")]
        public object[] CategoryListInfo { get; set; }
        [JsonProperty("values")]
        public CategoryNameList[] categoryNameLists { get; set; }
        public string Description { get; set; }
        public string Filter { get; set; }
        public bool IsServerSideRendering { get; set; }
    }

    public class CategoryNameList
    {
        [JsonProperty("children")]
        public ProductsCategory[] productsCategories { get; set; }
        public string UrlToken { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Deep { get; set; }
    }

    public class ProductsCategory
    {
        [JsonProperty("children")]
        public Product[] Products { get; set; }
        public string UrlToken { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Deep { get; set; }
    }

    public class Product
    {
        public Child2[] Children { get; set; }
        public string UrlToken { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Deep { get; set; }
    }

    public class Child2
    {
        public object[] Children { get; set; }
        public string UrlToken { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Deep { get; set; }
    }

}
