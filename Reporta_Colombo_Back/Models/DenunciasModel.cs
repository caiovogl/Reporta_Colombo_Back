using Google.Cloud.Firestore;

namespace Reporta_Colombo_Back.Models
{
    [FirestoreData]
    public class DenunciasModel
    {
        [FirestoreProperty]
        public string Des_Denuncia { get; set; }

        [FirestoreProperty]
        public double Latitude { get; set; }

        [FirestoreProperty]
        public double Longitude { get; set; }

        [FirestoreProperty]
        public string Url_Imagem { get; set; }
        [FirestoreProperty]
        public int Flg_Progresso { get; set; }
        [FirestoreProperty]
        public int Tipo_Denuncia { get; set; }

        [FirestoreProperty]
        public DateTime Dta_Cadastro { get; set; } = DateTime.UtcNow;
    }
}
