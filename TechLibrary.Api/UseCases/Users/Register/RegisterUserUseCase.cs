using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase {
    public ResponseRegisteredUserJson Execute(RequestUserJson request) {

        Validate(request);

        var entity = new User {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };

        var dbConext = new TechLibraryDbContext();

        dbConext.Users.Add(entity);
        dbConext.SaveChanges();

        return new ResponseRegisteredUserJson {
            Name = entity.Name,
        };
    }

    private static void Validate(RequestUserJson request) {

        var validator = new RegisterUserValidator();

        var validationResult = validator.Validate(request);

        if(!validationResult.IsValid) {
            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
