using Microsoft.JSInterop.Infrastructure;
using template_csharp_dotnet.DTOs.Request.AuthRequestDTOs;
using template_csharp_dotnet.DTOs.Response.AuthResponseDTOs;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Utilities.Mappers
{
    public static class UsuarioMappers
    {
        public static Usuario FromRegisterToModel(this RegisterRequestDto registerRequestDto)
        {
            return new Usuario
            {
                Email = registerRequestDto.Email,
                Password = registerRequestDto.Password
            };
        }

        public static Usuario FromLoginToModel(this LoginRequestDto loginRequestDto)
        {
            return new Usuario
            {
                Email = loginRequestDto.Email,
                Password = loginRequestDto.Password
            };
        }

        public static LoginResponseDto FromModelToLoginResponse(this Usuario usuario)
        {
            return new LoginResponseDto
            {
                Email = usuario.Email,
                Token = usuario.Token!
            };
        }

        public static Usuario FromModifyUsuarioRequestToModel(this ModifyUsuarioRequestDto modifyUsuarioRequestDto, Usuario existingData)
        {
            existingData.Nombre = modifyUsuarioRequestDto.Nombre;
            existingData.DNI = modifyUsuarioRequestDto.DNI;
            existingData.NumeroTelefono = modifyUsuarioRequestDto.NumeroTelefono;
            return existingData;
        }

        //public static UsuarioResponseDto ToResponseDto(this Usuario usuario)
        //{
        //    return new UsuarioResponseDto
        //    {
        //        DNI = usuario.DNI,
        //        Nombre = usuario.Nombre,
        //        NumeroTelefono = usuario.NumeroTelefono,
        //        Verificado = usuario.Verificado
        //    };
        //}
    }
}
