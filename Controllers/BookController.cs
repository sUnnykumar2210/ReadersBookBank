using Microsoft.AspNetCore.Mvc;
using ReadersBookBank.Data;
using ReadersBookBank.Models;
using ReadersBookBank.Services;

namespace ReadersBookBank.Controllers
{
    public class BookController : Controller
    {
        private readonly IService _service;

        public BookController(IService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var books = _service.ViewAllBooks();

            var model = books.Select(b => new Book
            {
                Id = b.Id,
                BookName = b.BookName,
                Genre = b.Genre,
                AvailabilityStatus = b.AvailabilityStatus
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
public IActionResult AddBook(Book book)
{
    if (!ModelState.IsValid)
    {
        foreach (var error in ModelState)
        {
            Console.WriteLine($"KEY: {error.Key}");

            foreach (var item in error.Value.Errors)
            {
                Console.WriteLine($"ERROR: {item.ErrorMessage}");
            }
        }

        return View(book);
    }

            var bookDetail = new BookDetail
            {
                Id = book.Id,
                BookName = book.BookName,
                Genre = book.Genre,
                AvailabilityStatus = book.AvailabilityStatus
            };

            var result = _service.AddBook(bookDetail);

            if (result)
            {
                return RedirectToAction("Index");
            }

            return View(book);
        }

        [HttpGet]
        public IActionResult RemoveBook(int id)
        {
            var result = _service.RemoveBook(id);

            return RedirectToAction("Index");
        }
    }
}