namespace PredictionNUnitTest
{
    public class CovidTest
    {
        [Test]
        public async Task GetForecastsHistoryAsync_ReturnsValidForecasts()
        {
            // Arrange
            var service = new CovidForecastHistoryService();
            var startDate = DateTime.Today;

            // Act
            CovidForecastHistory[] forecasts = await service.GetForecastsHistoryAsync(startDate);

            // Assert
            Assert.IsNotNull(forecasts);
            Assert.AreEqual(5, forecasts.Length);

            foreach (var forecast in forecasts)
            {
                Assert.AreEqual(startDate.AddDays(forecasts.ToList().IndexOf(forecast) + 1), forecast.Date);
                Assert.IsTrue(forecast.Cases >= 100 && forecast.Cases <= 1000);
                Assert.Contains(forecast.Summary, new[] { "Low", "Medium", "High", "Critical" });
            }
        }


        [Test]
        public async Task GetPredictionsAsync_ReturnsValidResult()
        {
            // Arrange
            var httpClient = new HttpClient();
            const string apiKey = "s5GxetTYeKJeNDeR0H4lAf6yXac0ldlg";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var inputObjects = new[]
            {
                new
                {
                    iso_code = "ALB",
                    location = "Albania",
                    date = "2023-10-13 00:00:00",
                    population = 2842318
                }
            };

            var payload = JsonSerializer.Serialize(new
            {
                Inputs = new
                {
                    input1 = inputObjects
                },
                GlobalParameters = new { }
            });

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            // Act
            var response = await httpClient.PostAsync("http://20.22.70.70:80/api/v1/service/covidendpoint02/score", content);
            var result = await response.Content.ReadAsStringAsync();

            var actualPredictionResult = "0";
            try
            {
                JsonDocument jsonDoc = JsonDocument.Parse(result);
                JsonElement root = jsonDoc.RootElement;
                JsonElement resultsElement;

                if (root.TryGetProperty("Results", out resultsElement))
                {
                    JsonElement webServiceOutput0Element;
                    if (resultsElement.TryGetProperty("WebServiceOutput0", out webServiceOutput0Element) && webServiceOutput0Element.ValueKind == JsonValueKind.Array)
                    {
                        JsonElement scoredLabelsElement;
                        if (webServiceOutput0Element[0].TryGetProperty("Scored Labels", out scoredLabelsElement))
                        {
                            actualPredictionResult = scoredLabelsElement.GetRawText();
                        }

                    }
                }
            }
            catch (JsonException ex)
            {
                actualPredictionResult = $"Error deserializing response: {ex.Message}";
            }

            // Assert
            int parsedResult = int.Parse(actualPredictionResult);
            Assert.IsTrue(parsedResult >= 0);
        }

    }
}
