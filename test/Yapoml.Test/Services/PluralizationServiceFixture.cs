using FluentAssertions;
using NUnit.Framework;
using Yapoml.Workspace.Services;

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

        [Test]
        [TestCase("Statuses", "Status")]
        [TestCase("Items", "Item")]
        [TestCase("MenuItems", "MenuItem")]
        public void Should_Singularize(string word, string expectedSingular)
        {
            var service = new PluralizationService();

            service.Singularize(word).Should().Be(expectedSingular);
        }
    }
}
