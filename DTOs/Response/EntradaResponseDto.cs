namespace template_csharp_dotnet.DTOs.Response
{
    public class EntradaResponseDto
    {
        public string CodigoQR { get; set; } = default!;
        public bool Verificada { get; set; }
        public int UsuarioId { get; set; }
    }
}
