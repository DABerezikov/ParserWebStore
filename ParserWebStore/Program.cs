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
