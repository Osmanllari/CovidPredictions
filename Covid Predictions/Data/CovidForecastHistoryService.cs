namespace Covid_Predictions.Data
{
    public class CovidForecastHistoryService
    {
        private static readonly string[] Summaries = new[]
        {
            "Low", "Medium", "High", "Critical"
        };

        public Task<CovidForecastHistory[]> GetForecastsHistoryAsync(DateTime startDate)
        {
            // Generated dummy predictions for the next 5 days
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new CovidForecastHistory
            {
                Date = startDate.AddDays(index),
                Cases = Random.Shared.Next(100, 1000),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray());
        }
    }
}
