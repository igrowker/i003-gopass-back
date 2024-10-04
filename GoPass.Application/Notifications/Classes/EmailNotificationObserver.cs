using GoPass.Application.Notifications.Interfaces;
using GoPass.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Notifications.Classes
{
    public class EmailNotificationObserver : IGenericObserver<string>
    {
        private readonly IEmailService _emailService;
        public EmailNotificationObserver(IEmailService emailservice)
        {
            _emailService = emailservice;
        }
        //public void Update(string message)
        //{
        //    //_emailService.SendEmail(message);
        //}
    }
}
