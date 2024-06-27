using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using project.Services;
using Xunit;
using Assert = Xunit.Assert;

public class CurrencyServiceTests
{
    [Fact]
    public async Task GetExchangeRateAsync_ShouldReturnCorrectRate()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("{\"table\":\"A\",\"currency\":\"dolar amerykański\",\"code\":\"USD\",\"rates\":[{\"no\":\"085/A/NBP/2023\",\"effectiveDate\":\"2023-05-05\",\"mid\":3.7845}]}")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var currencyService = new CurrencyService(httpClient);

        // Act
        var rate = await currencyService.GetExchangeRateAsync("USD");

        // Assert
        Assert.Equal(3.7845m, rate);
    }

    [Fact]
    public async Task GetExchangeRateAsync_ShouldThrowException_OnErrorResponse()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var currencyService = new CurrencyService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => currencyService.GetExchangeRateAsync("USD"));
    }
}
