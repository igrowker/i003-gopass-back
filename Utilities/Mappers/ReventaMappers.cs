using template_csharp_dotnet.DTOs.Request;
using template_csharp_dotnet.DTOs.Response;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Utilities.Mappers
{
    public static class ReventaMappers
    {
        public static Reventa ToModel(this ReventaRequestDto reventaRequestDto)
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
