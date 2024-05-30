using Moq;
using TestCore.Business;
using TestCore.Data.Entities;
using TestCore.Data.Models;
using TestCore.Data;
using Microsoft.EntityFrameworkCore;

[TestFixture]
public class RentServicesShould
{
    private RentServices _rentServices;
    private Mock<ICoreDataContext> _mockContext;

    public RentServicesShould()
    {
        _mockContext = new Mock<ICoreDataContext>();
        _rentServices = new RentServices(_mockContext.Object);
    }

    [TestCase("05-03-2022", "05-08-2022", "05-01-2022", "05-06-2022")]
    public void Should_return_false_when_dates_overlap(
        DateTime newDeliveryDate, DateTime newReturnDate,
        DateTime oldDeliveryDate, DateTime oldReturnDate)
    {
        // Arrange
        var rents = new RentDateDTO
        {
            DeliveryDate = oldDeliveryDate,
            ReturnDate = oldReturnDate
        };

        // Act
        var isMatching = RentServices.AreDatesMatching(newDeliveryDate, newReturnDate, new List<RentDateDTO> { rents });

        // Assert
        Assert.IsTrue(isMatching);
    }

    [TestCase("04-01-2022", "04-06-2022", "05-01-2022", "05-06-2022")]
    public void Should_return_true_when_dates_do_not_overlap(
        DateTime newDeliveryDate, DateTime newReturnDate,
        DateTime oldDeliveryDate, DateTime oldReturnDate)
    {
        // Arrange
        var rents = new RentDateDTO
        {
            DeliveryDate = oldDeliveryDate,
            ReturnDate = oldReturnDate
        };

        // Act
        var isMatching = RentServices.AreDatesMatching(
            newDeliveryDate, newReturnDate, new List<RentDateDTO> { rents });

        // Assert
        Assert.IsFalse(isMatching);
    }

    [TestCase("04-02-2022", "04-03-2022", 1, 1)]
    public void Should_return_correct_difference_Common(
        DateTime returnDate, DateTime realReturnDate, decimal price, decimal expectedDifference)
    {
        // Act
        var difference = RentServiceDecorator.GetDifference(realReturnDate, returnDate, price, 0);

        // Assert
        Assert.That(difference, Is.EqualTo(expectedDifference));
    }

    [TestCase("04-02-2022", "04-03-2022", 1, 0)]
    public void Should_return_correct_difference_Premium(
    DateTime returnDate, DateTime realReturnDate, decimal price, decimal expectedDifference)
    {
        // Act
        var difference = RentServiceDecorator.GetDifference(realReturnDate, returnDate, price, 1);

        // Assert
        Assert.That(difference, Is.EqualTo(expectedDifference));
    }

    [Test]
    public async Task Should_return_correct_rent_when_customer_has_rent()
    {
        // Arrange
        var businessId = 123;
        var deliveryDate = DateTime.Now.AddDays(1);
        var returnDate = DateTime.Now.AddDays(2);
        var rents = new List<Rent>
        {
            new() {
                Customer = new Customer() { BusinessID = businessId },
                DeliveryDate = deliveryDate,
                ReturnDate = returnDate
            }
        };

        var mockSet = new Mock<DbSet<Rent>>();
        mockSet.As<IQueryable<Rent>>().Setup(m => m.Provider).Returns(rents.AsQueryable().Provider);
        mockSet.As<IQueryable<Rent>>().Setup(m => m.Expression).Returns(rents.AsQueryable().Expression);
        mockSet.As<IQueryable<Rent>>().Setup(m => m.ElementType).Returns(rents.AsQueryable().ElementType);
        mockSet.As<IQueryable<Rent>>().Setup(m => m.GetEnumerator()).Returns(() => rents.GetEnumerator());
        _mockContext.Setup(x => x.Rents).Returns(mockSet.Object);

        // Act
        var hasRent = _rentServices.HasAlreadyARent(businessId, deliveryDate, returnDate);

        // Assert
        Assert.IsTrue(hasRent);
    }
}