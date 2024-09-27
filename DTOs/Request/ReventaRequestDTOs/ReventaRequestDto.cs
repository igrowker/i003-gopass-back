﻿namespace template_csharp_dotnet.DTOs.Request.ReventaRequestDTOs
{
    public class ReventaRequestDto
    {
        public int EntradaId { get; set; }
        public int VendedorId { get; set; }
        public int CompradorId { get; set; }
        public DateTime FechaReventa { get; set; }
        public decimal Precio { get; set; }
    }
}
