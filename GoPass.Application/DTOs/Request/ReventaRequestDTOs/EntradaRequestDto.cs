
namespace GoPass.Application.DTOs.Request.ReventaRequestDTOs
{
    public class EntradaRequestDto
    {
        public string CodigoQR { get; set; } = default!;
        public bool Verificada { get; set; }
        public int UsuarioId { get; set; }
    }
}
