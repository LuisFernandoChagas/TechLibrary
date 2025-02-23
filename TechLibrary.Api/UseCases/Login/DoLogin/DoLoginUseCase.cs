using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login.DoLogin;

public class DoLoginUseCase {
    public ResponseRegisteredUserJson Execute(RequestLoginJson request) {
        // Create the database context
        var dbContext = new TechLibraryDbContext();

        // Check if the user exists
        // If not, throw an exception
        var entity = dbContext.Users.FirstOrDefault(user => user.Email.Equals(request.Email));

        if(entity is null) {
            throw new InvalidLoginException();
        }

        // Check if the password is valid
        var cryptography = new BCryptAlgorithm();
        var passwordIsValid = cryptography.VerifyPassword(request.Password, entity);
        if(!passwordIsValid)             throw new InvalidLoginException();

        // Generate the token
        var tokenGenerator = new JwtTokenGenerator();
        return new ResponseRegisteredUserJson {
            Name = entity.Name,
            AccessToken = tokenGenerator.Generate(entity)
        };
    }
}
