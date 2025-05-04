using FluentAssertions;
using GWPApi.Services;
using GWPApi.Repositories;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Caching.Memory;
using GWPApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GwpApi.Tests
{
    [TestFixture]
    public class GwpCalculatorServiceTests
    {
        private Mock<IGwpDataRepository> _mockRepository;
        private Mock<IMemoryCache> _mockCache;
        private GwpCalculatorService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IGwpDataRepository>();
            _mockCache = new Mock<IMemoryCache>();
            _service = new GwpCalculatorService(_mockRepository.Object, _mockCache.Object);
        }

        [Test]
        public async Task CalculateAverage_ReturnsCorrectAverages()
        {
            // Arrange
            var testData = new List<GwpData>
            {
                new GwpData
                {
                    Country = "ae",
                    LineOfBusiness = "transport",
                    YearData = new Dictionary<int, double?>
                    {
                        {2008, 100.0}, {2009, 100.1}, {2010, 100.2},
                        {2011, 100.3}, {2012, 100.4}, {2013, 100.5},
                        {2014, 100.6}, {2015, 100.7}
                    }
                },
                new GwpData
                {
                    Country = "ae",
                    LineOfBusiness = "property",
                    YearData = new Dictionary<int, double?>
                    {
                       {2008, 100.0}, {2009, 100.1}, {2010, 100.2},
                        {2011, 100.3}, {2012, 100.4}, {2013, 100.5},
                        {2014, 100.6}, {2015, 100.7}
                    }
                }
            };

            _mockRepository.Setup(r => r.GetByCountry("ae")).ReturnsAsync(testData);

            object cachedValue = null;
            _mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedValue)).Returns(false);

            // Mock cache entry creation
            var cacheEntryMock = new Mock<ICacheEntry>();
            _mockCache.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

            var request = new GwpRequest { Country = "ae", Lob = new List<string> { "transport", "property" } };

            // Act
            var result = await _service.CalculateAverage(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task CalculateAverage_ReturnsEmpty_WhenNoDataFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByCountry("xx")).ReturnsAsync(new List<GwpData>());

            object cachedValue = null;
            _mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedValue)).Returns(false);

            // Setup cache to handle Set operation
            var cacheEntryMock = new Mock<ICacheEntry>();
            _mockCache.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

            var request = new GwpRequest { Country = "xx", Lob = new List<string> { "transport" } };

            // Act
            var result = await _service.CalculateAverage(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task CalculateAverage_ReturnsCachedValue_WhenAvailable()
        {
            // Arrange
            var expectedResult = new GwpResponse { { "transport", 100000000 } };

            object cachedValue = expectedResult;
            _mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedValue)).Returns(true);

            var request = new GwpRequest { Country = "ae", Lob = new List<string> { "transport" } };

            // Act
            var result = await _service.CalculateAverage(request);

            // Assert
            Assert.That(result, Is.SameAs(expectedResult));
        }
    }
}