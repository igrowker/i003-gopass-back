using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Usuario usuario);
    }
}
