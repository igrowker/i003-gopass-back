namespace template_csharp_dotnet.DTOs.Request.AuthRequestDTOs
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public string DNI { get; set; } = default!;
        public string NumeroTelefono { get; set; } = default!;
        public bool Verificado { get; set; }
    }
}
