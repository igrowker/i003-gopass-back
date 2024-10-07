using GoPass.Domain.DTOs.Request.AuthRequestDTOs;
using GoPass.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Utilities.Mappers
{
    public static class EmailMappers
    {
        public static EmailValidationRequestDto AssignEmailValues(this EmailValidationRequestDto emailValidationRequestDto, string usuarioEmail,
            string emailSubject, string emailBody)
        {
            return new EmailValidationRequestDto
            {
                To = usuarioEmail,
                Subject = emailSubject,
                Body = emailBody
            };
        }
    }
}
