using ReadersBookBank.Data;
using ReadersBookBank.Repository;

namespace ReadersBookBank.Services
{
    public class BookService : IService
    {
        private readonly IRepository _repository;

        public BookService(IRepository repository)
        {
            _repository = repository;
        }

        public bool AddBook(BookDetail book)
        {
            return _repository.AddBook(book);
        }

        public List<BookDetail> ViewAllBooks()
        {
            return _repository.ViewAllBooks();
        }

        public bool RemoveBook(int bookId)
        {
            return _repository.RemoveBook(bookId);
        }
    }
}