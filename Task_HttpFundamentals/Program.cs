using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteLoader;

namespace Task_HttpFundamentals
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter url");
            string url = Console.ReadLine();
            url = "https://translate.google.com/";
            Console.WriteLine("Enter path");
            string path = Console.ReadLine();
            path = "c:\\Temp\\Links";
            string depth = Console.ReadLine();         
            HTMLParser loader = new HTMLParser(int.Parse(depth));
            loader.ParseSite(url, path);
            Console.ReadKey();
        }
    }
}
