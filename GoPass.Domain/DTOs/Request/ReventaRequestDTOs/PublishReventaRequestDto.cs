
namespace GoPass.Domain.DTOs.Request.ReventaRequestDTOs
{
    public class PublishReventaRequestDto
    {
        public int EntradaId { get; set; }
        public string ResaleDetail { get; set; } = default!;
        public decimal Precio { get; set; }
    }
}
