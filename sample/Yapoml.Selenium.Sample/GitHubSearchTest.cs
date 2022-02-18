using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using Yapoml.Selenium;
using Yapoml.Extensions.Logging.Serilog;
using System.Linq;

namespace Yapoml.Selenium.Sample
{
    [TestFixture]
    public class GitHubSearchTest
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
        public void SearchOnGitHub()
        {
            _webDriver.Navigate().GoToUrl("https://github.com");

            _webDriver.FindElement(By.CssSelector(".header-search-input")).SendKeys("yapoml" + Keys.Enter);

            var leftMenu = _webDriver.FindElement(By.CssSelector(".menu"));

            var menuItems = leftMenu.FindElements(By.TagName("a"));

            foreach (var menuItem in menuItems)
            {
                Assert.That(menuItem.FindElement(By.CssSelector(".Counter")).Text, Is.Not.Empty);
            }
        }

        [Test]
        public void SearchOnGitHubWithYapoml()
        {
            _webDriver.Navigate().GoToUrl("https://github.com");

            var pages = _webDriver.Ya(
                //opts => opts.UseSerilog()
                opts => opts.UseLighter(300)
                )
                .Pages.GitHub;

            pages.HomePage.SearchInput.SendKeys("yapoml" + Keys.Enter);

            var menuItems = pages.SearchResultsPage.LeftMenu.MenuItems;

            foreach (var menuItem in menuItems)
            {
                Assert.That(menuItem.Counter.Text, Is.Not.Empty);
            }
        }
    }
}