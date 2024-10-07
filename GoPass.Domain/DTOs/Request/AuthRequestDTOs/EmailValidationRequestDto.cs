using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Domain.DTOs.Request.AuthRequestDTOs
{
    public class EmailValidationRequestDto
    {
        public string To { get; set; } = default!; 
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;
    }
}
