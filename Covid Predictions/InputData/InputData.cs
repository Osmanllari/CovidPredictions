using System.Text.Json.Serialization;

namespace Covid_Predictions.InputData
{
    public class InputData
    {
        [JsonPropertyName("iso_code")]
        public string? IsoCode { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("population")]
        public int Population { get; set; }
    }

}