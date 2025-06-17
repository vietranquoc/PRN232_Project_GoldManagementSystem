using BusinessObjects.EntityModel;
using Microsoft.Extensions.Configuration;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using System.Xml.Linq;

namespace Services.Services.Implementations
{
    public class GoldPriceService : IGoldPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly IGoldPriceRepository _goldPriceRepository;
        private readonly IGoldTypeRepository _goldTypeRepository;
        private readonly string _apiUrl; 

        public GoldPriceService(HttpClient httpClient, IGoldPriceRepository goldPriceRepository, IGoldTypeRepository goldTypeRepository, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _goldPriceRepository = goldPriceRepository;
            _goldTypeRepository = goldTypeRepository;
            _apiUrl = configuration["BTMCApi:Url"];
        }
        public async Task UpdateGoldPricesAsync()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API call failed with status code {response.StatusCode}");
            }

            var xmlContent = await response.Content.ReadAsStringAsync();
            var xmlDoc = XDocument.Parse(xmlContent);

            foreach (var row in xmlDoc.Descendants("row"))
            {
                var goldName = row.Attribute("n_1")?.Value;
                var goldType = row.Attribute("k_1")?.Value;
                var purity = row.Attribute("h_1")?.Value;
                var buyPriceChi = decimal.Parse(row.Attribute("pb_1")?.Value ?? "0");
                var sellPriceChi = decimal.Parse(row.Attribute("ps_1")?.Value ?? "0");
                var recordedAt = DateTime.ParseExact(row.Attribute("d_1")?.Value, "dd/MM/yyyy HH:mm", null);

                var buyPriceGram = buyPriceChi / 3.75m;
                var sellPriceGram = sellPriceChi / 3.75m;

                var goldTypeEntity = await _goldTypeRepository.GetByNameAsync(goldName);
                if (goldTypeEntity == null)
                {
                    goldTypeEntity = new GoldType
                    {
                        Name = goldName,
                        Description = $"{goldType} - {purity}%"
                        // CreatedBy và CreatedDate sẽ được gán trong RepositoryBase
                    };
                    _goldTypeRepository.Insert(goldTypeEntity);
                    await _goldTypeRepository.SaveChangesAsync();
                }

                var goldPrice = new GoldPrice
                {
                    GoldTypeId = goldTypeEntity.Id,
                    BuyPrice = buyPriceGram,
                    SellPrice = sellPriceGram,
                    RecordedAt = recordedAt
                    // CreatedBy và CreatedDate sẽ được gán trong RepositoryBase
                };
                _goldPriceRepository.Insert(goldPrice);
            }

            await _goldPriceRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<GoldPrice>> GetLatestGoldPricesAsync()
        {
            return await _goldPriceRepository.GetLatestPricesAsync();
        }

    }
}
