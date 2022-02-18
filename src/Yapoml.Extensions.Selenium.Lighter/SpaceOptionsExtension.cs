using OpenQA.Selenium;
using Yapoml.Options;
using Yapoml.Selenium.Events;

namespace Yapoml.Selenium
{
    public static class SpaceOptionsExtension
    {
        private static int _delay;

        public static ISpaceOptions UseLighter(this ISpaceOptions spaceOptions, int delay = 200)
        {
            _delay = delay;

            var eventSource = spaceOptions.Get<IEventSource>();

            eventSource.ComponentEventSource.OnFoundComponent += ComponentEventSource_OnFoundComponent;

            return spaceOptions;
        }

        private static void ComponentEventSource_OnFoundComponent(object sender, Yapoml.Selenium.Events.Args.WebElement.FoundElementEventArgs e)
        {
            var jsExecutor = e.WebDriver as IJavaScriptExecutor;

            if (jsExecutor != null)
            {
                var backgroundColor = e.WebElement.GetCssValue("backgroundColor");

                jsExecutor.ExecuteScript("arguments[0].setAttribute('style', 'background: rgba(200, 0, 0, 0.7);');", e.WebElement);

                System.Threading.Thread.Sleep(_delay);

                jsExecutor.ExecuteScript($"arguments[0].setAttribute('style', '{backgroundColor}');", e.WebElement);
            }
        }
    }
}
