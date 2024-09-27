namespace template_csharp_dotnet.DTOs.Request.AuthRequestDTOs
{
    public class ModifyUsuarioRequestDto
    {
        public string Nombre { get; set; } = default!;
        public string DNI { get; set; } = default!;
        public string NumeroTelefono { get; set; } = default!;
    }
}
