using CsvHelper;
using GWPApi.Models;
using System.Formats.Asn1;
using System.Globalization;

namespace GWPApi.Repositories
{
    public class GwpDataRepository : IGwpDataRepository
    {
        private readonly List<GwpData> _data;

        public GwpDataRepository()
        {
            _data = LoadDataFromCsv();
        }

        private List<GwpData> LoadDataFromCsv()
        {
            var gwpDataList = new List<GwpData>();

            // CSV file is kept in the root directory
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "gwpByCountry.csv");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The GWP data file was not found", filePath);
            }

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var gwpData = new GwpData
                    {
                        Country = csv.GetField("country"),
                        VariableId = csv.GetField("variableId"),
                        VariableName = csv.GetField("variableName"),
                        LineOfBusiness = csv.GetField("lineOfBusiness"),
                        YearData = new Dictionary<int, double?>()
                    };

                    for (int year = 2000; year <= 2015; year++)
                    {
                        var yearField = $"Y{year}";
                        // excluding years with missing values
                        if (csv.TryGetField(yearField, out string stringValue) && !string.IsNullOrEmpty(stringValue))
                        {
                            if (double.TryParse(stringValue, out double numericValue))
                            {
                                gwpData.YearData[year] = numericValue;
                            }
                        }
                    }

                    gwpDataList.Add(gwpData);
                }
            }

            return gwpDataList;
        }

        public Task<List<GwpData>> GetAll() => Task.FromResult(_data);

        public Task<List<GwpData>> GetByCountry(string country)
        {
            var result = _data.Where(d =>
                                        d.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
                                        .ToList();
            return Task.FromResult(result);
        }
    }
}
