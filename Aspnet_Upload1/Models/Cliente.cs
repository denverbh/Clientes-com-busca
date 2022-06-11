using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Aspnet_Upload1.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe a idade")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "Informe o email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Selecione uma imagem")]
        public string Foto { get; set; }
    }
}
