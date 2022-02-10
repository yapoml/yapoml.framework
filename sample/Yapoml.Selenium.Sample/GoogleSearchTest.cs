using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using Yapoml.Selenium;

namespace Yapoml.Selenium.Sample
{
    [TestFixture]
    public class GoogleSearchTest
    {
        private IWebDriver _webDriver;

        [SetUp]
        public void SetUp()
        {
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

            var searchInput = _webDriver.FindElement(By.CssSelector(".gLFyf"));
            searchInput.SendKeys("page object pattern");
            searchInput.SendKeys(Keys.Enter);

            var searchResultItems = _webDriver.FindElement(By.Id("rso")).FindElements(By.CssSelector(".g"));

            Assert.That(searchResultItems.Count, Is.GreaterThan(0));

            foreach (var searchResultItem in searchResultItems)
            {
                var resultTitle = searchResultItem.FindElement(By.XPath(".//a/h3"));

                Assert.That(resultTitle.Text, Does.Contain("page").IgnoreCase);
            }
        }

        [Test]
        public void SearchForWithYetAnotherPageObject()
        {
            _webDriver.Navigate().GoToUrl("https://www.google.com");
            
            var searchInput = _webDriver.Ya().Pages.Google.Search.SearchInput;
            searchInput.SendKeys("page object pattern");
            searchInput.SendKeys(Keys.Enter);

            var searchResultItems = _webDriver.Ya().Pages.Google.SearchResults.ResultsPane.ResultItems;

            Assert.That(searchResultItems.Count, Is.GreaterThan(0));

            foreach (var searchResultItem in searchResultItems)
            {
                var resultTitle = searchResultItem.Title;

                Assert.That(resultTitle.Text, Does.Contain("page").IgnoreCase);
            }
        }
    }
}