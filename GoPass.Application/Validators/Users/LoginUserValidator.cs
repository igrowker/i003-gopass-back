using FluentValidation;
using GoPass.Application.DTOs.Request.AuthRequestDTOs;
using GoPass.Application.Services.Interfaces;

namespace GoPass.Application.Validators.Users
{
    public class LoginUserValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginUserValidator(IUsuarioService usuarioService)
        {
            RuleFor(u => u.Email).NotEmpty().WithMessage("El campo {PropertyName} No puede ser vacio ya que debe coincidir con las credenciales para loguearse");

            RuleFor(u => u.Password).NotEmpty().WithMessage("El campo {PropertyName} No puede ser vacio ya que debe coincidir con las credenciales para loguearse");
        }
    }
}
