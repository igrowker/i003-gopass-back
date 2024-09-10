namespace template_csharp_dotnet.Models
{
    public class Entrada : BaseModel
    {
        public string CodigoQR { get; set; }
        public bool Verificada { get; set; }
        public int UsuarioId { get; set; }

        //Navigation Property

    }
}
