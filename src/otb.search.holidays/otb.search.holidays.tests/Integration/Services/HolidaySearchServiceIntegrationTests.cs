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

        public class Search : HolidaySearchServiceIntegrationTests
        {
            [Test]
            public async Task GivenSearchIsCalledForCustomerTest1_ShouldCallFlightsRepositoryGetAllMethod()
            {
                // Arrange
                var from = "MAN";
                var to = "AGP";
                var date = new DateOnly(2023, 7, 1);
                var nights = 7;

                var expectedFlightId = 2;
                var expectedHotelId = 9;

                // Act
                var result = await _classUnderTest.Search(from, to, date, nights);
                var topResult = result.FirstOrDefault();

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(topResult, Is.Not.Null);
                    Assert.That(topResult.Flight.Id, Is.EqualTo(expectedFlightId));
                    Assert.That(topResult.Hotel.Id, Is.EqualTo(expectedHotelId));
                });
            }

            [Test]
            public async Task GivenSearchIsCalledForCustomerTest2_ShouldCallFlightsRepositoryGetAllMethod()
            {
                // Arrange
                var from = "Any London Airport";
                var to = "PMI";
                var date = new DateOnly(2023, 6, 15);
                var nights = 10;

                var expectedFlightId = 6;
                var expectedHotelId = 5;

                // Act
                var result = await _classUnderTest.Search(from, to, date, nights);
                var topResult = result.FirstOrDefault();

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(topResult, Is.Not.Null);
                    Assert.That(topResult.Flight.Id, Is.EqualTo(expectedFlightId));
                    Assert.That(topResult.Hotel.Id, Is.EqualTo(expectedHotelId));
                });
            }

            [Test]
            public async Task GivenSearchIsCalledForCustomerTest3_ShouldCallFlightsRepositoryGetAllMethod()
            {
                // Arrange
                var from = "Any";
                var to = "LPA";
                var date = new DateOnly(2022, 11, 10);
                var nights = 14;

                var expectedFlightId = 7;
                var expectedHotelId = 6;

                // Act
                var result = await _classUnderTest.Search(from, to, date, nights);
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
