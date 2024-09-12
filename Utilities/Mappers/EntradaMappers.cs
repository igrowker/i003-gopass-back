using template_csharp_dotnet.DTOs.Request;
using template_csharp_dotnet.DTOs.Response;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Utilities.Mappers
{
    public static class EntradaMappers
    {
        public static Entrada ToModel(this EntradaRequestDto entradaRequestDto)
        {
            return new Entrada
            {
                CodigoQR = entradaRequestDto.CodigoQR,
                UsuarioId = entradaRequestDto.UsuarioId,
                Verificada = entradaRequestDto.Verificada,
            };
        }

        public static EntradaResponseDto ToResponseDto(this Entrada entrada)
        {
            return new EntradaResponseDto
            {
                CodigoQR = entrada.CodigoQR,
                UsuarioId = entrada.UsuarioId,
                Verificada = entrada.Verificada
            };
        }
    }
}
