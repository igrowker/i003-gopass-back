using template_csharp_dotnet.DTOs.Request.ReventaRequestDTOs;
using template_csharp_dotnet.DTOs.Response;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Utilities.Mappers
{
    public static class EntradaMappers
    {
        public static Entrada FromEntradaRequestToModel(this EntradaRequestDto entradaRequestDto)
        {
            return new Entrada
            {
                CodigoQR = entradaRequestDto.CodigoQR,
                UsuarioId = entradaRequestDto.UsuarioId,
                Verificada = entradaRequestDto.Verificada,
            };
        }

        public static Entrada FromPublishEntradaRequestToModel(this PublishEntradaRequestDto publishEntradaRequestDto)
        {
            return new Entrada
            {
                CodigoQR = publishEntradaRequestDto.CodigoQR,
                Verificada = publishEntradaRequestDto.Verificada
            };
        }

        public static EntradaResponseDto FromEntradaRequestToResponseDto(this Entrada entrada)
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
