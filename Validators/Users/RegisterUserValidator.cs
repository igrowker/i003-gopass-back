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

            RuleFor(u => u.DNI)
                .NotEmpty().WithMessage("El campo Dni es obligatorio")
                .MustAsync(async (dni, _) => !await usuarioService.VerifyDniExistsAsync(dni)).WithMessage("El {PropertyName} ya se encuentra registrado");

            RuleFor(u => u.NumeroTelefono)
                .NotEmpty().WithMessage("El campo Numero de telefono es obligatorio")
                .Matches(@"^\d+$").WithMessage("El campo {PropertyName} solo puede contener numeros")
                .MustAsync(async (phoneNumber, _) => !await usuarioService.VerifyPhoneNumberExistsAsync(phoneNumber)).WithMessage("El {PropertyName} ya se encuentra registrado");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("El campo {PropertyName} es obligatorio")
                .MinimumLength(8).MaximumLength(25);

            RuleFor(u => u.Nombre)
                .NotEmpty().WithMessage("El campo es obligatorio")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El campo {PropertyName} solo puede contener letras")
                .MinimumLength(2).MaximumLength(200).WithMessage("El campo {PropertyName} debe tener un minimo de 2 caracteres");

        }
    }
}
