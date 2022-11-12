using FluentAssertions;
using NUnit.Framework;
using System;
using Yapoml.Framework.Options;

namespace Yapoml.Framework.Test.Services
{
    class ServiceContainerFixture
    {
        [Test]
        public void ShouldGetService()
        {
            string str = "";

            var container = new SpaceOptions();

            container.Services.Register(str);

            container.Services.Get<string>().Should().Be(str);
        }

        [Test]
        public void ShouldRaiseExceptionIfInstanceNotRegistered()
        {
            var container = new SpaceOptions();

            Action act = () => container.Services.Get<object>();

            act.Should().ThrowExactly<Exception>();
        }

        [Test]
        public void ShouldTryGetServiceIfRegistered()
        {
            string str = "";

            var container = new SpaceOptions();

            container.Services.Register(str);

            var result = container.Services.TryGet<string>(out var service);

            result.Should().BeTrue();
            service.Should().Be(str);
        }

        [Test]
        public void ShouldTryGetServiceIfNotRegistered()
        {
            var container = new SpaceOptions();

            var result = container.Services.TryGet<string>(out var service);

            result.Should().BeFalse();
            service.Should().BeNull();
        }
    }
}
