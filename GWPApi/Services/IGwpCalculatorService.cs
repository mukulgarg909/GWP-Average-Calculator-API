using GWPApi.Models;

namespace GWPApi.Services
{
    public interface IGwpCalculatorService
    {
        Task<GwpResponse> CalculateAverage(GwpRequest request);
    }
}
