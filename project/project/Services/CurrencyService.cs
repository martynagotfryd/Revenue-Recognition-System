using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace project.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://api.nbp.pl/api/exchangerates/rates/a"; // Base URL for NBP API

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetExchangeRateAsync(string targetCurrency)
        {
            var url = $"{_baseUrl}/{targetCurrency}/?format=json";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching exchange rate.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var exchangeRateResponse = JsonSerializer.Deserialize<NbpExchangeRateResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (exchangeRateResponse == null || exchangeRateResponse.Rates == null || exchangeRateResponse.Rates.Length == 0)
            {
                throw new Exception("Invalid response from exchange rate API.");
            }

            return exchangeRateResponse.Rates[0].Mid;
        }
    }

    public class NbpExchangeRateResponse
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public Rate[] Rates { get; set; }
    }

    public class Rate
    {
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Mid { get; set; }
    }
}