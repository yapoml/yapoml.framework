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
            _webDriver.Navigate().GoToUrl("https://nuget.org");

            var ya = _webDriver.Ya(
                //opts => opts.UseSerilog()
                opts => opts.UseLighter(300)
                )
                .Pages.NuGet;

            ya.HomePage.SearchInput.SendKeys("yaml");
            ya.HomePage.SearchButton.Click();

            foreach (var package in ya.SearchResultsPage.SearchResultsPane.Packages)
            {
                Assert.That(package.Title.Text, Is.Not.Empty);
                Assert.That(package.Description.Text, Is.Not.Empty);
            }
        }
    }
}