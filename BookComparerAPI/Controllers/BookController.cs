using BookComparerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookComparerAPI.Scraping;

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

        [HttpGet(Name = "GetBookList")]
        // GET: BookController
        public List<Book> Index()
        {
            return Scraper.GetAmazonBook();
        }

        // GET: BookController/Search/12345
        public Book Search(int ISBN)
        {
            return null; //TODO: Not yet implemented
        }
    }
}
