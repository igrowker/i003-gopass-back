namespace GoPass.Domain.Models
{
    public class Entrada : BaseModel
    {
        public string CodigoQR { get; set; } = default!;
        public bool Verificada { get; set; }
        public int UsuarioId { get; set; }

        //Navigation Property

        public Usuario? Usuario { get; set; }
        public Reventa? Reventa { get; set; }

    }
}
