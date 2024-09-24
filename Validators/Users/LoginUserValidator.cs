using FluentValidation;
using template_csharp_dotnet.DTOs.Request.AuthRequestDTOs;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Validators.Users
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
