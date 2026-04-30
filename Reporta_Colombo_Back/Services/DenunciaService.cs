using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Reporta_Colombo_Back.Controllers;
using Reporta_Colombo_Back.DTOs;
using Reporta_Colombo_Back.Models;
using Reporta_Colombo_Back.Services.Interfaces;
using System.Collections;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace Reporta_Colombo_Back.Services
{
    public class DenunciaService : IDenunciaService
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName = "reporta-colombo.firebasestorage.app";

        public DenunciaService(FirestoreDb firestoreDb, StorageClient storageClient)
        {
            _firestoreDb = firestoreDb;
            _storageClient = storageClient;
        }

        public async Task<DenunciasModel> AddDenunciaAsync(DenunciaDto denunciaDto)
        {
            string imageUrl = await UploadImageToStorage(denunciaDto.Imagem);

            if (string.IsNullOrEmpty(imageUrl))
            {
                return null;
            }

            int tipoDenuncia = 0;
            if (Enum.TryParse(denunciaDto.Tipo_Denuncia, out TipoDenunciaEnum resultado))
            {
                tipoDenuncia = (int)resultado - 1;
            }

            var denunciaData = new DenunciasModel
            {
                Des_Denuncia = denunciaDto.Descricao,
                Latitude = denunciaDto.Latitude,
                Longitude = denunciaDto.Longitude,
                Url_Imagem = imageUrl, 
                Tipo_Denuncia = tipoDenuncia,
                Dta_Cadastro = DateTime.UtcNow,
                Flg_Progresso = 0
            };

            var collection = _firestoreDb.Collection("denuncias");
            await collection.AddAsync(denunciaData);

            return denunciaData;
        }

        public async Task<object> GetDenunciasFiltered(DenunciaFilterInput filter)
        {
            Query query = _firestoreDb.Collection("denuncias");

            if (filter.Progresso != 0)
            {
                query = query.WhereEqualTo("Flg_Progresso", filter.Progresso);
            }

            if (filter.Tipo_Denuncia != 0)
            {
                query = query.WhereEqualTo("Tipo_Denuncia", filter.Tipo_Denuncia);
            }

            if (filter.Latitude != 0 && filter.Longitude != 0)
            {
                query = query.WhereGreaterThanOrEqualTo("Latitude", filter.Latitude - filter.Distancia)
                    .WhereLessThanOrEqualTo("Latitude", filter.Latitude + filter.Distancia)
                    .WhereGreaterThanOrEqualTo("Longitude", filter.Longitude - filter.Distancia)
                    .WhereLessThanOrEqualTo("Longitude", filter.Longitude - filter.Distancia);
            }

            query = query.OrderByDescending("Dta_Cadastro");

            if (!string.IsNullOrEmpty(filter.UltimoIdDocumento))
            {
                DocumentSnapshot snapshot = await _firestoreDb.Collection("denuncias")
                                                              .Document(filter.UltimoIdDocumento)
                                                              .GetSnapshotAsync();
                query = query.StartAfter(snapshot);
            }

            query = query.Limit(filter.QuantPorPagina);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            var resultados = querySnapshot.Documents.Select(d => {
                var denuncia = d.ConvertTo<DenunciasModel>();

                return new
                {
                    Id = d.Id,
                    Des_Denuncia = denuncia.Des_Denuncia,
                    Dta_Cadastro = denuncia.Dta_Cadastro,
                    Flg_Progresso = denuncia.Flg_Progresso,
                    Tipo_Denuncia = (TipoDenunciaEnum)denuncia.Tipo_Denuncia,
                    Url_Imagem = denuncia.Url_Imagem,
                    Latitude = denuncia.Latitude,
                    Longitude = denuncia.Longitude
                };
        }).ToList();

            return resultados;
        }

        private async Task<string> UploadImageToStorage(IFormFile imageFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                string fileName = $"denuncias/{Guid.NewGuid()}_{imageFile.FileName}";

                var imageObject = await _storageClient.UploadObjectAsync(
                    _bucketName,
                    fileName,
                    imageFile.ContentType,
                    memoryStream);

                return $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(fileName)}?alt=media";
            }
        }
    }
}
