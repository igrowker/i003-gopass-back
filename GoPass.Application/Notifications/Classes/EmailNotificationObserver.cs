using GoPass.Application.Notifications.Interfaces;
using GoPass.Application.Services.Interfaces;
using GoPass.Domain.DTOs.Request.Notification;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Notifications.Classes
{
    public class EmailNotificationObserver : Interfaces.IObserver<string>
    {
        private readonly IEmailService _emailService;

        public EmailNotificationObserver(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Update(string data)
        {
            NotificationEmailRequestDto notification = new();
            if (notification != null)
            {
                await _emailService.SendNotificationEmailAsync(notification);
            }
        }
    }
}
