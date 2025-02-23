using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase {
    public ResponseRegisteredUserJson Execute(RequestUserJson request) {
        var dbConext = new TechLibraryDbContext();
        Validate(request, dbConext);

        var cryptography = new BCryptAlgorithm();

        var entity = new User {
            Name = request.Name,
            Email = request.Email,
            Password = cryptography.HashPassword(request.Password)
        };

        dbConext.Users.Add(entity);
        dbConext.SaveChanges();

        return new ResponseRegisteredUserJson {
            Name = entity.Name,
        };
    }

    private static void Validate(RequestUserJson request, TechLibraryDbContext dbContext) {

        var validator = new RegisterUserValidator();

        var validationResult = validator.Validate(request);

        var existUserWithEmail = dbContext.Users.Any(user => user.Email.Equals(request.Email));

        if(existUserWithEmail) {
            validationResult.Errors.Add(new ValidationFailure("Email", "Email já cadastrado."));
        }

        if(!validationResult.IsValid) {
            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
