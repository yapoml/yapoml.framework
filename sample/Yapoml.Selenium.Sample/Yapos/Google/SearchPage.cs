using OpenQA.Selenium;

namespace Yapoml.Selenium.Sample.Yapos.Google
{
    public partial class SearchPage
    {
        public void Search(string text)
        {
            SearchInput.SendKeys(text);
            SearchInput.SendKeys(Keys.Enter);
        }
    }
}
