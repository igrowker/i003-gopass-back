using FluentValidation;
using template_csharp_dotnet.DTOs.Request.AuthRequestDTOs;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Validators.Users
{
    public class RegisterUserValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterUserValidator(IUsuarioService usuarioService)
        {
            RuleFor(u => u.Email).EmailAddress()
                .NotEmpty().WithMessage("El campo email es obligatorio")
                .MustAsync(async (email, _) => !await usuarioService.VerifyEmailExistsAsync(email)).WithMessage("El {PropertyName} ya se encuentra registrado en nuestra base de datos");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("El campo {PropertyName} es obligatorio")
                .MinimumLength(8).MaximumLength(25);
        }
    }
}
