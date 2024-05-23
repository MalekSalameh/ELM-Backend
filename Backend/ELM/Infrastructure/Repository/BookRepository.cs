using System.Data;
using Dapper;
using ELM.Core.BooksAggregator;
using ELM.Infrastructure.Interface;

namespace ELM.Infrastructure.Repository;

public class BookRepository : IBookRepository
{
    private readonly IDbConnection _connection;

    public BookRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        const string sql = @"
            SELECT 
                BookId,
                BookData.BookTitle AS BookTitle,
                BookData.Author AS Author,
                BookData.PublishDate AS PublishDate,
                BookData.Description AS Description
            FROM 
                Book
            CROSS APPLY 
                OPENJSON(BookInfo)
                WITH (
                    BookTitle NVARCHAR(255) '$.BookTitle',
                    Author NVARCHAR(255) '$.Author',
                    PublishDate NVARCHAR(50) '$.PublishDate',
                    Description NVARCHAR(MAX) '$.BookDescription'
                ) AS BookData;";

        return await _connection.QueryAsync<Book>(sql);
    }

    public async Task<IEnumerable<Book>> GetBooksByIdsAsync(int[] ids)
    {
        if (ids.Length == 0)
        {
            return Enumerable.Empty<Book>();
        }

        const string sql = @"
            SELECT
                BookId,
                BookData.BookTitle AS BookTitle,
                BookData.Author AS Author,
                BookData.PublishDate AS PublishDate,
                BookData.Description AS Description,
                CAST(BookData.CoverBase64 AS NVARCHAR(MAX)) AS CoverBase64
            FROM 
                Book
            CROSS APPLY 
                OPENJSON(BookInfo)
                WITH (
                    BookTitle NVARCHAR(255) '$.BookTitle',
                    Author NVARCHAR(255) '$.Author',
                    PublishDate NVARCHAR(50) '$.PublishDate',
                    Description NVARCHAR(MAX) '$.BookDescription',
                    CoverBase64 NVARCHAR(MAX) '$.CoverBase64'
                ) AS BookData
            WHERE BookId IN @Ids;";

        return await _connection.QueryAsync<Book>(sql, new { Ids = ids });
    }
}

