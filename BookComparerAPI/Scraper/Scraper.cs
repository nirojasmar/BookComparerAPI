using System;
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
    }
}
