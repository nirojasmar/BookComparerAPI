﻿using System;
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
            var bookList = new List<Book>();
            var html = GetHtml("https://www.amazon.com/s?bbn=283155&rh=n%3A283155%2Cp_n_feature_browse-bin%3A2656022011&dc&language=es&qid=1646073525&rnid=618072011&ref=lp_1000_nr_p_n_feature_browse-bin_7");
            // TODO: Get Results for all pages.
            var links = html.CssSelect("div.a-section.a-spacing-small");
            foreach (var link in links)
            {
                try
                {
                    Book book = new Book();
                    var mainURL = link.CssSelect("a.a-link-normal"); //Book URL and ISBN
                    foreach (var link1 in mainURL)
                    {
                        var uri = link1.Attributes["href"].Value;
                        book.Url = uri;
                        //TODO: Extract ISBN from URL
                    }
                    var mainName = link.CssSelect("span.a-size-medium"); //Book Name
                    foreach (var link1 in mainName)
                    {
                        if(!link1.InnerHtml.Contains("class="))
                        {
                            book.Name = link1.InnerHtml;
                        }
                    }

                    var mainAuthor = link.CssSelect("a.a-size-base.a-link-normal"); //Book Author
                    foreach (var link1 in mainAuthor)
                    {
                        if(!link1.InnerHtml.Contains("class="))
                        {
                            book.Author = link1.InnerHtml;    
                        }
                    }
                    var mainFormat = link.CssSelect("a.a-size-base.a-link-normal"); //Book Format
                    foreach (var link1 in mainFormat)
                    {
                        if (!link1.InnerHtml.Contains("class="))
                        {
                            book.Format = link1.InnerHtml;
                        }
                    }
                    var mainPrice = link.CssSelect("span.a-price-whole"); //Book Price
                    foreach (var link1 in mainFormat)
                    {
                        if (!link1.InnerHtml.Contains("class="))
                        {
                            book.AmazonPrice = Convert.ToDecimal(link1.InnerHtml);
                        }
                    }

                    if (book.Name != null && book.Author != null && book.AmazonPrice != 0 && book.Format != null && book.Url != null)
                    {
                        bookList.Add(book);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return bookList;
        }
    }
}
