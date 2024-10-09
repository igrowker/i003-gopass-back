﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Services.Interfaces
{
    public interface IVonageSmsService
    {
        Task<bool> SendVonageVerificationCode(string phoneNumber);
        bool VerifyCode(int userInputCode);
    }
}
