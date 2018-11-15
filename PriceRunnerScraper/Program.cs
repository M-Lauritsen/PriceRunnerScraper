using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace PriceRunnerScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hvad vil du søge efter på PriceRunner?");
            GetHTMLAsync();
            Console.ReadKey();

        }

        private static async void GetHTMLAsync()
        {
            // Search Input Using Pricerunner.dk
            string q = Console.ReadLine();            
            var url = "https://www.pricerunner.dk/results?q= "+ q;
            Console.Clear();

            // Using the http client with Async
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            // Load the html document
            var HtmlDocument = new HtmlDocument();
            HtmlDocument.LoadHtml(html);


            var ProductList = HtmlDocument.DocumentNode.Descendants("ol")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("_3s8cb94L7l")).ToList();

            var ProductListItems = ProductList[0].Descendants("li")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("_25C076-hun")).ToList();

            foreach (var product in ProductListItems)
            {
                // the id class (dont wonna show it)
                //Console.WriteLine(product.GetAttributeValue("class", ""));

                // the product name
                Console.WriteLine(product.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("_2toT0OEaJp _4lEE4_tUxu _3D5OyasTJT _1wgy7yB_H3")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));

                // Product info
                Console.WriteLine(product.Descendants("p")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("_2k9_q2p_9L _1-53aS_lww tBxFeb0uqB")).FirstOrDefault().InnerText.Trim('\r','\n','\t'));

                // The Price
                Console.WriteLine(product.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("_1ELjwOz8XJ _2-X5EtVv_D")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));

                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
