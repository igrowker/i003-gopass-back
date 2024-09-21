using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.DTOs.Request.ReventaRequestDTOs
{
    public class PublishReventaRequestDto
    {
        public int EntradaId { get; set; }
        public DateTime FechaReventa { get; set; }
        public decimal Precio { get; set; }

        //public Entrada? Entrada { get; set; }
    }
}
