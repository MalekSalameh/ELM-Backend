using ELM.Application.DTOs;
using ELM.Core.BooksAggregator;

namespace ELM.Application.Mappers;

public interface IBookMapper
{
    public BookDto Map(Book book);
}

public class BookMapper : IBookMapper
{
    public BookDto Map(Book book)
    {
        return new BookDto()
        {
            BookId = book.BookId,
            BookTitle = book.BookTitle,
            Author = book.Author,
            Description = book.Description,
            PublishDate = book.PublishDate,
            CoverBase64 = book.CoverBase64
        };
    }
}