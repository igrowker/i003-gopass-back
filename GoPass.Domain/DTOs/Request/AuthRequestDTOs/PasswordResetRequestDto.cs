using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Domain.DTOs.Request.AuthRequestDTOs
{
    public class PasswordResetRequestDto
    {
        public string Email { get; set; } = default!;
    }
}
