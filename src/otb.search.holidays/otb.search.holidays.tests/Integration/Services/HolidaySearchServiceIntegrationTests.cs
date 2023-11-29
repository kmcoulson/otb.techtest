using NUnit.Framework;
using otb.search.holidays.Repositories;
using otb.search.holidays.Services;

namespace otb.search.holidays.tests.Integration.Services
{
    [TestFixture]
    public class HolidaySearchServiceIntegrationTests
    {
        private IHolidaySearchService _classUnderTest;

        [SetUp]
        public void Setup()
        {
            var flightRepository = new FlightRepository();
            var hotelRepository = new HotelRepository();
            var airportRepository = new AirportRepository();

            _classUnderTest = new HolidaySearchService(flightRepository, hotelRepository, airportRepository);

        }

        public class SearchHolidays : HolidaySearchServiceIntegrationTests
        {
            [Test]
            [TestCase("MAN", "AGP", "2023-07-01", 7, 2, 9, Description = "Customer #1")]
            [TestCase("ANY LONDON", "PMI", "2023-06-15", 10, 6, 5, Description = "Customer #2")]
            [TestCase("ANY", "LPA", "2022-11-10", 14, 7, 6, Description = "Customer #3")]
            public async Task GivenSearchIsCalled_ShouldCallFlightsRepositoryGetAllMethod(string from, string to, string dateString, int nights, int expectedFlightId, int expectedHotelId)
            {
                // Arrange
                var date = DateOnly.Parse(dateString);

                // Act
                var result = await _classUnderTest.SearchHolidays(from, to, date, nights);
                var topResult = result.FirstOrDefault();

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(topResult, Is.Not.Null);
                    Assert.That(topResult.Flight.Id, Is.EqualTo(expectedFlightId));
                    Assert.That(topResult.Hotel.Id, Is.EqualTo(expectedHotelId));
                });
            }
        }
    }
}
