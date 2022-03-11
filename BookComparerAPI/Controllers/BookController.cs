using BookComparerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookComparerAPI.Scraping;
using Swashbuckle.AspNetCore.Annotations;
using System.Data.SqlClient;

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
            return Scraper.GetAmazonBook();
        }


        [HttpGet("~/getPriceList")]
        //GET: PriceList
        public List<PriceDate> IndexPrice(int id)
        {
            return Scraper.GetAmazonBook()[id].PriceDates;
        }

        // GET: BookController/Search/12345
        /*
        [HttpGet("~/getSearchResult")]
        public Book Search(int ISBN)
        {
            return null; //TODO: Not yet implemented: DB Pending
        }*/
    }
}
