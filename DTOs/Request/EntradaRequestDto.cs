using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.DTOs.Request
{
    public class EntradaRequestDto
    {
        public string CodigoQR { get; set; }
        public bool Verificada { get; set; }
        public int UsuarioId { get; set; }

        //Navigation Property

        //   public Usuario Usuario { get; set; }
        //   public Reventa Reventa { get; set; }
    }
}
