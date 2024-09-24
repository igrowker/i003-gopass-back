using System.ComponentModel.DataAnnotations;

namespace template_csharp_dotnet.Models
{
    public class Usuario : BaseModel
    {
        [EmailAddress]
        public string Email { get; set; } = default!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        public string Nombre { get; set; } = default!;
        public string DNI { get; set; } = default!;
        public string NumeroTelefono { get; set; } = default!;
        public bool Verificado { get; set; }

        public string? Token { get; set; }

        //Navigation Properties

        public List<Entrada>? Entrada { get; set; }
        public List<Reventa>? Reventa { get; set; }
    }
}
