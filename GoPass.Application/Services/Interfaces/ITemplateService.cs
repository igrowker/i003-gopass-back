using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<string> ObtenerContenidoTemplateAsync(string nombrePlantilla, Dictionary<string, string> valoresReemplazo);
    }

}
