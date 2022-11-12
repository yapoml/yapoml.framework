using FluentAssertions;
using NUnit.Framework;
using Yapoml.Framework.Workspace.Services;

namespace Yapoml.Framework.Test.Services
{
    class NameNormalizerFixture
    {
        [Test]
        [TestCase("my_component", "MyComponent")]
        [TestCase("my-page", "MyPage")]
        [TestCase("my-page (copy)", "MyPageCopy")]
        public void Should_Normalize(string name, string expectedName)
        {
            var service = new NameNormalizer();

            service.Normalize(name).Should().Be(expectedName);
        }
    }
}
