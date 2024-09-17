namespace template_csharp_dotnet.Models
{
    public class Reventa : BaseModel
    {
        public int EntradaId { get; set; }
        public int VendedorId { get; set; }
        public int CompradorId { get; set; }
        public DateTime FechaReventa { get; set; }
        public decimal Precio { get; set; } 

        //Navigation Properties
        public Usuario Vendedor { get; set; }
        public Usuario Comprador { get; set; }
        public Entrada Entrada { get; set; }
    }
}
