using System;
using BookComparerAPI.Models;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace BookComparerAPI.Scraping
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
            for (int i = 1; i <= 75; i++)
            {
                var html = GetHtml("https://www.amazon.com/s?i=stripbooks&bbn=283155&rh=n%3A283155%2Cp_n_feature_browse-bin%3A2656022011&dc&page=" + i + "&language=es&qid=1646690447&rnid=618072011&ref=sr_pg_"+i);
                // Example URL:
                // https://www.amazon.com/s?i=stripbooks&bbn=283155&rh=n%3A283155%2Cp_n_feature_browse-bin%3A2656022011&dc&page=2&language=es&qid=1646710477&rnid=618072011&ref=sr_pg_2
                var links = html.CssSelect("div.a-section.a-spacing-small");
                foreach (var link in links)
                {
                    try
                    {
                        Book book = new Book();
                        List<PriceDate> priceDates = new List<PriceDate>();
                        book.PriceDates = priceDates;
                        var mainURL = link.CssSelect("a.a-link-normal"); //Book URL and ISBN
                        foreach (var link1 in mainURL)
                        {
                            var uri = link1.Attributes["href"].Value;
                            book.Url = "www.amazon.com" + uri;
                            var substring = uri.Substring(uri.IndexOf("dp/") + 3);
                            book.ISBN = Convert.ToDouble("978" + substring.Remove(9).Replace('X','0'));
                            /*
                             * Examples:
                             *"/-/es/Colleen-Hoover/dp/1501110349/ref=sr_1_15?qid=1646327166&amp;refinements=p_n_feature_browse-bin%3A2656022011&amp;rnid=618072011&amp;s=books&amp;sr=1-15"
                             *To ISBN --> 9781501110349
                             *"/-/es/Gary-Chapman/dp/080241270X/ref=sr_1_16?qid=1646327166&amp;refinements=p_n_feature_browse-bin%3A2656022011&amp;rnid=618072011&amp;s=books&amp;sr=1-16"
                             *To ISBN --> 9780802412700
                             */
                            break;
                        }
                        var mainName = link.CssSelect("span.a-size-medium"); //Book Name
                        foreach (var link1 in mainName)
                        {
                            if(!link1.InnerHtml.Contains("class="))
                            {
                                book.Name = link1.InnerHtml;
                            }
                        }

                        var mainAuthor = link.CssSelect("a.a-size-base"); //Book Author
                        foreach (var link1 in mainAuthor)
                        {
                            if(!link1.InnerHtml.Contains("class="))
                            {
                                book.Author = link1.InnerHtml;
                                break;
                            }
                        }
                        
                        var mainFormat = link.CssSelect("a.a-size-base.a-link-normal.s-underline-text.s-underline-link-text.s-link-style.a-text-bold"); //Book Format
                        foreach (var link1 in mainFormat)
                        {
                            if (!link1.InnerHtml.Contains("class="))
                            {
                                book.Format = link1.InnerHtml;
                                break;
                            }
                        }
                        
                        var mainPrice = link.CssSelect("span.a-offscreen"); //Book Price
                        foreach (var link1 in mainPrice)
                        {
                            if (!link1.InnerHtml.Contains("class="))
                            {
                                PriceDate priceDate = new PriceDate(Convert.ToDecimal(link1.InnerHtml.Replace("US$", "").Replace(',', '.'))/100, "Amazon");
                                book.PriceDates.Add(priceDate);
                                break;
                            }
                        }

                        if(book.ISBN != 0)
                        {
                            Book bookinfo = GetBLInfo(book.ISBN);
                            book.Language = bookinfo.Language;
                            book.Editor = bookinfo.Editor;
                            book.PriceDates.Add(bookinfo.PriceDates[0]);
                        }

                        if (book.Name != null && book.Author != null && book.Format != null && book.Url != null)
                        {
                            bookList.Add(book);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            return bookList;
        }

        public static Book? GetBLInfo(double isbn)
        {
            try
            {
                var html = GetHtml("https://www.buscalibre.com.co/libros/search?q=" + isbn);
                //Example URL:
                //https://www.buscalibre.com.co/libros/search?q=9788416240999

                Book book = new();
                List<PriceDate> priceDates = new List<PriceDate>();
                book.PriceDates = priceDates;

                var mainPrice = html.CssSelect("span"); //Book Price
                foreach (var link in mainPrice)
                {
                    if (!link.InnerHtml.Contains("class="))
                    {
                        PriceDate priceDate = new PriceDate(Convert.ToDecimal(link.InnerHtml.Replace("$ ", "").Replace(',', '.')) / 100, "BuscaLibre");
                        book.PriceDates.Add(priceDate);
                        break;
                    }
                }

                var mainLanguage = html.CssSelect("div#metadata-idioma.box"); //Book Language
                foreach (var link in mainLanguage)
                {
                    if (!link.InnerHtml.Contains("class="))
                    {
                        book.Language = link.InnerHtml;
                        break;
                    }
                }

                var mainEditor = html.CssSelect("a.color-primary.font-weight-medium.link-underline"); //Book Editor
                foreach (var link in mainEditor)
                {
                    if (!link.InnerHtml.Contains("class="))
                    {
                        book.Editor = link.InnerHtml;
                        break;
                    }
                }

                return book;
            }
            catch (Exception)
            {
                return null;
            }           
        }
    }
}
