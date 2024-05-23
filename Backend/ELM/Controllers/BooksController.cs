using ELM.Application.DTOs;
using ELM.Application.Mappers;
using ELM.Core.BooksAggregator;
using ELM.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ELM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMemoryCache _cache;
    private readonly IBookMapper _bookMapper;
    private readonly IBookRepository _bookRepository;

    public BooksController(IBookRepository bookRepository, IMemoryCache cache, IBookMapper bookMapper)
    {
        _bookRepository = bookRepository;
        _cache = cache;
        _bookMapper = bookMapper;
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(string query, int page = 1, int pageSize = 10)
    {
        List<Book> allBooks;

        if (_cache.TryGetValue("Books", out List<Book> cachedBooks))
        {
            allBooks = cachedBooks;
        }
        else
        {
            allBooks = (List<Book>)await _bookRepository.GetAllBooksAsync();
            _cache.Set("Books", allBooks, TimeSpan.FromHours(6));
        }

        var paginatedBooks = allBooks
                .Where(b => b.BookTitle.Contains(query) || b.Author.Contains(query) || b.Description.Contains(query) || b.PublishDate.Contains(query))
                .Skip((page - 1) * pageSize).Take(pageSize).Select(b => b.BookId).ToArray();

        var books = await _bookRepository.GetBooksByIdsAsync(paginatedBooks);

        return Ok(books.Select(b => _bookMapper.Map(b)));
    }
}