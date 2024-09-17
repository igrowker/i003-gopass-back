﻿using Microsoft.JSInterop.Infrastructure;
using template_csharp_dotnet.DTOs.Request;
using template_csharp_dotnet.DTOs.Response;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Utilities.Mappers
{
    public static class UsuarioMappers
    {
        public static Usuario ToModel(this UsuarioRequestDto usuarioRequestDto)
        {
            return new Usuario
            {
                DNI = usuarioRequestDto.DNI,
                Nombre = usuarioRequestDto.Nombre,
                NumeroTelefono = usuarioRequestDto.NumeroTelefono,
                Verificado = usuarioRequestDto.Verificado
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
