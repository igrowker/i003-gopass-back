using template_csharp_dotnet.DTOs.Request.ReventaRequestDTOs;
using template_csharp_dotnet.DTOs.Response;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Utilities.Mappers
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
                //Entrada = {
                //    UsuarioId = vendedorId
                //}
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
