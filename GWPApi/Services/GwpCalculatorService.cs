using GWPApi.Models;
using GWPApi.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace GWPApi.Services
{
    public class GwpCalculatorService : IGwpCalculatorService
    {
        private readonly IGwpDataRepository _repository;
        private readonly IMemoryCache _cache;

        public GwpCalculatorService(IGwpDataRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<GwpResponse> CalculateAverage(GwpRequest request)
        {
            var cacheKey = $"{request.Country}:{string.Join(",", request.Lob.OrderBy(x => x))}";

            if (_cache.TryGetValue(cacheKey, out GwpResponse cachedResponse))
            {
                return cachedResponse;
            }

            var countryData = await _repository.GetByCountry(request.Country);
            var response = new GwpResponse();

            foreach (var lob in request.Lob)
            {
                var lobData = countryData.FirstOrDefault(d =>
                                          d.LineOfBusiness.Equals(lob, StringComparison.OrdinalIgnoreCase));

                // exclude missing years from calculation
                if (lobData == null)
                {
                    continue;
                }
                
                var relevantYears = lobData.YearData
                                           .Where(kv => kv.Key >= 2008 && kv.Key <= 2015 && kv.Value.HasValue)
                                           .Select(kv => kv.Value.Value)
                                           .ToList();

                if (relevantYears.Any())
                {
                    var average = relevantYears.Average();
                    response.Add(lob, average);
                }
            }

            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10));
            return response;
        }
        
    }
}
