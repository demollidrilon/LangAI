using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using LangAI.Domain.Entities;
using LangAI.Application.DTOs;
using LangAI.Application.Options;
using LangAI.Infrastructure.Services;
using Moq;
using Moq.Protected;
using FluentAssertions;

namespace LangAI.Tests;
public class TranslationServiceTests
{
    private TranslationService CreateServiceWithMockedResponse(string jsonResponse)
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse),
            });

        var client = new HttpClient(mockHandler.Object);

        var options = Options.Create(new OpenAIOptions
        {
            URL = "http://fake-url",
            GTPModel = "gpt-3.5-turbo",
            Temperature = 0,
            MaxTokens = 256,
            SecretKey = "fake-key"
        });

        var mockCache = new Mock<IDistributedCache>();
        mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((byte[])null);

        var mockLogger = new Mock<ILogger<TranslationService>>();

        return new TranslationService(client, options, mockCache.Object, mockLogger.Object);
    }

    [Fact]
    public async Task TranslateAsync_ReturnsTranslatedText_WhenOpenAiResponds()
    {
        // Arrange
        var expectedText = "Bonjour le monde!";
        var responseObject = new TranslationResponseDTO
        {
            Choices = new List<Choice>
            {
                new Choice
                {
                    Message = new Message
                    {
                        Content = expectedText
                    }
                }
            }
        };

        var service = CreateServiceWithMockedResponse(JsonConvert.SerializeObject(responseObject));

        // Act
        var result = await service.TranslateAsync("Hello world", "French");

        // Assert
        result.Should().Be(expectedText);
    }

    [Fact]
    public async Task TranslateAsync_ReturnsFallbackMessage_WhenOpenAiFails()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var client = new HttpClient(mockHandler.Object);

        var options = Options.Create(new OpenAIOptions
        {
            URL = "http://fake-url",
            GTPModel = "gpt-3.5-turbo",
            Temperature = 0,
            MaxTokens = 256,
            SecretKey = "fake-key"
        });

        var mockCache = new Mock<IDistributedCache>();
        mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((byte[])null);

        var mockLogger = new Mock<ILogger<TranslationService>>();

        var service = new TranslationService(client, options, mockCache.Object, mockLogger.Object);

        // Act
        var result = await service.TranslateAsync("Hello world", "French");

        // Assert
        result.Should().Be("Translation failed. Please try again later.");
    }
}