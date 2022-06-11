using Microsoft.AspNetCore.Http;

namespace Aspnet_Upload1.Models
{

    //Relacionado com a controller em [HttpPost], [ValidateAntiForgeryToken]
    public class ClienteViewModel
    {
        public string Nome { get; set; }

        public int Idade { get; set; }

        public string Email { get; set; }

        public IFormFile Foto { get; set; }
    }
}
