using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Test
{
    [TestFixture]
    public class Tests
    {
        private IWebDriver _webDriver;

        [SetUp]
        public void SetUp()
        {
            new DriverManager().SetUpDriver(new FirefoxConfig());
            _webDriver = new FirefoxDriver();

            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }

        [Test]
        public void SearchForWithNative()
        {
            _webDriver.Navigate().GoToUrl("https://www.google.com");

            var input = _webDriver.FindElement(By.CssSelector(".gLFyf"));
            input.SendKeys("page object pattern");
            input.SendKeys(Keys.Enter);

            var resultsPane = _webDriver.FindElement(By.Id("rso"));

            var resultItems = resultsPane.FindElements(By.CssSelector(".g"));

            Assert.That(resultItems.Count, Is.GreaterThan(0));

            foreach (var resultItem in resultItems)
            {
                var resultLink = resultItem.FindElement(By.XPath(".//a/h3"));

                Assert.That(resultLink.Text, Does.Contain("page").IgnoreCase);
            }
        }
    }
}