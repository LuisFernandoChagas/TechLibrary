using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts;

public class RegisterBookCheckoutUseCase {
    private const int MAX_LOAN_DAYS = 7;

    private readonly LoggedUserService _loggedUser;

    public RegisterBookCheckoutUseCase(LoggedUserService loggedUser) {
        _loggedUser = loggedUser;
    }

    public void Execute(Guid bookId) {
        // Create a new instance of the DbContext
        var dbContext = new TechLibraryDbContext();

        // Validate the book
        Validate(dbContext, bookId);

        // Get the user from the database
        var user = _loggedUser.User(dbContext);

        // Register the checkout
        var entity = new Domain.Entities.Checkout {
            UserId = user.Id,
            BookId = bookId,
            ExpectedReturnedDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS)
        };

        // Add the checkout to the database
        dbContext.Checkouts.Add(entity);
        dbContext.SaveChanges();
    }

    private void Validate(TechLibraryDbContext dbContext, Guid bookId) {
        // Get the book from the database
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);

        // If the book is null, throw a NotFoundException
        if(book is null)
            throw new NotFoundException("Livro não encontrado!");

        // Get the amount of books that are not returned
        var amountBookNotReturned = dbContext.Checkouts.Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);

        // If the amount of books that are not returned is equal to the amount of books,
        // throw a ConflictException
        if(amountBookNotReturned == book.Amount)
            throw new ConflictException("Livro não está disponível para empréstimo!");
    }
}
