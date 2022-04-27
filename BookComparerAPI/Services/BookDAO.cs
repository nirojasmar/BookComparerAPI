using BookComparerAPI.Models;
using System.Data.SqlClient;
using System.Linq;

namespace BookComparerAPI.Services
{
    public class BookDAO : IBookDataService
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookDealsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public List<Book> GetAllBooks()
        {
            List<Book> foundBooks = new List<Book>();

            String sqlStatement = "SELECT * FROM dbo.Books";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        foundBooks.Add(new Book
                        {
                            Isbn = (long)reader[0],
                            Name = (string)reader[1],
                            Author = (string)reader[2],
                            Editor = (string)reader[3],
                            Language = (string)reader[4],
                            Format = (string)reader[5],
                            Url = (string)reader[6],
                            PriceDates = GetPriceDates((long)reader[0]) //NOT YET TESTED
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return foundBooks;
        }

        public Book GetBookByIsbn(long isbn)
        {
            Book? book = null;

            String sqlStatement = "SELECT * FROM dbo.Books WHERE Isbn = @isbn";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@isbn", System.Data.SqlDbType.BigInt).Value = isbn;
                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        book = new Book
                        {
                            Isbn = (long)reader[0],
                            Name = (string)reader[1],
                            Author = (string)reader[2],
                            Editor = (string)reader[3],
                            Language = (string)reader[4],
                            Format = (string)reader[5],
                            Url = (string)reader[6],
                            PriceDates = GetPriceDates(isbn) //NOT YET TESTED
                        };
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return book;
        }

        public List<PriceDate> GetPriceDates(long isbn)
        {
            List<PriceDate> foundPrices = new List<PriceDate>();
            string sqlStatement = "SELECT * FROM dbo.Prices WHERE BookID = @Isbn";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@Isbn", System.Data.SqlDbType.BigInt).Value = isbn;
                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        foundPrices.Add(new PriceDate(
                            (DateTime)reader[0],
                            (decimal)reader[1],
                            (string)reader[2]
                            ));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return foundPrices;
        }

        public int InsertBook(Book book)
        {
            int result = 0;

            String sqlStatement = "INSERT INTO dbo.Books (Isbn, Name, Author, Editor, Language, Format, Url) VALUES (@Isbn, @Name, @Author, @Editor, @Language, @Format, @Url)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@Isbn", System.Data.SqlDbType.BigInt).Value = book.Isbn;
                command.Parameters.Add("@Name", System.Data.SqlDbType.NChar).Value = book.Name;
                command.Parameters.Add("@Author", System.Data.SqlDbType.NChar).Value = book.Author;
                command.Parameters.Add("@Editor", System.Data.SqlDbType.NChar).Value = book.Editor;
                command.Parameters.Add("@Language", System.Data.SqlDbType.NChar).Value = book.Language;
                command.Parameters.Add("@Format", System.Data.SqlDbType.NChar).Value = book.Format;
                command.Parameters.Add("@Url", System.Data.SqlDbType.NChar).Value = book.Url;

                try
                {
                    connection.Open();

                    result = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return result;
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            List<Book> foundBooks = new List<Book>();

            String sqlStatement = "SELECT * FROM dbo.Books WHERE Name LIKE @Name";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@Name", '%' + searchTerm + '%');
                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        foundBooks.Add(new Book
                        {
                            Isbn = (long)reader[0],
                            Name = (string)reader[1],
                            Author = (string)reader[2],
                            Editor = (string)reader[3],
                            Language = (string)reader[4],
                            Format = (string)reader[5],
                            Url = (string)reader[6],
                            PriceDates =  GetPriceDates((long)reader[0]) //NOT YET TESTED
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return foundBooks;
        }

        public int UpdatePrice(Book book)
        {
            int result = 0;

            String sqlStatement = "INSERT INTO dbo.Prices (BookID, Date, Value, Store) VALUES (@isbn, @Date, @Value, @Store)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@isbn", System.Data.SqlDbType.BigInt).Value = book.Isbn;
                command.Parameters.Add("@Date", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.Add("@Value", System.Data.SqlDbType.Decimal).Value = book.PriceDates.Last().Price;
                command.Parameters.Add("@Store", System.Data.SqlDbType.NChar).Value = book.PriceDates.Last().Store;
                try
                {
                    connection.Open();

                    result = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return result;
        }
    }
}
