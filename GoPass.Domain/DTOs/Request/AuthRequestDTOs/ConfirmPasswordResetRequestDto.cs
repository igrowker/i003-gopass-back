using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Domain.DTOs.Request.AuthRequestDTOs
{
    public class ConfirmPasswordResetRequestDto
    {
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
