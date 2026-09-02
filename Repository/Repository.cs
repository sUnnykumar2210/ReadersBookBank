using ReadersBookBank.Data;

namespace ReadersBookBank.Repository
{
    public class Repository : IRepository
    {
        private readonly BookDbContext _context;

        public Repository(BookDbContext context)
        {
            _context = context;
        }

        public bool AddBook(BookDetail book)
        {
            try
            {
                _context.BookDetails.Add(book);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<BookDetail> ViewAllBooks()
        {
            try
            {
                return _context.BookDetails.ToList();
            }
            catch
            {
                return new List<BookDetail>();
            }
        }

        public bool RemoveBook(int bookId)
        {
            try
            {
                var book = _context.BookDetails.Find(bookId);

                if (book == null)
                {
                    return false;
                }

                _context.BookDetails.Remove(book);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}