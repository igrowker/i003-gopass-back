using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Domain.DTOs.Response.ReventaResponseDTOs
{
    public class PublishReventaResponseDto
    {
        public int EntradaId { get; set; }
        public string ResaleDetail { get; set; } = default!;
        public decimal Precio { get; set; }
        public DateTime FechaReventa { get; set; }
    }
}
