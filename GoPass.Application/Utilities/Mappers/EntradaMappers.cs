using GoPass.Application.DTOs.Request.ReventaRequestDTOs;
using GoPass.Application.DTOs.Response;
using GoPass.Domain.Models;

namespace GoPass.Application.Utilities.Mappers
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

        public static Reventa FromBuyEntradaRequestToModel(this BuyEntradaRequestDto buyEntradaRequestDto)
        {
            return new Reventa
            {
                EntradaId = buyEntradaRequestDto.EntradaId
            };
        }

        public static EntradaResponseDto FromPublishEntradaRequestToResponseDto(this Entrada entrada)
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
