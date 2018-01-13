using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace WebScrapper_EbayScrapper_v1
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();

            Console.ReadLine();
        }

        private static async void GetHtmlAsync()
        {
            var url = "https://www.ebay.com/sch/i.html?_nkw=xbox+one&_in_kw=1&_ex_kw=&_sacat=0&LH_Complete=1&_udlo=&_udhi=&_samilow=&_samihi=&_sargn=-1%26saslc%3D1&_salic=1&_sop=12&_dmd=1&_ipg=50";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var productsHtml = htmlDocument.DocumentNode.Descendants("ul")
                                                        .Where(node=> node.GetAttributeValue("id","")
                                                        .Equals("ListViewInner"))
                                                        .ToList();


            var productListItems = productsHtml[0].Descendants("li")
                                                  .Where(node => node.GetAttributeValue("id", "")
                                                  .Contains("item"))
                                                  .ToList();

            foreach(var productListItem in productListItems)
            {
                Console.WriteLine(productListItem.GetAttributeValue("listingid","empty"));

                Console.WriteLine(productListItem.Descendants("h3").Where(node => node.GetAttributeValue("class","")
                                                                   .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r','\n','\t') + "\n");
            }

            Console.WriteLine();
        }
    }
}
