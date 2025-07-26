using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly IGoldPriceService _goldPriceService;
        private static decimal? _cachedPrice;
        private static DateTime _lastFetch;

        public ChatBotController(HttpClient http, IConfiguration config, IMemoryCache cache, IGoldPriceService goldPriceService)
        {
            _http = http;
            _config = config;
            _cache = cache;
            _goldPriceService = goldPriceService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] ChatRequest request)
        {
            string apiKey = _config["OpenAI:ApiKey"];
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            string cacheKey = HttpContext.Connection.RemoteIpAddress.ToString();

            var goldPrice = await GetCachedGoldPrice();
            var systemPrompt = $"""
                <instruction>
                    Your name is Gold Bot.
                    You are a helpful and knowledgeable financial assistant.
                    Your mission is to help users decide whether they should buy or sell their valuables, such as gold, based on current market conditions.
                    Keep your answers concise, informative, and actionable.
                    ❗ Do not include detailed mathematical calculations unless the user specifically asks you to, just give user the result.
                    For example, do not show conversions or multiplications unless the prompt includes words like "calculate".
                    Keep responses short and focused unless asked to elaborate.
                </instruction>

                <data>
                    - Current gold price: {goldPrice} USD per troy ounce. Use this when the user asks for the current or today's gold price.
    
                    Gold unit conversions:
                    • 1 troy ounce = 31.1035 grams  
                    • 1 kilogram = 32.1507 troy ounces  
                    • 1 gram = 0.03215 troy ounces  
                    • 1 chỉ (Vietnamese unit) = 3.75 grams = 0.1204 troy ounces  
                    • 1 cây/lượng (Vietnamese unit) = 37.5 grams = 1.204 troy ounces  
    
                    Currency exchange rate:
                    - 1 USD = 26,110 VND
                </data>

                <example>
                    User: What is the current price for gold today?
                    Assistant: As of {DateTime.Now.ToString("MMMM dd, yyyy HH:mm")}, the current gold price is {goldPrice} USD per troy ounce.
                </example>

                <example>
                    User: How much is 1 USD in VND?
                    Assistant: At the current exchange rate, 1 USD equals 26,110 VND.
                </example>
                """;




            var messages = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                return new List<object>
                    {
                        new { role = "system", content = systemPrompt }
                    };
            });

            messages.Add(new { role = "user", content = request.Message });

            var payload = new { model = "gpt-3.5-turbo", messages = messages };
            var json = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("https://api.openai.com/v1/chat/completions", json);
            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            var reply = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            messages.Add(new { role = "assistant", content = reply });

            _cache.Set(cacheKey, messages, TimeSpan.FromMinutes(30));

            return Ok(new { reply });
        }

        public async Task<decimal?> GetCachedGoldPrice()
        {
            if (_cachedPrice.HasValue && DateTime.Now - _lastFetch < TimeSpan.FromMinutes(30))
                return _cachedPrice;

            _cachedPrice = await _goldPriceService.GetGoldPriceInVNDAsync();
            _lastFetch = DateTime.Now;
            return _cachedPrice;
        }

        public class ChatRequest
        {
            public string Message { get; set; }
        }
    }
}
