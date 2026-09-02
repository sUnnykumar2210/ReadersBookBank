using ReadersBookBank.Data;

namespace ReadersBookBank.Services
{
    public interface IService
    {
        bool AddBook(BookDetail book);

        List<BookDetail> ViewAllBooks();

        bool RemoveBook(int bookId);
    }
}