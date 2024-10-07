using GoPass.Application.Notifications.Interfaces;
using GoPass.Application.Services.Interfaces;
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

        public async Task Update(string notification)
        {
            var emailMessage = notification!.ToString();
           await _emailService.SendNotificationEmail(emailMessage);
        }
    }
}
