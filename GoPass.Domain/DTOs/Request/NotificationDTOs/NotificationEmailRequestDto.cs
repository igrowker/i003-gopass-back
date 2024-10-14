using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Domain.DTOs.Request.NotificationDTOs
{
    public class NotificationEmailRequestDto
    {
        public string To { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
        public string TicketQrCode { get; set; }
    }
}
