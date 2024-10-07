using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Domain.DTOs.Request.Notification
{
    public class NotificationEmailRequestDto
    {
        public string To { get; set; }
        public string Message { get; set; }
    }
}
