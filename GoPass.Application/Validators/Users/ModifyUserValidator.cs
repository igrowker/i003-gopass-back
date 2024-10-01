using FluentValidation;
using GoPass.Application.Services.Interfaces;
using GoPass.Domain.DTOs.Request.AuthRequestDTOs;

namespace GoPass.Application.Validators.Users
{
    public class ModifyUserValidator : AbstractValidator<ModifyUsuarioRequestDto>
    {
        public ModifyUserValidator(IUsuarioService usuarioService)
        {
            RuleFor(u => u.DNI)
            .NotEmpty().WithMessage("El campo Dni es obligatorio")
            .Matches(@"^\d+$").WithMessage("El campo {PropertyName} solo puede contener numeros")
            .MustAsync(async (dni, _) => !await usuarioService.VerifyDniExistsAsync(dni)).WithMessage("El {PropertyName} ya se encuentra registrado");

            RuleFor(u => u.NumeroTelefono)
                .NotEmpty().WithMessage("El campo Numero de telefono es obligatorio")
                .Matches(@"^\d+$").WithMessage("El campo {PropertyName} solo puede contener numeros")
                .MustAsync(async (phoneNumber, _) => !await usuarioService.VerifyPhoneNumberExistsAsync(phoneNumber)).WithMessage("El {PropertyName} ya se encuentra registrado");

            RuleFor(u => u.Nombre)
                .NotEmpty().WithMessage("El campo es obligatorio")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El campo {PropertyName} solo puede contener letras")
                .MinimumLength(2).MaximumLength(200).WithMessage("El campo {PropertyName} debe tener un minimo de 2 caracteres");
        }
    }
}
