using ELM.Core.BooksAggregator;

namespace ELM.Infrastructure.Interface;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBooksAsync();

    Task<IEnumerable<Book>> GetBooksByIdsAsync(int[] ids);
}