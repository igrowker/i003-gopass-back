using GoPass.Application.DTOs.Request.ReventaRequestDTOs;
using GoPass.Application.DTOs.Response;
using GoPass.Domain.Models;

namespace GoPass.Application.Utilities.Mappers
{
    public static class ReventaMappers
    {
        public static Reventa FromReventaRequestToModel(this ReventaRequestDto reventaRequestDto)
        {
            return new Reventa
            {
                CompradorId = reventaRequestDto.CompradorId,
                FechaReventa = reventaRequestDto.FechaReventa,
                EntradaId = reventaRequestDto.EntradaId,    
                Precio = reventaRequestDto.Precio,
                VendedorId = reventaRequestDto.VendedorId,
            };
        }

        public static Reventa FromPublishReventaRequestToModel(this PublishReventaRequestDto publishReventaRequestDto)
        {
            return new Reventa
            {
                FechaReventa = publishReventaRequestDto.FechaReventa,
                EntradaId = publishReventaRequestDto.EntradaId,
                Precio = publishReventaRequestDto.Precio,
            };
        }

        public static ReventaResponseDto ToResponseDto(this Reventa reventa)
        {
            return new ReventaResponseDto
            {
                CompradorId = reventa.CompradorId,
                FechaReventa = reventa.FechaReventa,
                EntradaId = reventa.EntradaId,
                Precio = reventa.Precio,
                VendedorId = reventa.VendedorId,
            };
        }
    }
}
