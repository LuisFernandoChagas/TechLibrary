using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestUserJson>{
    public RegisterUserValidator() {

        // The name is required
        RuleFor(request => request.Name).NotEmpty().WithMessage("O nome é obrigatório");
                
        // The email is valid
        RuleFor(request => request.Email).EmailAddress().WithMessage("O email não é válido");
        
        // The password is required
        RuleFor(request => request.Password).NotEmpty().WithMessage("A senha é obrigatória");

        // If the password is not empty, check if it has more than 6 characters
        When(request => string.IsNullOrEmpty(request.Password) == false, () => {
            RuleFor(request => request.Password.Length).GreaterThanOrEqualTo(6).WithMessage("A senha deve ter mais que 6 caracteres");
        });

    }
}
