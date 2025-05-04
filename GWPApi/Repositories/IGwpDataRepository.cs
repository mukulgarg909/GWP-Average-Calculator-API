using GWPApi.Models;

namespace GWPApi.Repositories
{
    public interface IGwpDataRepository
    {
        Task<List<GwpData>> GetAll();
        Task<List<GwpData>> GetByCountry(string country);
    }
}
