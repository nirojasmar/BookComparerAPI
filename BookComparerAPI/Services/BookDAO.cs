using BookComparerAPI.Models;

namespace BookComparerAPI.Services
{
    public class BookDAO : IBookDataService
    {
        public List<Book> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public Book GetBookByIsbn(double isbn)
        {
            throw new NotImplementedException();
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
