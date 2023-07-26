using TestCore.Business;
using TestCore.Business.Models;

namespace TestCore.Test.Rent
{
    public class RentServicesShould
    {
        [Theory]
        [TestCase("05-01-2022", "05-06-2022", "05-01-2022", "05-06-2022")]
        [TestCase("05-01-2022", "05-05-2022", "05-01-2022", "05-06-2022")]
        [TestCase("05-01-2022", "05-03-2022", "04-29-2022", "05-05-2022")]
        [TestCase("04-28-2022", "05-01-2022", "05-01-2022", "05-06-2022")]
        [TestCase("05-02-2022", "05-10-2022", "05-01-2022", "05-06-2022")]
        public void Should_have_error_MatchingDates(
            DateTime newDeliveryDate, DateTime newReturnDate, 
            DateTime oldDeliveryDate, DateTime oldReturnDate)
        {
            var rents = new RentDate
            {
                DeliveryDate = oldDeliveryDate,
                ReturnDate = oldReturnDate
            };
            var isMatching = RentServices.AreDatesMatching(newDeliveryDate, newReturnDate, new List<RentDate>{rents});
            Assert.That(isMatching, Is.False);
        }

        [Theory]
        [TestCase("04-01-2022", "04-06-2022", "05-01-2022", "05-06-2022")]
        [TestCase("04-01-2022", "04-30-2022", "05-01-2022", "05-06-2022")]
        [TestCase("06-01-2022", "06-30-2022", "05-01-2022", "05-06-2022")]
        public void Should_not_have_error_MatchingDates(
            DateTime newDeliveryDate, DateTime newReturnDate,
            DateTime oldDeliveryDate, DateTime oldReturnDate)
        {
            var rents = new RentDate
            {
                DeliveryDate = oldDeliveryDate,
                ReturnDate = oldReturnDate
            };
            var isMatching = RentServices.AreDatesMatching(
                newDeliveryDate, newReturnDate, new List<RentDate> { rents });
            Assert.That(isMatching, Is.True);
        }

        [Theory]
        [TestCase("04-02-2022", "04-02-2022", 1, 0)]
        [TestCase("04-02-2022", "04-03-2022", 1, 1)]
        [TestCase("04-02-2022", "04-04-2022", 1, 2)]
        [TestCase("04-02-2022", "04-04-2022", 1.4, 2.8)]
        [TestCase("04-02-2022", "04-04-2022", 1.6, 3.2)]
        [TestCase("04-02-2022", "04-05-2022", 1, 3)]
        [TestCase("04-02-2022", "04-01-2022", 1, 0)]
        [TestCase("04-05-2022", "04-03-2022", 1, 0)]
        public void Should_not_have_error_GetDifference(
            DateTime returnDate, DateTime realReturnDate, decimal price, decimal result)
        {
            var difference = RentServices.GetDifference(realReturnDate, returnDate, price);
            Assert.That(difference, Is.EqualTo(result));
        }
    }
}
