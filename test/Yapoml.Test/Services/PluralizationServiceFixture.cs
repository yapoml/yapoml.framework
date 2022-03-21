using FluentAssertions;
using NUnit.Framework;
using Yapoml.Generation.Services;

namespace Yapoml.Test.Services
{
    class PluralizationServiceFixture
    {
        [Test]
        [TestCase("Status", false)]
        [TestCase("Statuses", true)]
        public void Should_Verify_IsPlural(string word, bool expectedIsPlural)
        {
            var service = new PluralizationService();

            service.IsPlural(word).Should().Be(expectedIsPlural);
        }
    }
}
