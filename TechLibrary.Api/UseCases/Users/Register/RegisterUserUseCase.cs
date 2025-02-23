using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase {
    public ResponseRegisteredUserJson Execute(RequestUserJson request) {

        // Create the database context
        var dbConext = new TechLibraryDbContext();

        // Validate the request
        Validate(request, dbConext);

        // Create the cryptography algorithm
        var cryptography = new BCryptAlgorithm();

        // Create the user entity
        var entity = new User {
            Name = request.Name,
            Email = request.Email,
            Password = cryptography.HashPassword(request.Password)
        };

        // Add the user to the database
        dbConext.Users.Add(entity);
        dbConext.SaveChanges();

        var tokenGenerator = new JwtTokenGenerator();

        // Return the response
        return new ResponseRegisteredUserJson {
            Name = entity.Name,
            AccessToken = tokenGenerator.Generate(entity)
        };
    }

    private static void Validate(RequestUserJson request, TechLibraryDbContext dbContext) {

        var validator = new RegisterUserValidator();

        // Validate the request
        var validationResult = validator.Validate(request);

        // Check if the email already exists
        var existUserWithEmail = dbContext.Users.Any(user => user.Email.Equals(request.Email));

        // If the email already exists, add an error message
        if(existUserWithEmail) {
            validationResult.Errors.Add(new ValidationFailure("Email", "Email já cadastrado."));
        }

        // If the validation is not valid, throw an exception
        if(!validationResult.IsValid) {
            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
