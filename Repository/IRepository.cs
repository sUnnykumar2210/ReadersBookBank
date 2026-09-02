using ReadersBookBank.Data;

namespace ReadersBookBank.Repository
{
    public interface IRepository
    {
        bool AddBook(BookDetail book);

        List<BookDetail> ViewAllBooks();

        bool RemoveBook(int bookId);
    }
}