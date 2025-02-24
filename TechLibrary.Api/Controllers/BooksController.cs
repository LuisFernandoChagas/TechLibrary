using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Books.Filter;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BooksController : ControllerBase {

    [HttpGet("Filter")]
    [ProducesResponseType(typeof(ResponseBooksJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Filter(int pageNumber, string? title) {
        var useCase = new FilterBookUseCase();

        var request = new RequestFilterBooksJson {
            PageNumber = pageNumber,
            Title = title
        };

        var result = useCase.Execute(request);

        // If there are no books, return a 204 No Content
        if(result.Books.Count == 0) {
            return NoContent();
        }

        return Ok(result);
    }

}
