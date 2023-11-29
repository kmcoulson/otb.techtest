using NUnit.Framework;
using otb.search.holidays.Entities;
using otb.search.holidays.Repositories;

namespace otb.search.holidays.tests.Integration.Repositories
{
    [TestFixture]
    public class HotelRepositoryIntegrationTests
    {
        private IHotelRepository _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new HotelRepository();
        }

        public class GetAll : HotelRepositoryIntegrationTests
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
                    Assert.That(resultList, Is.InstanceOf<IEnumerable<HotelEntity>>());
                    Assert.That(resultList, Has.Count.EqualTo(13));
                    Assert.That(resultList[0].Id, Is.EqualTo(1));
                    Assert.That(resultList[0].Name, Is.EqualTo("Iberostar Grand Portals Nous"));
                    Assert.That(resultList[0].ArrivalDate, Is.EqualTo(new DateOnly(2022, 11, 05)));
                    Assert.That(resultList[0].PricePerNight, Is.EqualTo(100));
                    Assert.That(resultList[0].LocalAirports, Is.EqualTo(new List<string> { "TFS" }));
                    Assert.That(resultList[0].Nights, Is.EqualTo(7));
                });
            }
        }
    }
}
