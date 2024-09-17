using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Services.Classes
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration["JWT:Key"]!));
        }
        public string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(signingCredentials: credentials, claims: claims, expires: DateTime.UtcNow.AddDays(5), audience: null, issuer: null);

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenHandler;
        }
    }
}
