using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using LangAI.Application.DTOs;
using LangAI.Application.Options;
using LangAI.Application.Interfaces;

namespace LangAI.Infrastructure.Services;
public class TranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;
    private readonly OpenAIOptions _options;
    private readonly IDistributedCache _cache;
    private readonly ILogger<TranslationService> _logger;

    public TranslationService(HttpClient httpClient, IOptions<OpenAIOptions> options, IDistributedCache cache, ILogger<TranslationService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task<string> TranslateAsync(string text, string targetLanguage)
    {
        var cacheKey = $"{text}|{targetLanguage}";
        string cached = null;

        try
        {
            cached = await _cache.GetStringAsync(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis read failed for key {CacheKey}", cacheKey);
        }

        if (!string.IsNullOrEmpty(cached))
            return cached;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.SecretKey);

        var requestPayload = new
        {
            model = _options.GTPModel,
            messages = new[]
            {
                    new { role = "system", content = $"You are a professional translator. Translate the following text to {targetLanguage}." },
                    new { role = "user", content = text }
            },
            temperature = _options.Temperature,
            max_tokens = _options.MaxTokens
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_options.URL, content);

        if (!response.IsSuccessStatusCode)
            return "Translation failed. Please try again later.";

        var result = JsonConvert.DeserializeObject<TranslationResponseDTO>(await response.Content.ReadAsStringAsync());

        var translatedText = result?.Choices?[0]?.Message?.Content ?? "Translation not available.";

        try
        {
            await _cache.SetStringAsync(cacheKey, translatedText, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis write failed for key {CacheKey}", cacheKey);
        }

        return translatedText;
    }
}