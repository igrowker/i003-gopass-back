namespace template_csharp_dotnet.DTOs.Response
{
    public class UsuarioResponseDto
    {
        public required string Nombre { get; set; }
        public required string DNI { get; set; }
        public required string NumeroTelefono { get; set; }
        public bool Verificado { get; set; }
    }
}
