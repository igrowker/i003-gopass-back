using GoPass.Domain.DTOs.Request.AuthRequestDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendVerificationEmailAsync(EmailValidationRequestDto emailValidationRequestDto);
    }
}
