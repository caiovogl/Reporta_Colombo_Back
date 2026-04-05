using System.ComponentModel.DataAnnotations;

namespace Reporta_Colombo_Back.DTOs
{
    public class DenunciaDto
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A latitude é obrigatória.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "A longitude é obrigatória.")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "O tipo de denúncia é obrigatório.")]
        public string Tipo_Denuncia { get; set; }

        [Required(ErrorMessage = "A imagem é obrigatória.")]
        public IFormFile Imagem { get; set; }
    }
}
