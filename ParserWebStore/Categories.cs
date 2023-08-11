using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www.farfetch.com
{

    public class Rootobject
    {
        public Category category { get; set; }
    }

    public class Category
    {
        public object[] hiddenValues { get; set; }
        public Value[] values { get; set; }
        public string description { get; set; }
        public string filter { get; set; }
        public bool isServerSideRendering { get; set; }
    }

    public class Value
    {
        public Child[] children { get; set; }
        public string urlToken { get; set; }
        public string url { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public int deep { get; set; }
    }

    public class Child
    {
        public Child1[] children { get; set; }
        public string urlToken { get; set; }
        public string url { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public int deep { get; set; }
    }

    public class Child1
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
