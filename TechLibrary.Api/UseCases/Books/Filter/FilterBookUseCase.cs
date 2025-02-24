using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.UseCases.Books.Filter;

public class FilterBookUseCase {
    public ResponseBooksJson Execute(RequestFilterBooksJson request) {
        // Create a new instance of the DbContext
        var dbContext = new TechLibraryDbContext();

        // Create a queryable object of the Books DbSet
        var query = dbContext.Books.AsQueryable();

        // If the title is not null or empty,
        // filter the query by the title
        if(!string.IsNullOrWhiteSpace(request.Title)) {
            query = query.Where(book => book.Title.Contains(request.Title));
        }

        // Skip the number of books that should be skipped
        var books = dbContext.Books
            .Where(b => request.Title == null || b.Title.Contains(request.Title))
            .Skip((request.PageNumber - 1) * 10)
            .Take(10)
            .ToList();

        var totalCount = 0;

        // If the title is not null or empty,
        // get the total count of books that contains the title
        if(!string.IsNullOrWhiteSpace(request.Title)) {
            totalCount = dbContext.Books.Count(b => b.Title.Contains(request.Title));
        } else {
            totalCount = dbContext.Books.Count();
        }

        // Create a new instance of the ResponseBooksJson
        return new ResponseBooksJson {
            Pagination = new ResponsePaginationJson{
                PageNumber = request.PageNumber,
                TotalCount = totalCount
            },
            Books = books.Select(book => new ResponseBookJson {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
            }).ToList(),
        };

    }
}
