using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using Yapoml.Selenium;

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
            _webDriver.Navigate().GoToUrl("https://nuget.org");

            _webDriver.FindElement(By.Id("search")).SendKeys("yaml");
            _webDriver.FindElement(By.CssSelector(".btn-search")).Click();

            foreach (var package in _webDriver.FindElements(By.CssSelector(".package")))
            {
                Assert.That(package.FindElement(By.XPath(".//a")).Text, Is.Not.Empty);
                Assert.That(package.FindElement(By.CssSelector(".package-details")).Text, Is.Not.Empty);
            }
        }

        [Test]
        public void SearchOnGitHubWithYapoml()
        {
            _webDriver.Navigate().GoToUrl("https://nuget.org");

            var ya = _webDriver.Ya(
                //opts => opts.UseSerilog()
                opts => opts.UseLighter(delay: 200, fadeOutSpeed: 400)
                )
                .Pages.NuGet;

            ya.HomePage.Search("yaml");

            foreach (var package in ya.SearchResultsPage.Packages)
            {
                Assert.That(package.Title.Text, Is.Not.Empty);
                Assert.That(package.Description.Text, Is.Not.Empty);
            }
        }
    }
}