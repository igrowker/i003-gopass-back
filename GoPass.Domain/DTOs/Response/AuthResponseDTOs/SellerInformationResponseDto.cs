using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Domain.DTOs.Response.AuthResponseDTOs
{
    public class SellerInformationResponseDto
    {
        public string Nombre { get; set; } = default!;
        public string Image { get; set; } = default!;
    }
}
