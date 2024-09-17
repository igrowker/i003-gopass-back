using System.ComponentModel.DataAnnotations;

namespace template_csharp_dotnet.Models
{
    public class Usuario : BaseModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string NumeroTelefono { get; set; }
        public bool Verificado { get; set; }

        //Navigation Properties

        public Entrada Entrada { get; set; }
        public Reventa Reventa { get; set; }
    }
}
