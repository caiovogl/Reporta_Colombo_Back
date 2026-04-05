using Reporta_Colombo_Back.DTOs;
using Reporta_Colombo_Back.Models;

namespace Reporta_Colombo_Back.Services.Interfaces
{
    public interface IDenunciaService
    {
        //usuário comum
        Task<DenunciasModel> AddDenunciaAsync(DenunciaDto denunciaDto);
        Task<object> GetDenunciasFiltered(DenunciaFilterInput filter);
    }
}
