using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Domain.Models
{
    public class Correo
    {
        public string Para { get; set; } = default!;
        public string Asunto { get; set; } = default!;
        public string Contenido { get; set; } = default!;
    }
}
