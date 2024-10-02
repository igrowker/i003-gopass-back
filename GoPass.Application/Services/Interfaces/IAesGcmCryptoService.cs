using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Services.Interfaces
{
    public interface IAesGcmCryptoService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
