namespace template_csharp_dotnet.Models
{
    public class Usuario : BaseModel
    {
        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string NumeroTelefono { get; set; }
        public bool Verificado { get; set; }   
    }
}
