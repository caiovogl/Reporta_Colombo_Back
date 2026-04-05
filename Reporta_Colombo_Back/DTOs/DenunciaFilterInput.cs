namespace Reporta_Colombo_Back.DTOs
{
    public class DenunciaFilterInput
    {
        //public string? PalavraChave { get; set; } = "";
        public int? Progresso { get; set; } = 0;
        public int? Tipo_Denuncia { get; set; } = 0;
        public double? Latitude { get; set; } = 0;
        public double? Longitude { get; set; } = 0;
        public double? Distancia { get; set; } = 0.01;
        public int QuantPorPagina { get; set; } = 25;
        public string? UltimoIdDocumento { get; set; }
    }
}
