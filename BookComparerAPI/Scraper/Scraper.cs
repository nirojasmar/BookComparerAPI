using System;
using BookComparerAPI.Models;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace BookComparerAPI.Scraper
{
    public class Scraper
    {
        static ScrapingBrowser _scrapBrowser = new ScrapingBrowser();

        public static HtmlNode GetHtml(string URL)
        {
            _scrapBrowser.IgnoreCookies = true;
            _scrapBrowser.Timeout = TimeSpan.FromMinutes(15);
            _scrapBrowser.Headers["User-Agent"] = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)" +
                "(compatible, MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            WebPage _webPage = _scrapBrowser.NavigateToPage(new Uri(URL));
            return _webPage.Html;
        }

        public static List<Book> GetAmazonBook()
        {
            var BookName = new List<Book>();
            var html = GetHtml("https://www.amazon.com/s?i=stripbooks&bbn=1000&rh=n%3A283155%2Cn%3A25&dc&fs=true&language=es&qid=1645550499&rnid=1000&ref=sr_nr_n_7");
            //Verification Pending
            var links = html.CssSelect("div.a-scetion.a-spacing-none");
            foreach (var link in links)
            {
                try
                {
                    Book book = new Book();
                    var mainName = link.CssSelect("span.a-size-medium"); //Book Name
                    foreach (var link1 in mainName)
                    {
                        if(!link1.InnerHtml.Contains("class="))
                        {
                            book.Name = link1.InnerHtml;
                        }
                    }

                    var mainPrice = link.CssSelect("span.a-price-whole"); //Book Price
                    foreach (var link1 in mainName)
                    {
                        if(!link1.InnerHtml.Contains("class="))
                        {
                            book.AmazonPrice = Convert.ToDecimal(link1.InnerHtml);    
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return BookName;
        }
    }
}
