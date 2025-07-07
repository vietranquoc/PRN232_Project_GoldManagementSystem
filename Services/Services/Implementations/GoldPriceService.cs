using BusinessObjects.DTOs;
using BusinessObjects.EntityModel;
using Microsoft.Extensions.Configuration;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;

namespace Services.Services.Implementations
{
    public class GoldPriceService : IGoldPriceService
    {
        private readonly IGoldPriceRepository _goldPriceRepository;
        private readonly IGoldTypeRepository _goldTypeRepository;

        public GoldPriceService(IGoldPriceRepository goldPriceRepository, IGoldTypeRepository goldTypeRepository)
        {
            _goldPriceRepository = goldPriceRepository;
            _goldTypeRepository = goldTypeRepository;
        }

        public async Task AddManualGoldPriceAsync(ManualGoldPriceDTO dto)
        {
            await SaveGoldPriceAsync(
                name: dto.Name,
                karat: dto.Karat,
                priceType: dto.PriceType,
                description: dto.Description,
                buy: dto.BuyPrice,
                sell: dto.SellPrice,
                recordedAt: dto.RecordedAt ?? DateTime.Now
            );
        }

        private async Task SaveGoldPriceAsync(string name, int? karat, string priceType, string description, decimal buy, decimal sell, DateTime recordedAt)
        {
            try
            {
                var goldType = await _goldTypeRepository.GetByConditionsAsync(name, karat, priceType);
                if (goldType == null)
                {
                    goldType = new BusinessObjects.EntityModel.GoldType
                    {
                        Name = name,
                        Karat = karat,
                        PriceType = priceType,
                        Description = description
                    };
                    _goldTypeRepository.Insert(goldType);
                    await _goldTypeRepository.SaveChangesAsync();
                    Console.WriteLine($"Inserted GoldType: Name={name}, Karat={karat}, PriceType={priceType}");
                }

                var goldPrice = new BusinessObjects.EntityModel.GoldPrice
                {
                    GoldTypeId = goldType.Id,
                    BuyPrice = buy,
                    SellPrice = sell,
                    RecordedAt = recordedAt
                };

                _goldPriceRepository.Insert(goldPrice);
                await _goldPriceRepository.SaveChangesAsync();
                Console.WriteLine($"Inserted GoldPrice: GoldTypeId={goldType.Id}, Buy={buy}, Sell={sell}, Time={recordedAt}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving gold price: {ex}");
                throw;
            }
        }

        public async Task<IEnumerable<GoldPriceResponseDTO>> GetLatestGoldPricesAsync()
        {
            var latestPrices = await _goldPriceRepository.GetLatestPricesAsync();

            return latestPrices.Select(gp => new GoldPriceResponseDTO
            {
                GoldTypeId = gp.GoldTypeId,
                GoldTypeName = gp.GoldType?.Name ?? "",
                Description = gp.GoldType?.Description ?? "",
                BuyPrice = gp.BuyPrice,
                SellPrice = gp.SellPrice,
                RecordedAt = gp.RecordedAt
            });
        }
    }
}
