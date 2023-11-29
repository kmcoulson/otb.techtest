using NUnit.Framework;
using otb.search.holidays.Entities;
using otb.search.holidays.Repositories;

namespace otb.search.holidays.tests.Integration.Repositories
{
    [TestFixture]
    public class FlightRepositoryIntegrationTests
    {
        private IFlightRepository _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new FlightRepository();
        }

        public class GetAll : FlightRepositoryIntegrationTests
        {
            [Test]
            public async Task GivenDataExists_ShouldReturnTheDeserialisedData()
            {
                // Arrange


                // Act
                var result = await _classUnderTest.GetAll();
                var resultList = result.ToList();

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultList, Is.InstanceOf<IEnumerable<FlightEntity>>());
                    Assert.That(resultList, Has.Count.EqualTo(12));
                    Assert.That(resultList[0].Id, Is.EqualTo(1));
                    Assert.That(resultList[0].Airline, Is.EqualTo("First Class Air"));
                    Assert.That(resultList[0].From, Is.EqualTo("MAN"));
                    Assert.That(resultList[0].To, Is.EqualTo("TFS"));
                    Assert.That(resultList[0].Price, Is.EqualTo(470));
                    Assert.That(resultList[0].DepartureDate, Is.EqualTo(new DateOnly(2023, 07, 01)));
                });
            }
        }
    }
}
