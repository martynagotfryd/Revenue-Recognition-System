using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using project.Data;
using project.Models;
using project.Services;
using Assert = Xunit.Assert;

public class PaymentServiceTests
{
    private async Task<DataBaseContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid().ToString()) // Ensure a unique database name for each test run
            .Options;

        var context = new DataBaseContext(options);
        context.Contracts.AddRange(
            new Contract
            {
                Id = 1,
                Price = 100,
                Payments = new List<Payment>
                {
                    new Payment { Id = 1, Value = 50 },
                    new Payment { Id = 2, Value = 50 }
                },
                SoftwareVersion = new SoftwareVersion { Id = 1, IdSoftware = 1 }
            },
            new Contract
            {
                Id = 2,
                Price = 200,
                Payments = new List<Payment>
                {
                    new Payment { Id = 3, Value = 100 },
                    new Payment { Id = 4, Value = 100 }
                },
                SoftwareVersion = new SoftwareVersion { Id = 2, IdSoftware = 2 }
            }
        );
        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task GetRevenue_ShouldReturnTotalRevenue()
    {
        // Arrange
        var context = await GetDbContext();
        var mockCurrencyService = new Mock<CurrencyService>(null);
        var paymentService = new DbService(context, mockCurrencyService.Object);
    
        // Act
        var revenue = await paymentService.GetRevenue(null);
    
        // Assert
        Assert.Equal(300, revenue);
    }

    [Fact]
    public async Task GetPredictedRevenue_ShouldReturnTotalPredictedRevenue()
    {
        // Arrange
        var context = await GetDbContext();
        var mockCurrencyService = new Mock<CurrencyService>(null);
        var paymentService = new DbService(context, mockCurrencyService.Object);

        // Act
        var revenue = await paymentService.GetPredictedRevenue(null);

        // Assert
        Assert.Equal(300, revenue);
    }

    // [Fact]
    // public async Task GetRevenue_ShouldReturnConvertedRevenue()
    // {
    //     // Arrange
    //     var context = await GetDbContext();
    //     var mockCurrencyService = new Mock<CurrencyService>(null);
    //     mockCurrencyService.Setup(s => s.GetExchangeRateAsync("USD")).ReturnsAsync(0.25m);
    //     var paymentService = new DbService(context, mockCurrencyService.Object);
    //
    //     // Act
    //     var revenue = await paymentService.GetRevenue(null, "USD");
    //
    //     // Assert
    //     Assert.Equal(75, revenue);
    // }

    // [Fact]
    // public async Task GetPredictedRevenue_ShouldReturnConvertedPredictedRevenue()
    // {
    //     // Arrange
    //     var context = await GetDbContext();
    //     var mockCurrencyService = new Mock<CurrencyService>(null);
    //     mockCurrencyService.Setup(s => s.GetExchangeRateAsync("USD")).ReturnsAsync(0.25m);
    //     var paymentService = new DbService(context, mockCurrencyService.Object);
    //
    //     // Act
    //     var revenue = await paymentService.GetPredictedRevenue(null, "USD");
    //
    //     // Assert
    //     Assert.Equal(75, revenue);
    // }
}
