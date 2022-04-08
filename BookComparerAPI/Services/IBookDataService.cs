using BookComparerAPI.Models;

namespace BookComparerAPI.Services
{
    public interface IBookDataService
    {
        List<Book> GetAllBooks();
        List<Book> SearchBooks(string searchTerm);
        Book GetBookByIsbn(double isbn);
        int InsertBook(Book book);
        int UpdatePrice(Book book);
    }
}
