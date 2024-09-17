using Microsoft.JSInterop.Infrastructure;
using template_csharp_dotnet.DTOs.Request.AuthRequestDTOs;
using template_csharp_dotnet.DTOs.Response.AuthResponseDTOs;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Utilities.Mappers
{
    public static class UsuarioMappers
    {
        public static Usuario ToModel(this RegisterRequestDto registerRequestDto)
        {
            return new Usuario
            {
                Email = registerRequestDto.Email,
                Password = registerRequestDto.Password,
                DNI = registerRequestDto.DNI,
                Nombre = registerRequestDto.Nombre,
                NumeroTelefono = registerRequestDto.NumeroTelefono,
                Verificado = registerRequestDto.Verificado
            };
        }

        public static UsuarioResponseDto ToResponseDto(this Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                DNI = usuario.DNI,
                Nombre = usuario.Nombre,
                NumeroTelefono = usuario.NumeroTelefono,
                Verificado = usuario.Verificado
            };
        }
    }
}
