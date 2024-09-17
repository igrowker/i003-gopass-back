namespace template_csharp_dotnet.DTOs.Request.AuthRequestDTOs
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string NumeroTelefono { get; set; }
        public bool Verificado { get; set; }
    }
}
