using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Yapoml.Playwright;

namespace Yapoml.Selenium.Sample
{
    [TestFixture]
    public class NuGetSearchTest
    {
        [OneTimeSetUp]
        public void SetUpPlaywright()
        {
            var exitCode = Program.Main(new[] { "install" });
            if (exitCode != 0)
            {
                throw new Exception($"Playwright exited with code {exitCode}");
            }
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public async Task Search()
        {
            using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            await using var browser = await playwright.Firefox.LaunchAsync();
            var page = await browser.NewPageAsync();

            await page.GotoAsync("https://nuget.org");

            await (await page.QuerySelectorAsync("#search")).TypeAsync("yaml");
            await page.ClickAsync(".btn-search");

            foreach (var package in await page.QuerySelectorAllAsync(".package"))
            {
                var title = await (await package.QuerySelectorAsync("xpath=.//a")).TextContentAsync();
                Assert.That(title, Is.Not.Empty);

                var description = await (await package.QuerySelectorAsync(".package-details")).TextContentAsync();
                Assert.That(description, Is.Not.Empty);
            }
        }

        [Test]
        public async Task SearchWithYapoml()
        {
            using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            await using var browser = await playwright.Firefox.LaunchAsync();
            var page = await browser.NewPageAsync();

            await page.GotoAsync("https://nuget.org");

            var ya = page.Ya().Pages.NuGet;

            await ya.HomePage.SearchInput.TypeAsync("yaml");
            await ya.HomePage.SearchButton.ClickAsync();

            foreach (var package in ya.SearchResultsPage.Packages)
            {
                Assert.That(package.Title.TextContent, Is.Not.Empty);
                Assert.That(package.Description.TextContent, Is.Not.Empty);
            }
        }
    }
}