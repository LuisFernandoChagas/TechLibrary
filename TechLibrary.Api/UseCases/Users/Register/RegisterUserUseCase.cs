using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase {
    public ResponseRegisteredUserJson Execute(RequestUserJson request) {

        Validate(request);

        return new ResponseRegisteredUserJson {

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
