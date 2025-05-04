namespace GWPApi.Models
{
    public class GwpData
    {
        public string Country { get; set; }
        public string VariableId { get; set; }
        public string VariableName { get; set; }
        public string LineOfBusiness { get; set; }
        public Dictionary<int, double?> YearData { get; set; } = new Dictionary<int, double?>();
    }
}
