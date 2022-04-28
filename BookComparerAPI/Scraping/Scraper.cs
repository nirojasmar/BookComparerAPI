using System;
using BookComparerAPI.Models;
using BookComparerAPI.Services;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using Useragents;

namespace BookComparerAPI.Scraping
{
    public class Scraper
    {
        static ScrapingBrowser _scrapBrowser = new ScrapingBrowser();

        public static HtmlNode GetHtml(string URL)
        {
            _scrapBrowser.IgnoreCookies = true;
            _scrapBrowser.Timeout = TimeSpan.FromMinutes(15);
            /*_scrapBrowser.Headers["User-Agent"] = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)" +
                "(compatible, MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";*/
            _scrapBrowser.Headers["User-Agent"] = Collection.GetRandomDesktop();
            WebPage _webPage = _scrapBrowser.NavigateToPage(new Uri(URL));
            return _webPage.Html;
        }

        public static List<Book> GetAmazonBook() //TODO: Change Function into Void Type
        {
            BookDAO books = new BookDAO();
            var bookList = new List<Book>();
            for (int i = 1; i <= 70; i++)
            {
                var html = GetHtml("https://www.amazon.com/-/es/s?i=stripbooks&bbn=283155&rh=n%3A283155%2Cp_n_feature_browse-bin%3A2656022011&dc&page=" + i + "&language=es&qid=1650730501&rnid=618072011&ref=sr_pg_" + i);
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
                            book.Isbn = Convert.ToInt64("978" + substring.Remove(10).Replace('X','0'));
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

                        var mainAuthor = link.CssSelect("a.a-size-base"); //Book Author Case 1
                        foreach (var link1 in mainAuthor)
                        {
                            if(!link1.InnerHtml.Contains("class="))
                            {
                                book.Author = link1.InnerHtml;
                                break;
                            }
                        }

                        mainAuthor = link.CssSelect("span.a-size-base");
                        var link2 = mainAuthor.ElementAtOrDefault(1);
                        if (link2 != null && !link2.InnerHtml.Contains("class=") && book.Author == "Pasta blanda") //Book Author Case 2
                        {
                                book.Author = link2.InnerHtml;
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

                        book.Language = "Ingles";

                        //TODO: Optimize so that when the book is on DB just the price is addded

                        if (books.GetBookByIsbn(book.Isbn) != null)
                        {
                            books.UpdatePrice(book);
                        }

                        if (book.Isbn != 0)
                        {
                            Book? bookinfo = GetBLInfo(book.Isbn);
                            if(bookinfo?.Editor != null)
                            {
                                book.Editor = bookinfo.Editor;
                            }
                            else
                            {
                                book.Editor = "N/A";
                            }
                        }

                        if (book.Name != null && book.Author != null && book.Format != null && book.Url != null)
                        {
                            
                            if(books.GetBookByIsbn(book.Isbn) == null)
                            {
                                books.InsertBook(book);
                                books.UpdatePrice(book);
                            }
                            
                            bookList.Add(book);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return bookList;
        }

        public static Book? GetBLInfo(long isbn)
        {
            BookDAO bookDAO = new BookDAO();
            try
            {
                var html = GetHtml("https://www.buscalibre.com.co/libros/search?q=" + isbn);
                //Example URL:
                //https://www.buscalibre.com.co/libros/search?q=9788416240999

                Book bookBl = new();
                bookBl.Isbn = isbn;
                List<PriceDate> priceDates = new List<PriceDate>();
                bookBl.PriceDates = priceDates;
                var mainPrice = html.CssSelect("script"); //Book Price
                var priceLink = mainPrice.ElementAtOrDefault(1);
                if (priceLink.InnerHtml.Contains("ecomm_totalvalue_us"))
                {
                    var substring = priceLink.InnerText.Substring(priceLink.InnerText.IndexOf("value_us' : '") + 13).Remove(4);
                    PriceDate priceDate = new PriceDate(Convert.ToDecimal(substring) / 10, "BuscaLibre");
                    bookBl.PriceDates.Add(priceDate);
                    bookDAO.UpdatePrice(bookBl);
                }

                var mainEditor = html.CssSelect("a.font-color-text.link-underline"); //Book Editor
                foreach (var link in mainEditor)
                {
                    if (!link.InnerHtml.Contains("class="))
                    {
                        if (link.InnerHtml.Contains("amp"))
                        {
                            bookBl.Editor = link.InnerHtml.Replace("amp;", "");
                        }
                        else
                        {
                            bookBl.Editor = link.InnerText;
                        }
                        break;
                    }
                }

                return bookBl;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
