using BookComparerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookComparerAPI.Scraping;
using Swashbuckle.AspNetCore.Annotations;
using System.Data.SqlClient;
using BookComparerAPI.Services;

namespace BookComparerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpGet("~/getBookList")] 
        // GET: BookController
        public List<Book> Index()
        {
            BookDAO book = new BookDAO();
            return book.GetAllBooks();
            //return Scraper.GetAmazonBook();
        }

        [HttpGet("~/getPriceList")]
        //GET: PriceList
        public List<PriceDate> IndexPrice(long ISBN)
        {
            BookDAO book = new BookDAO();
            return book.GetBookByIsbn(ISBN).PriceDates;
            //return Scraper.GetAmazonBook()[id].PriceDates;
        }
        
        [HttpGet("~/getSearchResult")]
        // GET: BookController/Search/12345
        public Book Search(long ISBN)
        {
            BookDAO book = new BookDAO();
            return book.GetBookByIsbn(ISBN); //TODO: Not yet tested: DB Pending for Confirmation
        }
    }
}
