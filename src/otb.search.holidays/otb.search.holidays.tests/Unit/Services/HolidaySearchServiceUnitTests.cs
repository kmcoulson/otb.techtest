using Moq;
using NUnit.Framework;
using otb.search.holidays.Dtos;
using otb.search.holidays.Entities;
using otb.search.holidays.Repositories;
using otb.search.holidays.Services;

namespace otb.search.holidays.tests.Unit.Services
{
    [TestFixture]
    public class HolidaySearchServiceUnitTests
    {
        private Mock<IFlightRepository> _flightRepositoryMock;
        private Mock<IHotelRepository> _hotelRepositoryMock;
        private Mock<IAirportRepository> _airportRepositoryMock;
        private IHolidaySearchService _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _flightRepositoryMock = new Mock<IFlightRepository>();
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _airportRepositoryMock = new Mock<IAirportRepository>();

            _classUnderTest = new HolidaySearchService(_flightRepositoryMock.Object, _hotelRepositoryMock.Object, _airportRepositoryMock.Object);
        }

        public class Search : HolidaySearchServiceUnitTests
        {
            [Test]
            public async Task GivenSearchIsCalled_ShouldCallFlightsRepositoryGetAllMethod()
            {
                // Arrange
                var from = "MAN";
                var to = "CDG";
                var date = new DateOnly(2023, 11, 29);
                var days = 5;

                // Act
                await _classUnderTest.Search(from, to, date, days);

                // Assert
                _flightRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            }


            [Test]
            public async Task GivenSearchIsCalled_ShouldCallHotelsRepositoryGetAllMethod()
            {
                // Arrange
                var from = "MAN";
                var to = "LAX";
                var date = new DateOnly(2023, 06, 29);
                var days = 14;

                // Act
                await _classUnderTest.Search(from, to, date, days);

                // Assert
                _hotelRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            }

            [Test]
            public async Task GivenSearchIsCalledAndThereAreNoMatches_ShouldReturnAListOfHolidaySearchResultDtos()
            {
                // Arrange
                var from = "MAN";
                var to = "JFK";
                var date = new DateOnly(2023, 07, 19);
                var days = 10;

                // Act
                var result = await _classUnderTest.Search(from, to, date, days);

                // Assert
                Assert.That(result, Is.InstanceOf<IEnumerable<HolidaySearchResultDto>>());
            }

            [Test]
            public async Task GivenSearchIsCalledAndThereAreNoMatches_ShouldReturnAnEmptyListOfHolidaySearchResultDtos()
            {
                // Arrange
                var from = "MAN";
                var to = "EWR";
                var date = new DateOnly(2023, 07, 19);
                var days = 7;

                // Act
                var result = await _classUnderTest.Search(from, to, date, days);
                var resultList = result.ToList();

                // Assert
                Assert.That(resultList, Has.Count.EqualTo(0));
            }

            [Test]
            public async Task GivenSearchIsCalledAndThereAreFlightMatchesButNoHotelMatches_ShouldReturnAnEmptyListOfHolidaySearchResultDtos()
            {
                // Arrange
                var from = "MAN";
                var to = "JFK";
                var date = new DateOnly(2023, 07, 19);
                var days = 7;

                var flight = new FlightEntity
                {
                    Id = 1,
                    Airline = "Skippyair",
                    From = from,
                    To = to,
                    DepartureDate = date,
                    Price = 123
                };

                _flightRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<FlightEntity> { flight });

                // Act
                var result = await _classUnderTest.Search(from, to, date, days);
                var resultList = result.ToList();

                // Assert
                Assert.That(resultList, Has.Count.EqualTo(0));
            }

            [Test]
            public async Task GivenSearchIsCalledAndThereAreFlightMatchesAndHotelMatchesButNightsDoNotMatch_ShouldReturnAnEmptyListOfHolidaySearchResultDtos()
            {
                // Arrange
                var from = "MAN";
                var to = "EWR";
                var date = new DateOnly(2023, 09, 19);
                var days = 10;

                var flight = new FlightEntity
                {
                    Id = 1,
                    Airline = "Skippyair",
                    From = from,
                    To = to,
                    DepartureDate = date,
                    Price = 321
                };

                _flightRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<FlightEntity> { flight });

                var hotel = new HotelEntity
                {
                    Id = 1,
                    Name = "Skippy's Spa & Resort",
                    ArrivalDate = date,
                    PricePerNight = 1234,
                    LocalAirports = new List<string> { to },
                    Nights = 14
                };

                _hotelRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<HotelEntity> { hotel });

                // Act
                var result = await _classUnderTest.Search(from, to, date, days);
                var resultList = result.ToList();

                // Assert
                Assert.That(resultList, Has.Count.EqualTo(0));
            }

            [Test]
            public async Task GivenSearchIsCalledAndThereAreFlightMatchesAndHotelMatches_ShouldReturnAListOfHolidaySearchResultDtosWithFlightsAndHotel()
            {
                // Arrange
                var from = "MAN";
                var to = "EWR";
                var date = new DateOnly(2023, 09, 19);
                var days = 10;

                var flight = new FlightEntity
                {
                    Id = 1,
                    Airline = "Skippyair",
                    From = from,
                    To = to,
                    DepartureDate = date,
                    Price = 321
                };

                _flightRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<FlightEntity> { flight });

                var hotel = new HotelEntity
                {
                    Id = 1,
                    Name = "Skippy's Spa & Resort",
                    ArrivalDate = date,
                    PricePerNight = 1234,
                    LocalAirports = new List<string> { to },
                    Nights = 10
                };

                _hotelRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<HotelEntity> { hotel });

                // Act
                var result = await _classUnderTest.Search(from, to, date, days);
                var resultList = result.ToList();

                // Assert
                Assert.That(resultList, Has.Count.GreaterThan(0));
            }

            [Test]
            public async Task GivenSearchIsCalledWithFromValueOfAnyAndThereAreFlightAndHotelMatches_ShouldReturnAListOfHolidaySearchResultDtos()
            {
                // Arrange
                var from = "ANY";
                var to = "JFK";
                var date = new DateOnly(2023, 03, 01);
                var days = 14;

                var flight = new FlightEntity
                {
                    Id = 1,
                    Airline = "Skippyair",
                    From = "CDG",
                    To = to,
                    DepartureDate = date,
                    Price = 456
                };

                _flightRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<FlightEntity> { flight });

                var hotel = new HotelEntity
                {
                    Id = 1,
                    Name = "Skippy's Spa & Resort",
                    ArrivalDate = date,
                    PricePerNight = 1234,
                    LocalAirports = new List<string> { to },
                    Nights = 14
                };

                _hotelRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<HotelEntity> { hotel });

                // Act
                var result = await _classUnderTest.Search(from, to, date, days);
                var resultList = result.ToList();

                // Assert
                Assert.That(resultList, Has.Count.GreaterThan(0));
            }

            [Test]
            public async Task GivenSearchIsCalledWithFromValueOfAnyCityAirportAndThereAreFlightAndHotelMatches_ShouldReturnAListOfHolidaySearchResultDtos()
            {
                // Arrange
                var city = "New York";
                var from = $"Any {city} Airport";
                var to = "LHR";
                var date = new DateOnly(2023, 03, 01);
                var days = 14;

                var flight = new FlightEntity
                {
                    Id = 1,
                    Airline = "Skippyair",
                    From = "LGA",
                    To = to,
                    DepartureDate = date,
                    Price = 456
                };

                _flightRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<FlightEntity> { flight });

                var hotel = new HotelEntity
                {
                    Id = 1,
                    Name = "Skippy's Spa & Resort",
                    ArrivalDate = date,
                    PricePerNight = 1234,
                    LocalAirports = new List<string> { to },
                    Nights = 14
                };

                _hotelRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<HotelEntity> { hotel });

                var newYorkAirportData = new List<string> { "EWR", "JFK", "LGA" };

                _airportRepositoryMock.Setup(x => x.GetAirportCodesForCity(city)).Returns(newYorkAirportData);

                // Act
                var result = await _classUnderTest.Search(from, to, date, days);
                var resultList = result.ToList();

                // Assert
                Assert.That(resultList, Has.Count.GreaterThan(0));
            }

            [Test]
            public async Task GivenSearchIsCalledAndThereAreMultipleMatches_ShouldReturnAListOrderedByTotalPriceWithTheLowestAtTheTop()
            {
                // Arrange
                var from = "MAN";
                var to = "EWR";
                var date = new DateOnly(2023, 09, 19);
                var days = 14;

                var expectedCheapestPrice = 14220;

                var flights = new List<FlightEntity>
                {
                    new FlightEntity
                    {
                        Id = 1,
                        Airline = "Skippyair",
                        From = from,
                        To = to,
                        DepartureDate = date,
                        Price = 321
                    },
                    new FlightEntity
                    {
                        Id = 2,
                        Airline = "Aeroflop",
                        From = from,
                        To = to,
                        DepartureDate = date,
                        Price = 234
                    }
                };

                _flightRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(flights);

                var hotels = new List<HotelEntity>
                {
                    new HotelEntity
                    {
                        Id = 1,
                        Name = "Barny's Spa & Resort",
                        ArrivalDate = date,
                        PricePerNight = 999,
                        LocalAirports = new List<string> { to },
                        Nights = 14
                    },
                    new HotelEntity
                    {
                        Id = 2,
                        Name = "Skippy's Spa & Resort",
                        ArrivalDate = date,
                        PricePerNight = 1234,
                        LocalAirports = new List<string> { to },
                        Nights = 14
                    }
                };

                _hotelRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(hotels);

                // Act
                var result = await _classUnderTest.Search(from, to, date, days);
                var resultList = result.ToList();

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultList, Has.Count.EqualTo(4));
                    Assert.That(resultList.First().Flight.Id, Is.EqualTo(2));
                    Assert.That(resultList.First().Hotel.Id, Is.EqualTo(1));
                });
            }

            [Test]
            public async Task GivenSearchIsCalled_ShouldReturnAListOrderedWithCorrectlyCalculatedTotalPrices()
            {
                // Arrange
                var from = "MAN";
                var to = "EWR";
                var date = new DateOnly(2023, 09, 19);
                var days = 14;

                var expectedPrices = new[] { 14220, 14307, 17510, 17597 };

                var flights = new List<FlightEntity>
                {
                    new FlightEntity
                    {
                        Id = 1,
                        Airline = "Skippyair",
                        From = from,
                        To = to,
                        DepartureDate = date,
                        Price = 321
                    },
                    new FlightEntity
                    {
                        Id = 2,
                        Airline = "Aeroflop",
                        From = from,
                        To = to,
                        DepartureDate = date,
                        Price = 234
                    }
                };

                _flightRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(flights);

                var hotels = new List<HotelEntity>
                {
                    new HotelEntity
                    {
                        Id = 1,
                        Name = "Barny's Spa & Resort",
                        ArrivalDate = date,
                        PricePerNight = 999,
                        LocalAirports = new List<string> { to },
                        Nights = 14
                    },
                    new HotelEntity
                    {
                        Id = 2,
                        Name = "Skippy's Spa & Resort",
                        ArrivalDate = date,
                        PricePerNight = 1234,
                        LocalAirports = new List<string> { to },
                        Nights = 14
                    }
                };

                _hotelRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(hotels);

                // Act
                var result = await _classUnderTest.Search(from, to, date, days);
                var resultList = result.ToList();

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultList, Has.Count.EqualTo(4));
                    Assert.That(resultList[0].TotalPrice, Is.EqualTo(expectedPrices[0]));
                    Assert.That(resultList[1].TotalPrice, Is.EqualTo(expectedPrices[1]));
                    Assert.That(resultList[2].TotalPrice, Is.EqualTo(expectedPrices[2]));
                    Assert.That(resultList[3].TotalPrice, Is.EqualTo(expectedPrices[3]));
                });
            }
        }
    }
}
